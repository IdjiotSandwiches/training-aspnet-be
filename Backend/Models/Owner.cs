using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Owner
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public required string NIK { get; set; }

        [Required]
        public required string Name { get; set; }
    }
}
