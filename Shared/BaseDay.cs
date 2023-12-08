namespace AoC.Shared;

public class BaseDay
{
    public static List<string> ReadLines(string input)
        => input.Split("\n").ToList();
}