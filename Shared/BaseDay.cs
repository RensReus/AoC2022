namespace AoC.Shared;

public class BaseDay
{
    public static List<string> ReadLines(string input)
        => input.Split("\n").ToList();

    public static List<List<string>> ReadLinesDouble(string input)
        => input.Split("\n\n").Select(ReadLines).ToList();
}