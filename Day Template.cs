namespace AoC2022;

static class Day
{
    static IList<string> ProcessInput(string input)
    {
        return input.Split(";").ToList();
    }

    [Example(expected: 1111111, input: "AAAAA")]
    // [Puzzle(expected: 222222)]
    public static int Part1(string input)
    {
        var processedInput = ProcessInput(input);
        return 1111111;
    }

    [Example(expected: 1111111, input: "AAAAA")]
    // [Puzzle(expected: 222222)]
    public static int Part2(string input)
    {
        var processedInput = ProcessInput(input);
        return 1111111;
    }
}