using System.ComponentModel.DataAnnotations;

namespace StnkApi.Models
{
    public class CarType
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Percentage { get; set; }
    }
}
