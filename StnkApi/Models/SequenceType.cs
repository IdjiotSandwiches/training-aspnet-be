using System.ComponentModel.DataAnnotations;

namespace StnkApi.Models
{
    public class SequenceType
    {
        [Key]
        public int Id { get; set; }
        public string Pattern { get; set; } = string.Empty;
    }
}
