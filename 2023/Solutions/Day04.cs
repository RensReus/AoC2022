
namespace AoC2023;

class Day04 : BaseDay
{

    [Example(expected: 13, input: "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\nCard 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\nCard 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\nCard 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\nCard 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11")]
    [Puzzle(expected: 32609)]
    public static int Part1(string input)
        => (int)ReadLines(input).Select(x => MatchCount(x.Split(':')[1])).Sum(overlap => overlap == 0 ? 0 : Math.Pow(2, overlap - 1));

    private static int MatchCount(string v)
    {
        var (winners, actual) = (v.Split('|')[0].Trim().Split(), v.Split('|')[1].Trim().Split());
        return winners.Where(x => x != "").Intersect(actual.Where(x => x != "")).Count();
    }

    [Example(expected: 30, input: "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\nCard 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\nCard 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\nCard 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\nCard 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11")]
    [Puzzle(expected: 14624680)]
    public static int Part2(string input)
    {
        var cards = ReadLines(input).Select(x => (MatchCount(x.Split(':')[1]), 1)).ToList();
        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = 0; j < cards[i].Item1; j++)
            {
                if (i + j + 1 >= cards.Count) continue;

                cards[i + j + 1] = (cards[i + j + 1].Item1, cards[i].Item2 + cards[i + j + 1].Item2);
            }
        }
        return cards.Sum(x => x.Item2);
    }
}