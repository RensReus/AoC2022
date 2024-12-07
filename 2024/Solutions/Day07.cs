

namespace AoC2024;

class Day07 : BaseDay
{
    [Example(expected: 3749, input: "190: 10 19\n3267: 81 40 27\n83: 17 5\n156: 15 6\n7290: 6 8 6 15\n161011: 16 10 13\n192: 17 8 14\n21037: 9 7 18 13\n292: 11 6 16 20")]
    [Puzzle(expected: 28730327770375)]
    public static long Part1(string input)
        => ReadLines(input).Select(ParseLine).Where(x => IsValid(x)).Sum(x => x.Item1);

    private static (long, List<long>) ParseLine(string arg1)
    {
        var split = arg1.Split(": ");
        return (long.Parse(split[0]), split[1].Split(" ").Select(long.Parse).ToList());
    }

    private static bool IsValid((long, List<long>) tuple, bool part2 = false)
        => GetPossibleValues(tuple.Item2, part2, tuple.Item1).Contains(tuple.Item1);

    private static List<long> GetPossibleValues(List<long> item2, bool part2, long target)
    {
        if (item2.Count == 1)
        {
            return [item2[0]];
        }

        var last = item2[^1];
        var rest = GetPossibleValues(item2[..^1], part2, target);

        var result = new List<long>();

        foreach (var num in rest)
        {
            result.Add(num + last);
            result.Add(num * last);
            if (part2)
            {
                result.Add(long.Parse(num.ToString() + last.ToString()));
            }
        }
        return result.Where(x => x <= target).ToList();
    }

    [Example(expected: 11387, input: "190: 10 19\n3267: 81 40 27\n83: 17 5\n156: 15 6\n7290: 6 8 6 15\n161011: 16 10 13\n192: 17 8 14\n21037: 9 7 18 13\n292: 11 6 16 20")]
    [Puzzle(expected: 424977609625985)]
    public static long Part2(string input)
        => ReadLines(input).Select(ParseLine).Where(x => IsValid(x, part2: true)).Sum(x => x.Item1);
}