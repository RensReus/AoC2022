
namespace AoC2023;

class Day09 : BaseDay
{
    [Example(expected: 114, input: "0 3 6 9 12 15\n1 3 6 10 15 21\n10 13 16 21 30 45")]
    [Puzzle(expected: 1993300041)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input).Select(x => x.Split(" ").Select(int.Parse).ToList());
        return lines.Select(CalculateNext).Sum();
    }

    private static int CalculateNext(List<int> enumerable, int arg2)
    {
        var depth = 0;
        var levels = new List<List<int>> { enumerable };
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
        levels[^1].Add(0);
        for (int i = levels.Count - 2; i >= 0; i--)
        {
            levels[i].Add(levels[i][^1] + levels[i + 1][^1]);
        }
        return levels[0][^1];
    }

    [Example(expected: 2, input: "0 3 6 9 12 15\n1 3 6 10 15 21\n10 13 16 21 30 45")]
    [Puzzle(expected: 1038)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input).Select(x => x.Split(" ").Select(int.Parse).ToList());
        return lines.Select(CalculatePrev).Sum();
    }
    private static int CalculatePrev(List<int> enumerable, int arg2)
    {
        var depth = 0;
        var levels = new List<List<int>> { enumerable };
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
        levels[^1] = levels[^1].Prepend(0).ToList();
        for (int i = levels.Count - 2; i >= 0; i--)
        {
            levels[i] = levels[i].Prepend(levels[i][0] - levels[i + 1][0]).ToList();
        }
        return levels[0][0];
    }
}