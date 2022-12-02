using AoC2022.Days;

namespace AoC2022.Days02;

class Day : BaseDay
{
    static IEnumerable<IEnumerable<string>> ProcessInput(bool part2, string suffix = "")
    {
        var readFile = ReadFile("02/" + suffix);
        return readFile.Select(line => line.Split(" "));
    }

    public override int Part1(string suffix)
         => ProcessInput(false, suffix).Sum(x => RoundScore1(x.ElementAt(0), x.ElementAt(1)));

    public override int Part2(string suffix)
         => ProcessInput(false, suffix).Sum(x => RoundScore2(x.ElementAt(0), x.ElementAt(1)));


    private int RoundScore1(string opponent, string you)
    => YourChoiceScore(you)
        +
        (opponent, you) switch
        {
            ("A", "X") or ("B", "Y") or ("C", "Z") => 3,
            ("A", "Y") or ("B", "Z") or ("C", "X") => 6,
            _ => 0
        };

    private int RoundScore2(string opponent, string result)
    => result switch
    {
        "X" => 0,
        "Y" => 3,
        "Z" => 6,
        _ => throw new ArgumentOutOfRangeException()
    }
        +
        (opponent, result) switch
        {
            ("A", "Y") or ("B", "X") or ("C", "Z") => YourChoiceScore("X"),
            ("A", "Z") or ("B", "Y") or ("C", "X") => YourChoiceScore("Y"),
            _ => YourChoiceScore("Z")
        };

    private int YourChoiceScore(string you)
        => you switch
        {
            "X" => 1,
            "Y" => 2,
            "Z" => 3,
            _ => throw new ArgumentOutOfRangeException()
        };

    public override List<Case> Part1Cases() => new() { new("1a", 15), new("p1", 10718) };

    public override List<Case> Part2Cases() => new() { new("1a", 12), new("p1", 14652) };
}