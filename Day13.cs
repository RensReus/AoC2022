namespace AoC2022;

class Day13
{
    static IList<string> ProcessInput(string input)
    {
        return input.Split(";").ToList();
    }

    [Example(expected: 1111111, input: "AAAAA")]
    [Puzzle(expected: 222222)]
    public int Part1(string input)
    {
        var processedInput = ProcessInput(input);
        return 0;
    }

    [Example(expected: 1111111, input: "AAAAA")]
    [Puzzle(expected: 222222)]
    public int Part2(string input)
    {
        var processedInput = ProcessInput(input);
        return 0;
    }
}