namespace AoC2022;

class Day01 : BaseDay
{
    static IEnumerable<int> ProcessInput(string input)
        => input.Split("\n\n").Select(x => x.Split("\n")).Select(y => y.Sum(y => int.Parse(y)));

    [Example(expected: 24000, input: "1000\n2000\n3000\n\n4000\n\n5000\n6000\n\n7000\n8000\n9000\n\n10000")]
    [Puzzle(expected: 74198)]
    public int Part1(string input)
        => ProcessInput(input).Max();

    [Example(expected: 45000, input: "1000\n2000\n3000\n\n4000\n\n5000\n6000\n\n7000\n8000\n9000\n\n10000")]
    [Puzzle(expected: 209914)]
    public int Part2(string input)
        => ProcessInput(input).OrderDescending().Take(3).Sum();
}