namespace AoC2023;

class Day02 : BaseDay
{
    static IList<BallGame> ProcessInput(string input)
    {
        return ReadLines(input).Select(line => new BallGame(line)).ToList();
    }

    [Example(expected: 8, input: "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green\r\nGame 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue\r\nGame 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red\r\nGame 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red\r\nGame 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green")]
    [Puzzle(expected: 2512)]
    public static int Part1(string input)
    {
        var processedInput = ProcessInput(input);
        var (maxRed, maxGreen, maxBlue) = (12, 13, 14);
        return processedInput.Where(x => x.Red <= maxRed && x.Blue <= maxBlue && x.Green <= maxGreen).Sum(x => x.Id);
    }

    [Example(expected: 2286, input: "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green\r\nGame 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue\r\nGame 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red\r\nGame 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red\r\nGame 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green")]
    [Puzzle(expected: 67335)]
    public static int Part2(string input)
    {
        var processedInput = ProcessInput(input);
        return processedInput.Sum(x => x.Blue * x.Green * x.Red);
    }
}

public record BallGame
{
    public int Red = 0;
    public int Blue = 0;
    public int Green = 0;
    public int Id;

    public BallGame(string game)
    {
        Id = int.Parse(game[5..game.IndexOf(':')]);
        FindMax(game[(game.IndexOf(':') + 1)..]);
    }

    private void FindMax(string game)
    {
        var rounds = game.Split(';');
        foreach (var round in rounds)
        {
            var colors = round.Split(", ");
            foreach (var color in colors)
            {
                var (value, col) = (color.Trim().Split(" ")[0], color.Trim().Split(" ")[1]);
                switch (col)
                {
                    case "red": Red = int.Max(Red, int.Parse(value)); break;
                    case "blue": Blue = int.Max(Blue, int.Parse(value)); break;
                    case "green": Green = int.Max(Green, int.Parse(value)); break;
                }
            }
        }
    }
}