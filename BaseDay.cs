namespace AoC2022.Days;

public abstract class BaseDay
{
    public static List<string> ReadFile(string filename, string separator = "\r\n")
        => File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), $"Inputs/{filename}.txt")).Trim().Split(separator).ToList();

    public virtual List<Case> Part1Cases() => new();
    public virtual List<Case> Part2Cases() => new();

    public virtual int Part1(string filename) => 0;
    public virtual int Part2(string filename) => 0;

    public record Case(string Input, int Expected);

    public void Test1()
        => EvaluateCases(Part1Cases(), 1);

    public void Test2()
        => EvaluateCases(Part2Cases(), 2);

    internal void EvaluateCases(List<Case> cases, int part)
    {
        Console.WriteLine("\nTest Cases Part " + part);

        foreach (var c in cases)
        {
            var output = part switch
            {
                1 => Part1(c.Input),
                2 => Part2(c.Input),
                _ => throw new ArgumentOutOfRangeException()
            };
            if (output != c.Expected)
            {
                Console.WriteLine($"Failed on {c.Input} Expected: {c.Expected} Actual: {output}");
            }
            else
            {
                Console.WriteLine("Success");
            }
        }
        Console.WriteLine();
    }
}