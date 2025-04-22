using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Backend.Dtos;
using Backend.Services.Interfaces;

namespace Backend.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class STNKController(IStnkService stnkService) : Controller
    {
        private readonly IStnkService _stnkService = stnkService;

        [HttpGet("init")]
        public async Task<ActionResult<ApiResponseDto<InitDto>>> Init()
        {
            var init = await _stnkService.Init();
            if (!init.IsSuccess)
                return StatusCode(init.Status, new ApiResponseDto<InitDto>
                {
                    Status = init.Status,
                    Message = init.Message,
                    Data = null
                });

            return Ok(new ApiResponseDto<InitDto>
            {
                Status = init.Status,
                Message = init.Message,
                Data = init.Data
            });
        }

        [HttpGet("all-stnk")]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<AllStnkDto>>>> GetAllStnk()
        {
            var stnkList = await _stnkService.GetAllStnk();
            if (!stnkList.IsSuccess)
                return StatusCode(stnkList.Status, new ApiResponseDto<IEnumerable<AllStnkDto>>
                {
                    Status = stnkList.Status,
                    Message = stnkList.Message,
                    Data = null
                });

            return Ok(new ApiResponseDto<IEnumerable<AllStnkDto>>
            {
                Status = stnkList.Status,
                Message = stnkList.Message,
                Data = stnkList.Data
            });
        }

        [HttpGet("{registrationNumber}")]
        public async Task<ActionResult<ApiResponseDto<StnkUpdateReadDto>>> GetStnkByStnkNumber(string registrationNumber)
        {
            if (registrationNumber == null) return BadRequest(new ApiResponseDto<StnkUpdateReadDto>
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Registration number cannot be null!",
                Data = null
            });

            var stnk = await _stnkService.GetStnkByRegistrationNumber(registrationNumber);

            if (!stnk.IsSuccess)
                return StatusCode(stnk.Status, new ApiResponseDto<StnkUpdateReadDto>
                {
                    Status = stnk.Status,
                    Message = stnk.Message,
                    Data = null
                });

            return Ok(new ApiResponseDto<StnkUpdateReadDto>
            {
                Status = stnk.Status,
                Message = stnk.Message,
                Data = stnk.Data
            });
        }
        
        [HttpPost("insert")]
        public async Task<ActionResult<ApiResponseDto<object>>> InsertStnk([FromBody] StnkInsertReadDto stnk)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiResponseDto<object>
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Invalid input!",
                Data = null
            });

            var newStnk = await _stnkService.InsertStnk(stnk);
            if (!newStnk.IsSuccess) 
                return StatusCode(newStnk.Status, new ApiResponseDto<object>
                {
                    Status = newStnk.Status,
                    Message = newStnk.Message,
                    Data = null
                });

            return Ok(new ApiResponseDto<object>
            {
                Status = newStnk.Status,
                Message = newStnk.Message,
                Data = null,
            });
        }

        [HttpPut("update")]
        public async Task<ActionResult<ApiResponseDto<StnkUpdateReadDto>>> UpdateStnk(string registrationNumber, [FromBody] StnkUpdateWriteDto stnk)
        {
            if (!ModelState.IsValid || registrationNumber == null) return BadRequest(new ApiResponseDto<StnkUpdateReadDto>
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Invalid input!",
                Data = null
            });

            var updateStnk = await _stnkService.UpdateStnk(registrationNumber, stnk);
            if (!updateStnk.IsSuccess)
                return StatusCode(updateStnk.Status, new ApiResponseDto<StnkUpdateReadDto>
                {
                    Status = updateStnk.Status,
                    Message = updateStnk.Message,
                    Data = null
                });

            return Ok(new ApiResponseDto<StnkUpdateReadDto>
            {
                Status = updateStnk.Status,
                Message = updateStnk.Message,
                Data = updateStnk.Data
            });
        }

        [HttpGet("calculate-tax")]
        public async Task<ActionResult<ApiResponseDto<decimal>>> CalculateTax(int carType, int engineSize, decimal carPrice, string ownerName, string registrationNumber = "")
        {
            var tax = await _stnkService.CalculateTax(carType, engineSize, carPrice, ownerName, registrationNumber);
            if (!tax.IsSuccess)
                return StatusCode(tax.Status, new ApiResponseDto<decimal>
                {
                    Status = tax.Status,
                    Message = tax.Message,
                    Data = 0
                });

            return Ok(new ApiResponseDto<decimal>
            {
                Status = tax.Status,
                Message = tax.Message,
                Data = tax.Data
            });
        }
    }
}
