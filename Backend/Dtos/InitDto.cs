using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Dtos
{
    public class InitDto
    {
        public IEnumerable<CarType> CarType { get; set; } = new List<CarType>();
        public IEnumerable<EngineSize> EngineSize { get; set; } = new List<EngineSize>();
    }
}
