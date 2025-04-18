using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class SequenceType
    {
        [Key]
        public int Id { get; }
        public string Pattern { get; } = string.Empty;

        public SequenceType(int id, string pattern)
        {
            Id = id;
            Pattern = pattern;
        }
    }
}
