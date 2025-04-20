using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class AllStnkDto
    {
        public required string RegistrationNumber { get; set; }
        public required string CarName { get; set; }
        public decimal LastTaxPrice { get; set; }
    }
}
