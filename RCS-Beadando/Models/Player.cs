using System.ComponentModel.DataAnnotations;
using RCS_Beadando.Types;

namespace RCS_Beadando.Models
{
    // An opponent you sat with
    public class Player
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Muszáj lennie névnek")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Play style")]
        public Enums.PlayStyle PlayStyle { get; set; }

        public string? Notes { get; set; }

        // Other side of the many-to-many with Hand. EF creates a "HandPlayer" join table for us
        public List<Hand> Hands { get; set; } = new();
    }
}
