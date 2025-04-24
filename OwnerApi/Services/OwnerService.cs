using AutoMapper;
using OwnerApi.ApiClients.Interfaces;
using OwnerApi.Data;
using OwnerApi.Dtos;
using OwnerApi.Models;
using OwnerApi.Repositories.Interfaces;
using OwnerApi.Services.Interfaces;
using SharedLibrary.Enums;

namespace OwnerApi.Services
{
    public class OwnerService(AppDbContext dbContext, IOwnerRepository repo, ISequenceApiClient sequenceApiClient, IMapper mapper) : IOwnerService
    {
        private readonly IOwnerRepository _repo = repo;
        private readonly ISequenceApiClient _sequenceApiClient = sequenceApiClient;
        private readonly IMapper _mapper = mapper;
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<OwnerReadDto?> GetOwner(int id)
        {
            return _mapper.Map<OwnerReadDto>(await _repo.GetOwnerAsync(id));
        }

        public async Task<int> GetOwnerId(string name)
        {
            return await _repo.GetOwnerIdAsync(name);
        }

        public async Task<int> InsertOwner(OwnerWriteDto owner)
        {
            if (await GetOwnerId(owner.Name) != 0) throw new InvalidOperationException("Owner already exists!");

            var sequence = await _sequenceApiClient.GetSequence(SequenceTypeEnum.NIK) ?? throw new Exception("Failed to retrieve Owner NIK!");

            using var transacton = _dbContext.Database.BeginTransaction();

            try
            {
                var ownerId = await _repo.InsertOwnerAsync(owner.Name, sequence);
                await _repo.SaveChangesAsync();
                await transacton.CommitAsync();
                return ownerId;
            }
            catch (Exception ex)
            {
                await transacton.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
    }
}
