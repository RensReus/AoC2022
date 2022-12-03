using AoC2022.Days;

namespace AoC2022.DaysXX;

class Day : BaseDay
{
    static IEnumerable<String> ProcessInput(bool part2, string suffix)
    {
        var readFile = ReadFile("XX/" + suffix);
        return readFile.Select(line => line);
    }

    public override int Part1(string suffix)
    {
        var input = ProcessInput(false, suffix);

        int answer = 0;
        return answer;
    }

    public override int Part2(string suffix)
    {
        var input = ProcessInput(true, suffix);

        int answer = 0;
        return answer;
    }

    public override List<Case> Part1Cases() => new() { new("1a", 1111111111) };

    public override List<Case> Part2Cases() => new() { };
}