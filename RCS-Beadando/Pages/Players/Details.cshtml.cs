using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RCS_Beadando.Data;
using RCS_Beadando.Models;

namespace RCS_Beadando.Pages.Players
{
    public class DetailsModel : PageModel
    {
        private readonly PokerContext _db;

        public DetailsModel(PokerContext db) => _db = db;

        public Player Player { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var player = await _db.Players
                .Include(p => p.Hands)
                    .ThenInclude(h => h.Session)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (player == null) return NotFound();

            Player = player;
            return Page();
        }
    }
}
