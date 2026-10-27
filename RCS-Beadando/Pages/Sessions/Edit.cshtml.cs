using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RCS_Beadando.Data;
using RCS_Beadando.Models;

namespace RCS_Beadando.Pages.Sessions
{
    public class EditModel : PageModel
    {
        private readonly PokerContext _db;

        public EditModel(PokerContext db) => _db = db;

        // [BindProperty] fills this object from the submitted form fields (Session.Date, Session.Stakes, ...)
        [BindProperty]
        public Session Session { get; set; } = new() { Date = DateOnly.FromDateTime(DateTime.Today) };

        public bool IsNew => Session.Id == 0;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return Page();

            var session = await _db.Sessions.FindAsync(id);
            if (session == null) return NotFound();

            Session = session;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // ModelState holds the result of the [Required], [Range]... checks on Session
            if (!ModelState.IsValid) return Page();

            if (IsNew)
                _db.Sessions.Add(Session);
            else
                _db.Sessions.Update(Session);

            await _db.SaveChangesAsync();
            return RedirectToPage("Details", new { id = Session.Id });
        }
    }
}
