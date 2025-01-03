

namespace AoC2024;

class Day07 : BaseDay
{
    [Example(expected: 3749, input: "190: 10 19\n3267: 81 40 27\n83: 17 5\n156: 15 6\n7290: 6 8 6 15\n161011: 16 10 13\n192: 17 8 14\n21037: 9 7 18 13\n292: 11 6 16 20")]
    [Puzzle(expected: 28730327770375)]
    public static long Part1(string input)
        => ReadLines(input).Select(ParseLine).Where(x => IsValid(x.Item1, 0, x.Item2)).Sum(x => x.Item1);

    private static (long, List<long>) ParseLine(string arg1)
    {
        var split = arg1.Split(": ");
        return (long.Parse(split[0]), split[1].Split(" ").Select(long.Parse).ToList());
    }

    private static bool IsValid(long target, long initialValue, List<long> values)
    {
        if (values.Count == 0) return target == initialValue;
        var add = initialValue + values[0];
        var mult = initialValue * values[0];
        return IsValid(target, add, values[1..]) || IsValid(target, mult, values[1..]);
    }

    [Example(expected: 11387, input: "190: 10 19\n3267: 81 40 27\n83: 17 5\n156: 15 6\n7290: 6 8 6 15\n161011: 16 10 13\n192: 17 8 14\n21037: 9 7 18 13\n292: 11 6 16 20")]
    [Puzzle(expected: 424977609625985)]
    public static long Part2(string input)
        => ReadLines(input).Select(ParseLine).Where(x => IsValid2(x.Item1, 0, x.Item2)).Sum(x => x.Item1);

    private static bool IsValid2(long target, long initialValue, List<long> values)
    {
        if (values.Count == 0) return target == initialValue;
        var add = initialValue + values[0];
        var mult = initialValue * values[0];
        var concat = long.Parse(initialValue.ToString() + values[0].ToString());
        return IsValid2(target, add, values[1..]) || IsValid2(target, mult, values[1..]) || IsValid2(target, concat, values[1..]);
    }
}