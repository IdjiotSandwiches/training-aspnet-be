using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class STNK
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public required string StnkNumber { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public int CarType { get; set; }

        [Required]
        public required string CarName { get; set; }

        [Required]
        public int EngineType { get; set; }
    }
}
