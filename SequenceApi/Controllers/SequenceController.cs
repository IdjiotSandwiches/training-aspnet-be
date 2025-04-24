using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Enums;
using SharedLibrary.Dtos;
using SharedLibrary.Helpers;
using SequenceApi.Services.Interfaces;

namespace SequenceApi.Controllers
{
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    public class SequenceController(ISequenceService sequenceService) : Controller
    {
        private readonly ISequenceService _sequenceService = sequenceService;

        [HttpGet]
        public async Task<IActionResult> GetSequence(SequenceTypeEnum type)
        {
            try
            {
                var sequence = await _sequenceService.GenerateSequence(type);

                return Ok(new ApiResponseDto<string>
                {
                    Status = StatusCodes.Status200OK,
                    Message = "OK",
                    Data = sequence
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
    }
}
