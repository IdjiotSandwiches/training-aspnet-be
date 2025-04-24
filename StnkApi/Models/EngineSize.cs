using System.ComponentModel.DataAnnotations;

namespace StnkApi.Models
{
    public class EngineSize
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Size { get; set; }

        [Required]
        public int Percentage { get; set; }
    }
}
