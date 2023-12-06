
namespace AoC2023;

partial class Day06 : BaseDay
{
    [Example(expected: 288, input: "Time:      7  15   30\nDistance:  9  40  200")]
    [Puzzle(expected: 781200)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);
        var times = NumberRegex().Matches(lines[0]).Select(x => int.Parse(x.Value)).ToList();
        var dists = NumberRegex().Matches(lines[1]).Select(x => int.Parse(x.Value)).ToList();
        var answer = 1;
        for (int i = 0; i < times.Count; i++)
        {
            answer *= WaysToWin(times[i], dists[i]);
        }
        return answer;
    }

    private static int WaysToWin(long time, long dist)
    {
        var ways = 0;
        for (long i = 1; i < time; i++)
        {
            if ((time - i) * i > dist) ways++;
        }
        return ways;
    }

    [Example(expected: 71503, input: "Time:      7  15   30\nDistance:  9  40  200")]
    [Puzzle(expected: 49240091)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input);
        var times = long.Parse(lines[0].Replace(" ", "").Substring(lines[0].IndexOf(':') + 1));
        var dists = long.Parse(lines[1].Replace(" ", "").Substring(lines[1].IndexOf(':') + 1));
        return WaysToWin(times, dists);
    }

    [GeneratedRegex(@"(\d+)")]
    private static partial Regex NumberRegex();
}