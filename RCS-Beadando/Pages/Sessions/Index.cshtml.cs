using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RCS_Beadando.Data;
using RCS_Beadando.Models;

namespace RCS_Beadando.Pages.Sessions
{
    public class IndexModel : PageModel
    {
        private readonly PokerContext _db;

        public IndexModel(PokerContext db) => _db = db;

        public List<Session> Sessions { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Include(...) also loads the related hands in the same query (otherwise Hands would be empty)
            Sessions = await _db.Sessions
                .Include(s => s.Hands)
                .OrderByDescending(s => s.Date)
                .ToListAsync();
        }

        // Called by a form with asp-page-handler="Delete" -> method name OnPost + Delete
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var session = await _db.Sessions.FindAsync(id);
            if (session != null)
            {
                _db.Sessions.Remove(session);
                await _db.SaveChangesAsync();
            }
            // Post/Redirect/Get: redirect so refreshing the page doesn't re-submit the delete
            return RedirectToPage();
        }
    }
}
