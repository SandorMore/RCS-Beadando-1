using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RCS_Beadando.Data;
using RCS_Beadando.Models;

namespace RCS_Beadando.Pages.Players
{
    public class EditModel : PageModel
    {
        private readonly PokerContext _db;

        public EditModel(PokerContext db) => _db = db;

        [BindProperty]
        public Player Player { get; set; } = new();

        public bool IsNew => Player.Id == 0;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return Page();

            var player = await _db.Players.FindAsync(id);
            if (player == null) return NotFound();

            Player = player;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await _db.Players.AnyAsync(p => p.Name == Player.Name && p.Id != Player.Id))
                ModelState.AddModelError("Player.Name", "Már van ilyen nevű játékos");

            if (!ModelState.IsValid) return Page();

            if (IsNew)
                _db.Players.Add(Player);
            else
                _db.Players.Update(Player);

            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
