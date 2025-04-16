using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class AllStnkDto
    {
        [Required]
        public required string StnkNumber { get; set; }

        [Required]
        public required string CarName { get; set; }

        [Required]
        public decimal LastTaxPrice { get; set; }
    }
}
