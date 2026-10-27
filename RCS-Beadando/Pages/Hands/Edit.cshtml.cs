using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RCS_Beadando.Data;
using RCS_Beadando.Models;
using RCS_Beadando.Types;

namespace RCS_Beadando.Pages.Hands
{
    public class EditModel : PageModel
    {
        private readonly PokerContext _db;

        public EditModel(PokerContext db) => _db = db;

        [BindProperty]
        public Hand Hand { get; set; } = new();

        // The <select multiple> posts back the chosen player ids
        [BindProperty]
        public List<int> SelectedPlayerIds { get; set; } = new();

        // Options for the opponents dropdown (not bound, rebuilt on every request)
        public List<SelectListItem> PlayerOptions { get; set; } = new();

        public bool IsNew => Hand.Id == 0;

        // New hand:  /Hands/Edit?sessionId=3     Edit hand: /Hands/Edit/7
        public async Task<IActionResult> OnGetAsync(int? id, int? sessionId)
        {
            if (id != null)
            {
                var hand = await _db.Hands.Include(h => h.Opponents).FirstOrDefaultAsync(h => h.Id == id);
                if (hand == null) return NotFound();
                Hand = hand;
                SelectedPlayerIds = hand.Opponents.Select(p => p.Id).ToList();
            }
            else
            {
                if (sessionId == null || !await _db.Sessions.AnyAsync(s => s.Id == sessionId)) return NotFound();
                Hand.SessionId = sessionId.Value;
            }

            await LoadPlayerOptionsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateCards();
            if (!ModelState.IsValid)
            {
                await LoadPlayerOptionsAsync();
                return Page();
            }

            // Store cards in a consistent format, e.g. "asKD" -> "AsKd"
            Hand.HoleCards = Card.Join(Hand.HoleCardList);
            Hand.Board = string.IsNullOrEmpty(Hand.Board) ? null : Card.Join(Hand.BoardList);

            var opponents = await _db.Players.Where(p => SelectedPlayerIds.Contains(p.Id)).ToListAsync();

            if (IsNew)
            {
                Hand.Opponents = opponents;
                _db.Hands.Add(Hand);
            }
            else
            {
                // For many-to-many we load the tracked entity with its current opponents, then replace them;
                // EF works out which join-table rows to insert/delete
                var existing = await _db.Hands.Include(h => h.Opponents).FirstOrDefaultAsync(h => h.Id == Hand.Id);
                if (existing == null) return NotFound();

                _db.Entry(existing).CurrentValues.SetValues(Hand); // copies the simple columns
                existing.Opponents.Clear();
                existing.Opponents.AddRange(opponents);
            }

            await _db.SaveChangesAsync();
            return RedirectToPage("/Sessions/Details", new { id = Hand.SessionId });
        }

        // Checks the attributes on Hand can't do: the same card can't appear twice
        private void ValidateCards()
        {
            if (!ModelState.IsValid) return; // format is already wrong, the regex error is enough

            var all = Hand.HoleCardList.Concat(Hand.BoardList).ToList();
            if (all.Count != all.Distinct().Count())
                ModelState.AddModelError(string.Empty, "Ugyanaz a lap kétszer szerepel");
        }

        private async Task LoadPlayerOptionsAsync()
        {
            var players = await _db.Players.OrderBy(p => p.Name).ToListAsync();
            PlayerOptions = players
                .Select(p => new SelectListItem(p.Name, p.Id.ToString(), SelectedPlayerIds.Contains(p.Id)))
                .ToList();
        }
    }
}
