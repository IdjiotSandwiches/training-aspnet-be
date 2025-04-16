using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class CarType
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public int Percentage { get; set; }
    }
}
