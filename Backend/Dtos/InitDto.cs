using Backend.Models;

namespace Backend.Dtos
{
    public class InitDto
    {
        public IEnumerable<CarType> CarType { get; set; } = [];
        public IEnumerable<EngineSize> EngineSize { get; set; } = [];
    }
}
