using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCS_Beadando.Models
{
    public class Session
    {
        public int Id { get; set; }

        // [DataType(Date)] makes the form render <input type="date"> (a date picker)
        [Required(ErrorMessage = "Muszáj lennie dátumnak")]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }
        public Types.Enums.Stakes Stakes { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Buy-in")]
        public int BuyInPrice { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Number of buy-ins")]
        public int BuyInNumber { get; set; } = 1;

        [StringLength(100)]
        public string? Location { get; set; }

        // 0 allowed: busting out is a valid cash-out
        [Range(0, int.MaxValue)]
        [Display(Name = "Cash out")]
        public int CashOut { get; set; }

        [NotMapped]
        public int Net => CashOut - (BuyInPrice * BuyInNumber);

        public List<Hand> Hands { get; set; } = new();

        public string? Notes { get; set; } = string.Empty;
    }
}
