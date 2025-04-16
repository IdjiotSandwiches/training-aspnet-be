using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Backend.Helpers;
using Backend.Dtos;

namespace Backend.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]

    public class STNKController(CrudHelper crudHelper, IMapper mapper) : Controller
    {
        private readonly IMapper _mapper = mapper;
        private readonly CrudHelper _crudHelper = crudHelper;

        [HttpGet]
        public async Task<ActionResult<AllStnkDto>> GetAllStnk()
        {
            var stnk = await _crudHelper.GetAllStnk();

            if (stnk == null) return NotFound();

            return Ok(_mapper.Map<AllStnkDto>(stnk));
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetStnkByStnkNumber(string stnkNumber)
        {
            return Ok();
        }
    }
}
