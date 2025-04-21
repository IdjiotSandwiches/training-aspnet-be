using AutoMapper;
using Backend.Common;
using Backend.Data;
using Backend.Dtos;
using Backend.Enums;
using Backend.Helpers;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;

namespace Backend.Services
{
    public class StnkService(AppDbContext dbContext, IStnkRepository repo, IMapper mapper, ILogger<StnkService> logger, ISequenceService sequenceService, StnkHelper helper) : IStnkService
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IStnkRepository _repo = repo;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<StnkService> _logger = logger;
        private readonly ISequenceService _sequenceService = sequenceService;
        private readonly StnkHelper _helper = helper;

        public async Task<Result<InitDto>> Init()
        {
            if (await _repo.IsCarTypeEmptyAsync() || await _repo.IsEngineSizeEmptyAsync())
                return Result<InitDto>.Error(500, "Initialize failed!");

            return Result<InitDto>.Success(200, "OK", new InitDto
            {
                CarType = await _repo.GetCarTypeAsync(),
                EngineSize = await _repo.GetEngineSizeAsync()
            });
        }

        public async Task<Result<IEnumerable<AllStnkDto>>> GetAllStnk()
        {
            return Result<IEnumerable<AllStnkDto>>.Success(200, "OK", _mapper.Map<IEnumerable<AllStnkDto>>(await _repo.GetAllStnkAsync()));
        }

        public async Task<Result<StnkUpdateReadDto>> GetStnkByRegistrationNumber(string registrationNumber)
        {
            if (await _repo.IsStnkEmptyAsync(registrationNumber))
                return Result<StnkUpdateReadDto>.Error(404, "STNK not found!");

            var stnk = await _repo.GetStnkByRegistrationNumberAsync(registrationNumber);
            
            return Result<StnkUpdateReadDto>.Success(200, "OK", stnk);
        }

        public async Task<Result<StnkInsertReadDto>> InsertStnk(StnkInsertReadDto stnkInput)
        {
            var carType = await _repo.GetCarTypeAsync(stnkInput.CarType);
            var engineSize = await _repo.GetEngineSizeAsync(stnkInput.EngineSize);

            if (carType == null || engineSize == null)
                return Result<StnkInsertReadDto>.Error(400, "Invalid operation!");

            var taxPercentage = new {
                CarTypeTax = carType.Percentage,
                EngineSizeTax = engineSize.Percentage
            };

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                int ownerId;
                var stnkSequence = await _sequenceService.GenerateSequence(SequenceTypeEnum.STNK, transaction);

                if (!stnkSequence.IsSuccess)
                {
                    await transaction.RollbackAsync();
                    return Result<StnkInsertReadDto>.Error(stnkSequence.Status, stnkSequence.Message);
                }

                if (await _repo.IsOwnerEmptyAsync(stnkInput.OwnerName))
                {
                    var nikSequence = await _sequenceService.GenerateSequence(SequenceTypeEnum.NIK, transaction);
                    if (!nikSequence.IsSuccess)
                    {
                        await transaction.RollbackAsync();
                        return Result<StnkInsertReadDto>.Error(nikSequence.Status, nikSequence.Message);
                    }

                    ownerId = await _repo.InsertOwner(stnkInput.OwnerName, nikSequence.Data!);
                }
                else
                    ownerId = await _repo.GetOwnerIdAsync(stnkInput.OwnerName);


                await _repo.InsertStnk(stnkInput, stnkSequence.Data!, ownerId, taxPercentage);
                await _repo.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result<StnkInsertReadDto>.Success(200, "OK");
            } 
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error while inserting STNK");
                return Result<StnkInsertReadDto>.Error(500, "Fail inserting STNK!");
            }
        }
        
        public async Task<Result<StnkUpdateReadDto>> UpdateStnk(string registrationNumber, StnkUpdateWriteDto stnkInput)
        {
            var carType = await _repo.GetCarTypeAsync(stnkInput.CarType);
            var engineSize = await _repo.GetEngineSizeAsync(stnkInput.EngineSize);

            if (carType == null || engineSize == null)
                return Result<StnkUpdateReadDto>.Error(400, "Invalid operation!");

            var taxPercentage = new
            {
                CarTypeTax = carType.Percentage,
                EngineSizeTax = engineSize.Percentage
            };

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                if (await _repo.IsStnkEmptyAsync(registrationNumber))
                    return Result<StnkUpdateReadDto>.Error(404, "STNK not found!");

                var stnk = await _repo.UpdateStnkAsync(stnkInput, registrationNumber, taxPercentage);
                
                await _repo.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result<StnkUpdateReadDto>.Success(200, "OK", _mapper.Map<StnkUpdateReadDto>(stnk));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error while updating STNK");
                return Result<StnkUpdateReadDto>.Error(500, "Fail updating STNK!");
            }
        }
    
        public async Task<Result<decimal>> CalculateTax(int carTypeId, int engineSizeId, decimal carPrice, string ownerName, string registrationNumber)
        {
            var carType = await _repo.GetCarTypeAsync(carTypeId);
            var engineSize = await _repo.GetEngineSizeAsync(engineSizeId);

            if (carType == null || engineSize == null)
                return Result<decimal>.Error(400, "");

            var owner = await _repo.GetOwnerIdAsync(ownerName);
            var currentCarNumber = await _repo.GetCurrentCarNumber(owner, registrationNumber);

            var tax = _helper.CalculateTax(carPrice, carType.Percentage, engineSize.Percentage, currentCarNumber);

            return Result<decimal>.Success(200, "OK", tax);
        }
    }
}
