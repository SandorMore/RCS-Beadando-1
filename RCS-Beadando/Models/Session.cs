using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RCS_Beadando.Models
{
    public class Session
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Muszáj lennie dátumnak")]
        public DateOnly date { get; set; }


        public Types.Enums.Stakes stakes { get; set; }

        public int BuyInPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int BuyInNumber { get; set; }

        public int CashOut { get; set; }

        [NotMapped]
        public int Net => CashOut - (BuyInPrice * BuyInNumber);
        public List<Hand> Hands { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
