using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class StnkUpdateWriteDto
    {
        [Required]
        public int CarType { get; set; }

        [Required]
        public required string CarName { get; set; }

        [Required]
        public int EngineSize { get; set; }

        [Required]
        public decimal CarPrice { get; set; }
    }
}
