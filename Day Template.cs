using AoC2022.Days;

namespace AoC2022.DaysXX;

class Day : BaseDay
{
    static IList<String> ProcessInput(string filename)
        => ReadFileLines("XX/" + filename);

    public override int Part1(string filename)
    {
        var input = ProcessInput(filename);

        int answer = 0;
        return answer;
    }

    public override int Part2(string filename)
    {
        var input = ProcessInput(filename);

        int answer = 0;
        return answer;
    }

    public override List<Case> Part1Cases() => new() { new("1a", 1111111111) };

    public override List<Case> Part2Cases() => new() { };
}