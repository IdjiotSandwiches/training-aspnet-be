using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Dtos
{
    public class InitDto
    {
        public required List<CarType> CarType { get; set; }
        public required List<EngineSize> EngineSize { get; set; }
    }
}
