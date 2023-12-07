
namespace AoC2023;

class Day07 : BaseDay
{
    private const string CardOrder = "AKQJT98765432";
    private const string CardOrder2 = "AKQT98765432J";

    [Example(expected: 6440, input: "32T3K 765\nT55J5 684\nKK677 28\nKTJJT 220\nQQQJA 483")]
    [Puzzle(expected: 251545216)]
    public static int Part1(string input)
        => ReadLines(input)
            .Select(x => new Hand(x)).ToList()
            .Order()
            .Select((x, i) => (i + 1) * x.Bid).Sum();

    [Example(expected: 5905, input: "32T3K 765\nT55J5 684\nKK677 28\nKTJJT 220\nQQQJA 483")]
    [Puzzle(expected: 250384185)]
    public static int Part2(string input)
        => ReadLines(input)
            .Select(x => new Hand(x, true)).ToList()
            .Order()
            .Select((x, i) => (i + 1) * x.Bid).Sum();

    private record Hand : IComparable<Hand>
    {
        public string Cards;
        public int[] Counts;
        public int Bid;
        private readonly bool Joker;

        public Hand(string input, bool joker = false)
        {
            Joker = joker;
            Cards = input.Split()[0];
            Bid = int.Parse(input.Split()[1]);
            Counts = CalcCounts(Cards, joker);
        }

        private static int[] CalcCounts(string cards, bool joker)
        {
            var counts = new List<int>();
            var jokerCount = cards.Count(x => x == 'J');
            if (joker) cards = cards.Replace("J", "");
            if (joker && jokerCount == 5) return [5];
            while (cards.Length > 0)
            {
                var card = cards[0];
                counts.Add(cards.Count(x => x == card));
                cards = cards.Replace(card.ToString(), "");
            }
            var outputCounts = counts.OrderDescending().ToArray();
            if (joker) outputCounts[0] += jokerCount;
            return outputCounts;
        }

        public int CompareTo(Hand? that)
        {
            if (that is null) return 1;
            var betterType = that.Counts.Length - Counts.Length;
            if (betterType != 0) return betterType;
            for (int i = 0; i < Counts.Length; i++)
            {
                betterType = Counts[i] - that.Counts[i];
                if (betterType != 0) return betterType;
            }
            for (int i = 0; i < Cards.Length; i++)
            {
                var cardOrder = Joker ? CardOrder2 : CardOrder;
                var betterCard = cardOrder.IndexOf(that.Cards[i]) - cardOrder.IndexOf(Cards[i]);
                if (betterCard != 0) return betterCard;
            }
            return 0;
        }
    }
}