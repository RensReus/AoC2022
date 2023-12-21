
namespace AoC2023;

class Day21 : BaseDay
{
    [Example(expected: 1111111, input: "AAAAA")]
    [Puzzle(expected: 3671)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);
        var visitedEven = new Dictionary<(int, int), bool>();
        var visitedOdd = new Dictionary<(int, int), bool>();
        var startRow = lines.FindIndex(x => x.Contains('S'));
        var startCol = lines[startRow].IndexOf('S');
        var toEval = new List<(int, int)> { (startRow, startCol) };

        var steps = 0;
        for (int i = 0; i <= 64; i++)
        {
            steps++;
            var nextToEval = new List<(int, int)>();
            foreach (var item in toEval)
            {
                foreach (var n in GetNeighbours(item))
                {
                    if (lines[n.Item1][n.Item2] == '#') continue;
                    if (steps % 2 == 0 && !visitedEven.ContainsKey(n))
                    {
                        visitedEven[n] = true;
                        nextToEval.Add(n);
                    }
                    if (steps % 2 != 0 && !visitedOdd.ContainsKey(n))
                    {
                        visitedOdd[n] = true;
                        nextToEval.Add(n);
                    }
                }
            }
            toEval = nextToEval;
        }
        Console.WriteLine(visitedEven.Intersect(visitedOdd).Count());
        return visitedEven.Count;
    }

    private static List<(int, int)> GetNeighbours((int, int) item)
        => [(item.Item1 + 1, item.Item2), (item.Item1 - 1, item.Item2), (item.Item1, item.Item2 - 1), (item.Item1, item.Item2 + 1)];

    [Example(expected: 1111111, input: "AAAAA")]
    // [Puzzle(expected: 222222)]
    public static int Part2(string input)
    {
        // van elk punt naar elk ander punt het minimale aantal stappen om daar in even of oneven te komen
        // hoeft misschien alleen vanaf S en elk rand punt

        var lines = ReadLines(input);
        return 1111111;
    }
}