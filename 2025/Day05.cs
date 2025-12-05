namespace AoC2025;

class Day05 : BaseDay
{
    [Example(expected: 3, input: "3-5\n10-14\n16-20\n12-18\n\n1\n5\n8\n11\n17\n32")]
    [Puzzle(expected: 513)]
    public static int Part1(string input)
    {
        var lines = ReadLinesDouble(input);
        var ranges = lines[0].Select(ParseRange).ToList();
        return lines[1].Select(long.Parse).Count(num => ranges.Any(r => num >= r.Item1 && num <= r.Item2));
    }

    private static (long, long) ParseRange(string line)
    {
        var parts = line.Split('-').Select(long.Parse).ToArray();
        return (parts[0], parts[1]);
    }

    [Example(expected: 14, input: "3-5\n10-14\n16-20\n12-18\n\n1\n5\n8\n11\n17\n32")]
    [Puzzle(expected: 339668510830757)]
    public static long Part2(string input)
    {
        var ranges = ReadLinesDouble(input)[0].Select(ParseRange).ToList();
        var nonOverlapping = new List<(long, long)> { ranges[0] };
        foreach (var newRange in ranges.Skip(1))
        {
            var newNonOverlapping = new List<(long, long)>();
            var toEval = newRange;
            foreach (var uniqueRange in nonOverlapping)
            {
                if (toEval.Item2 < uniqueRange.Item1 || toEval.Item1 > uniqueRange.Item2)
                {
                    newNonOverlapping.Add(uniqueRange);
                    continue;
                }
                toEval = (Math.Min(toEval.Item1, uniqueRange.Item1), Math.Max(toEval.Item2, uniqueRange.Item2));
            }
            newNonOverlapping.Add(toEval);
            nonOverlapping = newNonOverlapping;
        }
        return nonOverlapping.Sum(r => r.Item2 - r.Item1 + 1);
    }
}