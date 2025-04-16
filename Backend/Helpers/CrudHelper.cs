using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Dtos;
using AutoMapper;

namespace Backend.Helpers
{
    public class CrudHelper(AppDbContext dbContext, IMapper mapper)
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<InitDto> Init()
        {
            var carType = await _dbContext.CarType.ToListAsync();
            var engineSize = await _dbContext.EngineSize.ToListAsync();

            var init = new InitDto
            {
                CarType = _mapper.Map<List<CarType>>(carType),
                EngineSize = _mapper.Map<List<EngineSize>>(engineSize)
            };

            return init;
        }

        public async Task<IEnumerable<STNK>> GetAllStnk()
        {
            return await _dbContext.STNK.ToListAsync();
        }

        public async Task<object?> GetStnkByStnkNumber(string stnkNumber)
        {
            var stnk = await _dbContext.STNK
                .Join(_dbContext.Owner,
                    stnk => stnk.OwnerId,
                    owner => owner.Id,
                    (stnk, owner) => new
                    {
                        stnk, owner
                    })
                .Where(x => x.stnk.RegistrationNumber == stnkNumber)
                .FirstOrDefaultAsync();

            if (stnk == null) return null;

            return stnk;
        }
    }
}
