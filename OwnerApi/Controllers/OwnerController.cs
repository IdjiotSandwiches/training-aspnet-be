using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OwnerApi.Dtos;
using OwnerApi.Services.Interfaces;
using SharedLibrary.Dtos;
using SharedLibrary.Helpers;

namespace OwnerApi.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerController(IOwnerService ownerService) : ControllerBase
    {
        private readonly IOwnerService _ownerService = ownerService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOwner(int id)
        {
            try
            {
                var owner = await _ownerService.GetOwner(id);
                return Ok(new ApiResponseDto<OwnerReadDto>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "OK",
                    Data = owner
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

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetOwnerId(string name)
        {
            try
            {
                var owner = await _ownerService.GetOwnerId(name);
                return Ok(new ApiResponseDto<int>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "OK",
                    Data = owner
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
        public async Task<IActionResult> InsertOwner([FromBody] OwnerWriteDto owner)
        {
            try
            {
                var ownerId = await _ownerService.InsertOwner(owner);
                return Ok(new ApiResponseDto<int>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "OK",
                    Data = ownerId
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ErrorResponseHelper.ErrorResponse(
                        StatusCodes.Status400BadRequest,
                        "Bad Request",
                        ex.Message
                    )
                );
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
    }
}
