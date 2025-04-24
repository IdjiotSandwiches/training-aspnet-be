namespace StnkApi.Dtos
{
    public class ApiResponseDto_DEPRECATE<T>
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }

    public class ErrorResponseDto
    {
        public string Code { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
    }
}
