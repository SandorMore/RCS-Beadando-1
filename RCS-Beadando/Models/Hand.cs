using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RCS_Beadando.Types;

namespace RCS_Beadando.Models
{
    public class Hand
    {
        public int Id { get; set; }

        // Foreign key: EF sees "SessionId" next to a "Session" navigation and links them
        public int SessionId { get; set; }
        public Session? Session { get; set; }

        // Stored as text like "AsKd" (see Card.Join / Card.ParseMany)
        [Required(ErrorMessage = "Add meg a lapjaidat")]
        [RegularExpression($"^({Card.Pattern}){{2}}$", ErrorMessage = "Két lap kell, pl. AsKd")]
        [Display(Name = "Hole cards")]
        public string HoleCards { get; set; } = string.Empty;

        // Flop/turn/river: 0, 3, 4 or 5 cards
        [RegularExpression($"^(({Card.Pattern}){{3,5}})?$", ErrorMessage = "3-5 lap, pl. Qh7c2d")]
        public string? Board { get; set; }

        public Enums.Position Position { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Amount bet")]
        public int AmountBet { get; set; }

        // Net chips won (+) or lost (-) on this hand
        public int Result { get; set; }

        [Display(Name = "Went to showdown")]
        public bool WentToShowdown { get; set; }

        public string? Notes { get; set; }

        public List<Player> Opponents { get; set; } = new();

        // [NotMapped] = computed in C#, no database column
        [NotMapped]
        public List<Card> HoleCardList => Card.ParseMany(HoleCards);

        [NotMapped]
        public List<Card> BoardList => Card.ParseMany(Board ?? string.Empty);
    }
}
