using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Dtos
{
    public class InitDto
    {
        [Required]
        public required List<CarType> CarType { get; set; }

        [Required]
        public required List<EngineSize> EngineSize { get; set; }
    }
}
