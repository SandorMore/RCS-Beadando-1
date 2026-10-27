using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RCS_Beadando.Data;
using RCS_Beadando.Models;

namespace RCS_Beadando.Pages.Sessions
{
    public class DetailsModel : PageModel
    {
        private readonly PokerContext _db;

        public DetailsModel(PokerContext db) => _db = db;

        public Session Session { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var session = await _db.Sessions
                .Include(s => s.Hands)
                    .ThenInclude(h => h.Opponents)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (session == null) return NotFound();

            Session = session;
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteHandAsync(int id, int handId)
        {
            var hand = await _db.Hands.FindAsync(handId);
            if (hand != null)
            {
                _db.Hands.Remove(hand);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage(new { id });
        }
    }
}
