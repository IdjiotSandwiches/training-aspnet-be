namespace Backend.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int Status { get; set; } = 200;

        public static Result<T> Success(int status, string message, T? data = default) => new () { IsSuccess = true, Status = status, Message = message, Data = data };
        public static Result<T> Error(int status, string message) => new () { IsSuccess = false, Status = status, Message = message };
    }
}
