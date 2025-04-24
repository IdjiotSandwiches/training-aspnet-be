using System.ComponentModel.DataAnnotations;

namespace OwnerApi.Models
{
    public class Owner
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Nik { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
