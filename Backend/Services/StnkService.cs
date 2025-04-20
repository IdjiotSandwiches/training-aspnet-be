using AutoMapper;
using Backend.Common;
using Backend.Data;
using Backend.Dtos;
using Backend.Enums;
using Backend.Models;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;

namespace Backend.Services
{
    public class StnkService(AppDbContext dbContext, IStnkRepository repo, IMapper mapper, ILogger<StnkService> logger, ISequenceService sequenceService) : IStnkService
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IStnkRepository _repo = repo;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<StnkService> _logger = logger;
        private readonly ISequenceService _sequenceService = sequenceService;

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

                    ownerId = _repo.InsertOwner(stnkInput.OwnerName, nikSequence.Data!);
                }
                else
                    ownerId = await _repo.GetOwnerIdAsync(stnkInput.OwnerName);

                var stnk = _mapper.Map<Stnk>(new StnkWriteDto
                {
                    RegistrationNumber = stnkSequence.Data!,
                    OwnerId = ownerId,
                    CarName = stnkInput.CarName,
                    CarType = stnkInput.CarType,
                    CarPrice = stnkInput.CarPrice,
                    LastTaxPrice = stnkInput.LastTaxPrice,
                    EngineSize = stnkInput.EngineSize,
                    AddedBy = "",
                    AddedDate = DateOnly.FromDateTime(DateTime.Now)
                });

                _repo.InsertStnk(stnk);
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
    }
}
