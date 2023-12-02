using System.Text.RegularExpressions;

namespace AoC2022;

class Day04 : BaseDay
{
    static IList<ElfPair> ProcessInput(string input)
        => input.Split("\n").Select(x => new ElfPair(x)).ToList();

    [Example(expected: 2, input: "2-4,6-8\n2-3,4-5\n5-7,7-9\n2-8,3-7\n6-6,4-6\n2-6,4-8")]
    [Puzzle(expected: 471)]
    public int Part1(string input)
        => ProcessInput(input).Select(x => x.HasFullOverlap()).Sum();

    [Example(expected: 4, input: "2-4,6-8\n2-3,4-5\n5-7,7-9\n2-8,3-7\n6-6,4-6\n2-6,4-8")]
    [Puzzle(expected: 888)]
    public int Part2(string input)
        => ProcessInput(input).Select(x => x.HasAnyOverlap()).Sum();
}

internal class ElfPair
{
    public int Low1;
    public int Low2;
    public int High1;
    public int High2;

    public ElfPair(string line)
    {
        var groups = Regex.Match(line, @"(\d+)-(\d+),(\d+)-(\d+)").Groups;
        Low1 = Int32.Parse(groups[1].Value);
        High1 = Int32.Parse(groups[2].Value);
        Low2 = Int32.Parse(groups[3].Value);
        High2 = Int32.Parse(groups[4].Value);
    }

    internal int HasFullOverlap()
        => (Low1 <= Low2 && High1 >= High2) || (Low2 <= Low1 && High2 >= High1) ? 1 : 0;

    internal int HasAnyOverlap()
        => (Low1 >= Low2 && Low1 <= High2) || (High1 >= Low2 && High1 <= High2) ||
            (Low2 >= Low1 && Low2 <= High1) || (High2 >= Low1 && High2 <= High1) ? 1 : 0;
}