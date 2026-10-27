namespace RCS_Beadando.Types
{
    public enum Suit { Spades, Clubs, Diamonds, Hearts }

    public enum Rank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

    public readonly record struct Card(Rank Rank, Suit Suit)
    {
        private const string Ranks = "23456789TJQKA";
        private const string Suites = "scdh";

        // Regex for one card, e.g. "As", "td", "9H" (used by [RegularExpression] on Hand)
        public const string Pattern = "[2-9TJQKAtjqka][scdhSCDH]";

        public bool IsRed => Suit is Suit.Hearts or Suit.Diamonds;
        public string Symbol => $"{Ranks[(int)Rank - 2]}{"♠♣♦♥"[(int)Suit]}";

        public override string ToString() => $"{Ranks[(int)Rank - 2]}{Suites[(int)Suit]}";
        public static Card Parse(string s)
        {
            return new((Rank)(Ranks.IndexOf(char.ToUpper(s[0])) + 2), (Suit)Suites.IndexOf(char.ToLower(s[1])));
        }

        public static string Join(List<Card> cards)
        {
            return string.Concat(cards.Select(c => c.ToString()));
        }
        public static List<Card> ParseMany(string s) =>
            Enumerable.Range(0, s.Length / 2).Select(i => Parse(s.Substring(i * 2, 2))).ToList();
    }
}
