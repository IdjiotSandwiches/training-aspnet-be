using System.ComponentModel.DataAnnotations;

namespace StnkApi.Dtos
{
    public class OwnerWriteDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
