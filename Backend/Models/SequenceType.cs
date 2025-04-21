using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class SequenceType
    {
        [Key]
        public int Id { get; set; }
        public string Pattern { get; set; } = string.Empty;
    }
}
