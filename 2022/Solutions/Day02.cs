namespace AoC2022;

class Day02
{
    static IEnumerable<IEnumerable<string>> ProcessInput(string input)
        => input.Split(";").Select(line => line.Split(" "));

    [Example(expected: 15, input: "A Y;B X;C Z")]
    [Puzzle(expected: 10718)]
    public int Part1(string input)
         => ProcessInput(input).Sum(x => RoundScore1(x.ElementAt(0), x.ElementAt(1)));

    [Example(expected: 12, input: "A Y;B X;C Z")]
    [Puzzle(expected: 14652)]
    public int Part2(string input)
         => ProcessInput(input).Sum(x => RoundScore2(x.ElementAt(0), x.ElementAt(1)));

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
}