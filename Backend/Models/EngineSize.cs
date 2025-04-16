using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class EngineSize
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public int size { get; set; }

        [Required]
        public int percentage { get; set; }
    }
}
