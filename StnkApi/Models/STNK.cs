using System.ComponentModel.DataAnnotations;

namespace StnkApi.Models
{
    public class Stnk
    {
        [Key]
        [Required]
        public int Id { get; set; }

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

        public DateOnly ModifiedDate { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
    }
}
