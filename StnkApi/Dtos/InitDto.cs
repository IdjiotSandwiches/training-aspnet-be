using StnkApi.Models;

namespace StnkApi.Dtos
{
    public class InitDto
    {
        public IEnumerable<CarType> CarType { get; set; } = [];
        public IEnumerable<EngineSize> EngineSize { get; set; } = [];
    }
}
