namespace AoC2022;

class Day01
{
    static IEnumerable<int> ProcessInput(string input)
        => input.Split(";;").Select(x => x.Split(";").Sum(y => Int32.Parse(y)));

    [Example(expected: 24000, input: "1000;2000;3000;;4000;;5000;6000;;7000;8000;9000;;10000")]
    [Puzzle(expected: 74198)]
    public int Part1(string input)
        => ProcessInput(input).Max();

    [Example(expected: 45000, input: "1000;2000;3000;;4000;;5000;6000;;7000;8000;9000;;10000")]
    [Puzzle(expected: 209914)]
    public int Part2(string filename)
        => ProcessInput(filename).OrderDescending().Take(3).Sum();
}