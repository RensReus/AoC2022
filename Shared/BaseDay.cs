namespace AoC.Shared;

public class BaseDay
{
    public static IList<string> ReadLines(string input)
        => input.Split("\n").ToList();
}