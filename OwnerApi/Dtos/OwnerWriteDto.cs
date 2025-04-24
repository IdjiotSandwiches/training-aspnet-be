using System.ComponentModel.DataAnnotations;

namespace OwnerApi.Dtos
{
    public class OwnerWriteDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
