using AutoMapper;
using SharedLibrary.Enums;
using StnkApi.ApiClients.Interfaces;
using StnkApi.Data;
using StnkApi.Dtos;
using StnkApi.Helpers;
using StnkApi.Repositories.Interfaces;
using StnkApi.Services.Interfaces;

namespace StnkApi.Services
{
    public class StnkService(AppDbContext dbContext, IStnkRepository repo, IMapper mapper, ISequenceApiClient sequenceApiClient) : IStnkService
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IStnkRepository _repo = repo;
        private readonly IMapper _mapper = mapper;
        private readonly ISequenceApiClient _sequenceApiClient = sequenceApiClient;

        public async Task<InitDto> Init()
        {
            return new InitDto
            {
                CarType = await _repo.GetCarTypeAsync(),
                EngineSize = await _repo.GetEngineSizeAsync()
            };
        }

        public async Task<IEnumerable<AllStnkDto>> GetStnks()
        {
            return _mapper.Map<IEnumerable<AllStnkDto>>(await _repo.GetStnksAsync());
        }

        public async Task<StnkUpdateReadDto?> GetStnk(string registrationNumber)
        {
            var stnk = await _repo.GetStnkFullAsync(registrationNumber);
            return stnk;
        }

        public async Task<string?> GetSequence(SequenceTypeEnum type)
        {
            var sequence = await _sequenceApiClient.GetSequence(type);
            if (sequence.Status != 200) return null;

            var registrationNumber = sequence.Data?.ToString();
            if (string.IsNullOrEmpty(registrationNumber)) return null;

            return registrationNumber;
        }

        public async Task InsertStnk(StnkInsertReadDto stnkInput)
        {
            var carType = await _repo.GetCarTypeAsync(stnkInput.CarType);
            var engineSize = await _repo.GetEngineSizeAsync(stnkInput.EngineSize);

            if (carType == null || engineSize == null)
                throw new NullReferenceException("Car Type or Engine Size not match!");

            var registrationNumber = await GetSequence(SequenceTypeEnum.STNK) ?? throw new InvalidOperationException("Failed to retrieve Registration Number!");

            var ownerId = await _repo.GetOwnerIdAsync(stnkInput.OwnerName);

            if (ownerId == 0)
            {
                var nik = await GetSequence(SequenceTypeEnum.NIK) ?? throw new InvalidOperationException("Failed to retrieve valid NIK!");
                ownerId = await _repo.InsertOwner(stnkInput.OwnerName, nik);
            }
            else
                ownerId = await _repo.GetOwnerIdAsync(stnkInput.OwnerName);

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var tax = await CalculateTax(carType.Id, engineSize.Id, stnkInput.CarPrice, stnkInput.OwnerName);
                await _repo.InsertStnk(stnkInput, registrationNumber, ownerId, tax);
                await _repo.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<StnkUpdateReadDto> UpdateStnk(string registrationNumber, StnkUpdateWriteDto stnkInput)
        {
            var carType = await _repo.GetCarTypeAsync(stnkInput.CarType);
            var engineSize = await _repo.GetEngineSizeAsync(stnkInput.EngineSize);

            if (carType == null || engineSize == null)
                throw new NullReferenceException("Car Type or Engine Size not match!");

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var stnk = await _repo.GetStnkAsync(registrationNumber) ?? throw new NullReferenceException("STNK not found!");
                var owner = await _repo.GetOwnerAsync(stnk.OwnerId) ?? throw new NullReferenceException("Owner not found!");
                var tax = await CalculateTax(carType.Id, engineSize.Id, stnkInput.CarPrice, owner.Name, registrationNumber);
                var updateStnk = await _repo.UpdateStnkAsync(stnkInput, stnk, tax);

                await _repo.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<StnkUpdateReadDto>(stnk);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<decimal> CalculateTax(int carTypeId, int engineSizeId, decimal carPrice, string ownerName, string registrationNumber = "")
        {
            var carType = await _repo.GetCarTypeAsync(carTypeId);
            var engineSize = await _repo.GetEngineSizeAsync(engineSizeId);

            if (carType == null || engineSize == null)
                throw new NullReferenceException("Car Type or Engine Size not match!");

            var owner = await _repo.GetOwnerIdAsync(ownerName);
            var currentCarNumber = await _repo.GetCurrentCarNumber(owner, registrationNumber);

            var tax = StnkHelper.CalculateTax(carPrice, carType.Percentage, engineSize.Percentage, currentCarNumber);

            return tax;
        }
    }
}
