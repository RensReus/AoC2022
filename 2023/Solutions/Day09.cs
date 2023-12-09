namespace AoC2023;

class Day09 : BaseDay
{
    [Example(expected: 114, input: "0 3 6 9 12 15\n1 3 6 10 15 21\n10 13 16 21 30 45")]
    [Puzzle(expected: 1993300041)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input).Select(x => x.Split(" ").Select(int.Parse));
        return lines.Select(CalculateNext).Sum();
    }

    private static int CalculateNext(IEnumerable<int> numbers)
    {
        var levels = new List<List<int>> { numbers.ToList() };
        var depth = 0;
        while (true)
        {
            var prev = levels[depth];
            var nextLevel = new List<int>();
            for (int i = 0; i < prev.Count - 1; i++)
            {
                nextLevel.Add(prev[i + 1] - prev[i]);
            }
            depth += 1;
            levels.Add(nextLevel);
            if (nextLevel.All(x => x == 0)) break;
        }
        return levels.Sum(x => x[^1]);
    }

    [Example(expected: 2, input: "0 3 6 9 12 15\n1 3 6 10 15 21\n10 13 16 21 30 45")]
    [Puzzle(expected: 1038)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input).Select(x => x.Split(" ").Select(int.Parse).Reverse());
        return lines.Select(CalculateNext).Sum();
    }
}