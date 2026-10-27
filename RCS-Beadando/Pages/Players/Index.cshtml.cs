using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RCS_Beadando.Data;
using RCS_Beadando.Models;

namespace RCS_Beadando.Pages.Players
{
    public class IndexModel : PageModel
    {
        private readonly PokerContext _db;

        public IndexModel(PokerContext db) => _db = db;

        public List<Player> Players { get; set; } = new();

        // [BindProperty(SupportsGet = true)] reads ?search=... from the URL on a GET request
        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public async Task OnGetAsync()
        {
            var query = _db.Players.Include(p => p.Hands).AsQueryable();
            if (!string.IsNullOrWhiteSpace(Search))
                query = query.Where(p => p.Name.Contains(Search));

            Players = await query.OrderBy(p => p.Name).ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var player = await _db.Players.FindAsync(id);
            if (player != null)
            {
                // Only the join-table rows are removed; the hands themselves stay
                _db.Players.Remove(player);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
