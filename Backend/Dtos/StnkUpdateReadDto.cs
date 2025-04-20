using System.ComponentModel.DataAnnotations;

namespace Backend.Dtos
{
    public class StnkUpdateReadDto
    {
        public required string RegistrationNumber { get; set; }
        public required string OwnerName { get; set; }
        public required string OwnerNIK { get; set; }
        public int CarType { get; set; }
        public required string CarName { get; set; }
        public int EngineSize { get; set; }
        public decimal CarPrice { get; set; }
        public decimal LastTaxPrice { get; set; }
    }
}
