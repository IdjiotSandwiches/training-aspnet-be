using System.ComponentModel.DataAnnotations;

namespace StnkApi.Models
{
    public class Sequence
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        public long CurrentSequence { get; set; }
    }
}
