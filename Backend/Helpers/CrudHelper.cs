using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Dtos;
using Backend.Models;
using AutoMapper;

namespace Backend.Helpers
{
    public class CrudHelper(AppDbContext dbContext, IMapper mapper)
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<STNK>> GetAllStnk()
        {
            return await _dbContext.STNKs.ToListAsync();
        }

        public async Task<STNK> GetStnkByStnkNumber(string stnkNumber)
        {
            var stnk = await _dbContext.STNKs
                .Join(_dbContext.Owners,
                    stnk => stnk.OwnerId,
                    owner => owner.Id,
                    (stnk, owner) => new
                    {
                        stnk, owner
                    })
                .Where(x => x.stnk.StnkNumber == stnkNumber)
                .FirstOrDefaultAsync();

            return _mapper.Map<STNK>(stnk);
        }
    }
}
