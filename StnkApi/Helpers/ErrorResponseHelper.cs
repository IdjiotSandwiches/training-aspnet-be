using StnkApi.Dtos;
using StnkApi.Enums;

namespace StnkApi.Helpers
{
    public class ErrorResponseHelper
    {
        public static string GetErrorCode(int statusCode)
        {
            return statusCode switch
            {
                400 => ErrorCode.BadRequest.ToString(),
                401 => ErrorCode.Unauthorized.ToString(),
                403 => ErrorCode.Forbidden.ToString(),
                404 => ErrorCode.NotFound.ToString(),
                500 => ErrorCode.ServerError.ToString(),
                _ => "UnknownError"
            };
        }

        public static ApiResponseDto_DEPRECATE<ErrorResponseDto> ErrorResponse(int code, string message, string detail = "")
        {
            return new ApiResponseDto_DEPRECATE<ErrorResponseDto>
            {
                Status = code,
                Message = message,
                Data = new ErrorResponseDto
                {
                    Code = GetErrorCode(code),
                    Detail = detail
                }
            };
        }
    }
}
