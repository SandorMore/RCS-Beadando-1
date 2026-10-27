using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RCS_Beadando.Data;
using RCS_Beadando.Types;

namespace RCS_Beadando.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PokerContext _db;

        public IndexModel(PokerContext db) => _db = db;

        // A "record" is a compact class for holding data: one row of a stats table
        public record PositionStat(Enums.Position Position, int Hands, int TotalBet, double AvgBet, int Net, double WinRate);
        public record StakesStat(Enums.Stakes Stakes, int Sessions, int Invested, int CashOut, int Net);
        public record OpponentStat(int Id, string Name, int Hands, int Net);

        public int TotalSessions { get; set; }
        public int TotalHands { get; set; }
        public int TotalInvested { get; set; }
        public int TotalNet { get; set; }
        public int WinningSessions { get; set; }

        public List<PositionStat> ByPosition { get; set; } = new();
        public List<StakesStat> ByStakes { get; set; } = new();
        public List<OpponentStat> ByOpponent { get; set; } = new();

        public async Task OnGetAsync()
        {
            // AsNoTracking: read-only query, EF doesn't need to watch these objects for changes (faster)
            var sessions = await _db.Sessions.AsNoTracking().ToListAsync();
            var hands = await _db.Hands.AsNoTracking().Include(h => h.Opponents).ToListAsync();

            TotalSessions = sessions.Count;
            TotalHands = hands.Count;
            TotalInvested = sessions.Sum(s => s.BuyInPrice * s.BuyInNumber);
            TotalNet = sessions.Sum(s => s.Net);
            WinningSessions = sessions.Count(s => s.Net > 0);

            ByPosition = hands
                .GroupBy(h => h.Position)
                .OrderBy(g => g.Key)
                .Select(g => new PositionStat(
                    g.Key,
                    g.Count(),
                    g.Sum(h => h.AmountBet),
                    g.Average(h => h.AmountBet),
                    g.Sum(h => h.Result),
                    100.0 * g.Count(h => h.Result > 0) / g.Count()))
                .ToList();

            ByStakes = sessions
                .GroupBy(s => s.Stakes)
                .OrderBy(g => g.Key)
                .Select(g => new StakesStat(
                    g.Key,
                    g.Count(),
                    g.Sum(s => s.BuyInPrice * s.BuyInNumber),
                    g.Sum(s => s.CashOut),
                    g.Sum(s => s.Net)))
                .ToList();

            // SelectMany "flattens": every (hand, opponent) pair becomes one item, then we group by opponent
            ByOpponent = hands
                .SelectMany(h => h.Opponents, (h, p) => new { h.Result, p.Id, p.Name })
                .GroupBy(x => new { x.Id, x.Name })
                .Select(g => new OpponentStat(g.Key.Id, g.Key.Name, g.Count(), g.Sum(x => x.Result)))
                .OrderByDescending(o => o.Hands)
                .ToList();
        }
    }
}
