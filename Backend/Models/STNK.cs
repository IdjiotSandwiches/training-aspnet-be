using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class STNK
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public required string RegistrationNumber { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public int CarType { get; set; }

        [Required]
        public required string CarName { get; set; }

        [Required]
        public int EngineSize { get; set; }

        [Required]
        public decimal CarPrice { get; set; }

        [Required]
        public decimal LastTaxPrice { get; set; }

        [Required]
        public DateOnly AddedDate { get; set; }

        [Required]
        public required string AddedBy { get; set; }

        public DateOnly ModifiedDate { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
    }
}
