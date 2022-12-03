using AoC2022.Days;

namespace AoC2022.Days03;

class Day : BaseDay
{
    static List<string> ProcessInput(bool part2, string suffix)
        => ReadFile("03/" + suffix).ToList();

    public override int Part1(string suffix)
        => ProcessInput(false, suffix)
            .Select(line => line.Substring(0, line.Length / 2)
                .Intersect(line.Substring(line.Length / 2))
                .First())
                .Sum(x => x.PriorityValue());

    public override int Part2(string suffix)
    {
        var input = ProcessInput(true, suffix);
        var answer = 0;
        for (int i = 0; i < input.Count; i += 3)
        {
            answer += input[i].Intersect(input[i + 1]).Intersect(input[i + 2]).First().PriorityValue();
        }
        return answer;
    }

    public override List<Case> Part1Cases() => new() { new("1a", 157), new("p1", 8298) };

    public override List<Case> Part2Cases() => new() { new("1a", 70), new("p1", 2708) };
}

public static class CharExtensions
{
    public static int PriorityValue(this char letter)
        => letter switch
        {
            <= 'Z' => letter - 'A' + 27,
            _ => letter - 'a' + 1,
        };
}
