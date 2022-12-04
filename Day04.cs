using System.Text.RegularExpressions;
using AoC2022.Days;

namespace AoC2022.Days04;

class Day : BaseDay
{
    static IList<ElfPair> ProcessInput(string filename)
        => ReadFile("04/" + filename).Select(x => new ElfPair(x)).ToList();

    public override int Part1(string filename)
        => ProcessInput(filename).Select(x => x.HasFullOverlap()).Sum();

    public override int Part2(string filename)
        => ProcessInput(filename).Select(x => x.HasAnyOverlap()).Sum();

    public override List<Case> Part1Cases() => new() { new("1a", 2), new("p1", 471) };

    public override List<Case> Part2Cases() => new() { new("1a", 4), new("p1", 888) };
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