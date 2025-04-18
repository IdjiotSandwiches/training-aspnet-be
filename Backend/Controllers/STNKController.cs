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

        [HttpGet("init")]
        public async Task<ActionResult<InitDto>> Init()
        {
            var init = await _crudHelper.Init();

            if (init == null) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(new ApiResponseDto<object>
            {
                Status = StatusCodes.Status200OK,
                Message = "OK",
                Data = _mapper.Map<InitDto>(init)
            });
        }

        [HttpGet("all-stnk")]
        public async Task<ActionResult<AllStnkDto>> GetAllStnk()
        {
            var stnk = await _crudHelper.GetAllStnk();

            if (!stnk.Any())
                return NotFound(new ApiResponseDto<AllStnkDto>
                {
                    Status = StatusCodes.Status404NotFound,
                    Message = "Data Empty!",
                    Data = null
                });

            return Ok(new ApiResponseDto<IEnumerable<AllStnkDto>>
            {
                Status = StatusCodes.Status200OK,
                Message = "OK",
                Data = _mapper.Map<IEnumerable<AllStnkDto>>(stnk)
            });
        }

        [HttpGet("{stnkNumber}")]
        public async Task<ActionResult<StnkDto>> GetStnkByStnkNumber(string stnkNumber)
        {
            if (stnkNumber == null) return BadRequest(new ApiResponseDto<StnkDto>());

            var stnk = await _crudHelper.GetStnkByStnkNumber(stnkNumber);

            if (stnk == null) return NotFound(new ApiResponseDto<StnkDto>
            {
                Status = StatusCodes.Status404NotFound,
                Message = "STNK not found!",
                Data = null
            });

            return Ok(new ApiResponseDto<StnkDto>
            {
                Status = StatusCodes.Status200OK,
                Message = "OK",
                Data = _mapper.Map<StnkDto>(stnk)
            });
        }
        
        [HttpPost("insert")]
        public async Task<ActionResult<ApiResponseDto<object>>> CreateStnk([FromBody] StnkDto stnk)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                await _crudHelper.InsertStnk(stnk);
            }
            catch (Exception ex)
            {
                return Conflict(new ApiResponseDto<object>
                {
                    Status = StatusCodes.Status409Conflict,
                    Message = ex.Message,
                    Data = null
                });
            }

            return Ok(new ApiResponseDto<object>
            {
                Status = StatusCodes.Status200OK,
                Message = "OK",
                Data = null,
            });
        }
    }
}
