using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StnkApi.Services.Interfaces;
using StnkApi.Helpers;
using SharedLibrary.Dtos;
using StnkApi.Dtos;

namespace StnkApi.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class StnkController(IStnkService stnkService) : Controller
    {
        private readonly IStnkService _stnkService = stnkService;

        [HttpGet("init")]
        public async Task<IActionResult> Init()
        {
            try
            {
                var init = await _stnkService.Init();

                return Ok(new ApiResponseDto<InitDto>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "OK",
                    Data = init
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ErrorResponseHelper.ErrorResponse(
                        StatusCodes.Status500InternalServerError,
                        "Internal Server Error",
                        ex.Message
                    )
                );
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStnk()
        {
            try
            {
                var stnkList = await _stnkService.GetStnks();

                return Ok(new ApiResponseDto<IEnumerable<AllStnkDto>>
                {
                    Status = 200,
                    Message = "OK",
                    Data = stnkList
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ErrorResponseHelper.ErrorResponse(
                        StatusCodes.Status500InternalServerError,
                        "Internal Server Error",
                        ex.Message
                    )
                );
            }
        }

        [HttpGet]
        [Route("{registrationNumber}")]
        public async Task<IActionResult> GetStnk(string registrationNumber)
        {
            if (registrationNumber == null) return BadRequest(
                    ErrorResponseHelper.ErrorResponse(
                        StatusCodes.Status400BadRequest,
                        "Registration number cannot be empty!"
                    )
                ); ;

            try
            {
                var stnk = await _stnkService.GetStnk(registrationNumber);

                if (stnk == null) return NoContent();
                return Ok(new ApiResponseDto<StnkUpdateReadDto>
                {
                    Status = 200,
                    Message = "OK",
                    Data = stnk
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ErrorResponseHelper.ErrorResponse(
                        StatusCodes.Status500InternalServerError,
                        "Internal Server Error",
                        ex.Message
                    )
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertStnk([FromBody] StnkInsertReadDto stnk)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join(", ", ModelState
                    .Where(x => x.Value?.Errors?.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                    .ToList());

                return BadRequest(
                    ErrorResponseHelper.ErrorResponse(
                        StatusCodes.Status400BadRequest,
                        "Invalid input!",
                        errorMessages
                    )
                );
            }

            try
            {
                await _stnkService.InsertStnk(stnk);

                return Ok(new ApiResponseDto<object>
                {
                    Status = 200,
                    Message = "OK",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ErrorResponseHelper.ErrorResponse(
                        StatusCodes.Status500InternalServerError,
                        "Internal Server Error",
                        ex.Message
                    )
                );
            }
        }

        [HttpPut]
        public IActionResult UpdateStnk(string registrationNumber, [FromBody] StnkUpdateWriteDto stnk)
        {
            //if (!ModelState.IsValid || registrationNumber == null) return BadRequest(new ApiResponseDto<StnkUpdateReadDto>
            //{
            //    Status = StatusCodes.Status400BadRequest,
            //    Message = "Invalid input!",
            //    Data = null
            //});

            //var updateStnk = await _stnkService.UpdateStnk(registrationNumber, stnk);
            //if (!updateStnk.IsSuccess)
            //    return StatusCode(updateStnk.Status, new ApiResponseDto<StnkUpdateReadDto>
            //    {
            //        Status = updateStnk.Status,
            //        Message = updateStnk.Message,
            //        Data = null
            //    });

            //return Ok(new ApiResponseDto<StnkUpdateReadDto>
            //{
            //    Status = updateStnk.Status,
            //    Message = updateStnk.Message,
            //    Data = updateStnk.Data
            //});

            return Ok();
        }

        [HttpGet("calculate-tax")]
        public async Task<IActionResult> CalculateTax(int carType, int engineSize, decimal carPrice, string ownerName, string registrationNumber = "")
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
