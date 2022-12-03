using AoC2022.Days;

namespace AoC2022.Days01;

class Day : BaseDay
{
    static IEnumerable<int> ProcessInput(string filename)
        => ReadFile("01/" + filename, "\r\n\r\n").Select(x => x.Split("\r\n").Sum(y => Int32.Parse(y)));

    public override int Part1(string filename)
        => ProcessInput(filename).Max();

    public override int Part2(string filename)
        => ProcessInput(filename).OrderDescending().Take(3).Sum();

    public override List<Case> Part1Cases() => new() { new("1a", 24000), new("p1", 74198) };

    public override List<Case> Part2Cases() => new() { new("1a", 45000), new("p1", 209914) };
}