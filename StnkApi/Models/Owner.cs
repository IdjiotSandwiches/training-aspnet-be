using System.ComponentModel.DataAnnotations;

namespace StnkApi.Models
{
    public class Owner
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string NIK { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
