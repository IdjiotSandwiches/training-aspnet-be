using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class StnkInsertWriteDto
    {
        [Required]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public int CarType { get; set; }

        [Required]
        public string CarName { get; set; } = string.Empty;

        [Required]
        public int EngineSize { get; set; }

        [Required]
        public decimal CarPrice { get; set; }

        [Required]
        public decimal LastTaxPrice { get; set; }

        [Required]
        public DateOnly AddedDate { get; set; }

        [Required]
        public string AddedBy { get; set; } = string.Empty;
    }
}
