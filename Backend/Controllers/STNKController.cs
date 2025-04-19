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
    public class STNKController(StnkHelper stnkHelper, IMapper mapper) : Controller
    {
        private readonly IMapper _mapper = mapper;
        private readonly StnkHelper _stnkHelper = stnkHelper;

        [HttpGet("init")]
        public async Task<ActionResult<ApiResponseDto<InitDto>>> Init()
        {
            InitDto init;

            try
            {
                init = await _stnkHelper.Init();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponseDto<object>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Data = null,
                });
            }

            return Ok(new ApiResponseDto<object>
            {
                Status = StatusCodes.Status200OK,
                Message = "OK",
                Data = init
            });
        }

        [HttpGet("all-stnk")]
        public async Task<ActionResult<ApiResponseDto<AllStnkDto>>> GetAllStnk()
        {
            var stnkList = await _stnkHelper.GetAllStnk();

            return Ok(new ApiResponseDto<IEnumerable<AllStnkDto>>
            {
                Status = StatusCodes.Status200OK,
                Message = "OK",
                Data = stnkList
            });
        }

        [HttpGet("{stnkNumber}")]
        public async Task<ActionResult<ApiResponseDto<StnkUpdateReadDto>>> GetStnkByStnkNumber(string stnkNumber)
        {
            if (stnkNumber == null) return BadRequest(new ApiResponseDto<StnkUpdateReadDto>());

            var stnk = await _stnkHelper.GetStnkByStnkNumber(stnkNumber);

            if (stnk == null) return NotFound(new ApiResponseDto<StnkUpdateReadDto>
            {
                Status = StatusCodes.Status404NotFound,
                Message = "STNK not found!",
                Data = null
            });

            return Ok(new ApiResponseDto<StnkUpdateReadDto>
            {
                Status = StatusCodes.Status200OK,
                Message = "OK",
                Data = _mapper.Map<StnkUpdateReadDto>(stnk)
            });
        }
        
        [HttpPost("insert")]
        public async Task<ActionResult<ApiResponseDto<object>>> CreateStnk([FromBody] StnkInsertReadDto stnk)
        {
            if (!ModelState.IsValid) return BadRequest();

            try
            {
                await _stnkHelper.InsertStnk(stnk);
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
