
namespace AoC2023;

class Day16 : BaseDay
{
    [Example(expected: 46, 1)]
    [Puzzle(expected: 7939)]
    public static int Part1(string input)
        => CountVisited(input, ((0, 0), (0, 1)));

    private static int CountVisited(string input, ((int, int), (int, int)) initial)
    {
        var lines = ReadLines(input);
        var stepsToEval = new List<((int, int), (int, int))> { initial };
        var visited = new List<((int, int), (int, int))> { initial };
        while (stepsToEval.Count > 0)
        {
            var newSteps = new List<((int, int), (int, int))>();
            foreach (var step in stepsToEval)
            {
                var next = NextSteps(step.Item1, step.Item2, lines);
                foreach (var item in next)
                {
                    if (!visited.Contains(item) && !newSteps.Contains(item) && InBounds(lines, item.Item1))
                    {
                        visited.Add(item);
                        newSteps.Add(item);
                    }
                }
            }
            stepsToEval = newSteps;
        }

        return visited.Select(x => x.Item1).Distinct().Count();
    }

    private static bool InBounds(List<string> lines, (int, int) pos)
        => pos.Item1 >= 0 && pos.Item1 < lines.Count && pos.Item2 >= 0 && pos.Item2 < lines[0].Length;

    private static List<((int, int), (int, int))> NextSteps((int, int) pos, (int, int) dir, List<string> lines)
    {
        var cell = lines[pos.Item1][pos.Item2];
        if (ShouldSplit(dir, cell)) return SplitRays(pos, dir);
        dir = UpdateDir(dir, cell);
        pos = (pos.Item1 + dir.Item1, pos.Item2 + dir.Item2);
        return [(pos, dir)];
    }

    private static (int, int) UpdateDir((int, int) dir, char cell)
        => cell switch
        {
            '\\' => (dir.Item2, dir.Item1),
            '/' => (-dir.Item2, -dir.Item1),
            _ => dir
        };

    private static List<((int, int), (int, int))> SplitRays((int, int) pos, (int, int) dir)
        => [(pos, (dir.Item2, dir.Item1)), (pos, (-dir.Item2, -dir.Item1))];

    private static bool ShouldSplit((int, int) dir, char cell)
        => (cell == '-' && dir.Item1 != 0) || (cell == '|' && dir.Item2 != 0);

    [Example(expected: 51, 1)]
    [Puzzle(expected: 8318)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input);
        var max = 0;
        for (int i = 0; i < lines[0].Length; i++)
        {
            max = int.Max(max, CountVisited(input, ((0, i), (1, 0))));
            max = int.Max(max, CountVisited(input, ((lines.Count - 1, i), (-1, 0))));
        }
        for (int i = 0; i < lines.Count; i++)
        {
            max = int.Max(max, CountVisited(input, ((i, 0), (0, 1))));
            max = int.Max(max, CountVisited(input, ((i, lines[0].Length - 1), (0, -1))));
        }
        return max;
    }
}