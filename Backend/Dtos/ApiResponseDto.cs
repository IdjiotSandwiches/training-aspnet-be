using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class ApiResponseDto<T>
    {
        [Required]
        public int Status { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public T? Data { get; set; }
    }
}
