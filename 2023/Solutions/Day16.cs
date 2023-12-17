namespace AoC2023;

class Day16 : BaseDay
{
    [Example(expected: 46, 1)]
    [Puzzle(expected: 7939)]
    public static int Part1(string input)
        => CountVisited(input, new(new(0, 0), new(0, 1)));

    private static int CountVisited(string input, PointWithDirection initial)
    {
        var lines = ReadLines(input);
        var stepsToEval = new List<PointWithDirection> { initial };
        var visited = new List<PointWithDirection> { initial };
        while (stepsToEval.Count > 0)
        {
            var newSteps = new List<PointWithDirection>();
            foreach (var step in stepsToEval)
            {
                var next = NextSteps(step, lines);
                foreach (var item in next)
                {
                    if (!visited.Contains(item) && !newSteps.Contains(item) && item.Position.InBounds(lines.Count, lines[0].Length))
                    {
                        visited.Add(item);
                        newSteps.Add(item);
                    }
                }
            }
            stepsToEval = newSteps;
        }
        return visited.Select(x => x.Position).Distinct().Count();
    }

    private static List<PointWithDirection> NextSteps(PointWithDirection ray, List<string> lines)
    {
        var cell = lines[ray.Position.Row][ray.Position.Col];
        if (ShouldSplit(ray.Direction, cell)) return SplitRays(ray);
        ray = new(ray.Position, UpdateDir(ray.Direction, cell));     // ook iets voor in extention 
        return [ray.Move()];
    }

    private static Vec2D UpdateDir(Vec2D dir, char cell)
        => cell switch
        {
            '\\' => dir.ReflectPositive(),
            '/' => dir.ReflectNegative(),
            _ => dir
        };

    private static List<PointWithDirection> SplitRays(PointWithDirection ray)
        => [
            new(ray.Position, ray.Direction.ReflectPositive()),
            new(ray.Position, ray.Direction.ReflectNegative())];

    private static bool ShouldSplit(Vec2D dir, char cell)
        => (cell == '-' && dir.Row != 0) || (cell == '|' && dir.Col != 0);

    [Example(expected: 51, 1)]
    [Puzzle(expected: 8318)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input);
        var max = 0;
        for (int i = 0; i < lines[0].Length; i++)
        {
            max = int.Max(max, CountVisited(input, new(new(0, i), new(1, 0))));
            max = int.Max(max, CountVisited(input, new(new(lines.Count - 1, i), new(-1, 0))));
        }
        for (int i = 0; i < lines.Count; i++)
        {
            max = int.Max(max, CountVisited(input, new(new(i, 0), new(0, 1))));
            max = int.Max(max, CountVisited(input, new(new(i, lines[0].Length - 1), new(0, -1))));
        }
        return max;
    }

    private readonly struct Vec2D(int x, int y)
    {
        public int Row { get; } = x;
        public int Col { get; } = y;

        public bool InBounds(int rowMax, int colMax, int rowMin = 0, int colMin = 0)
            => Row >= rowMin && Row < rowMax && Col >= colMin && Col < colMax;

        public static Vec2D operator +(Vec2D left, Vec2D right)
            => new(left.Row + right.Row, left.Col + right.Col);

        public static Vec2D operator -(Vec2D left, Vec2D right)
            => new(left.Row - right.Row, left.Col - right.Col);

        public Vec2D ReflectPositive() // TODO deze twee toch in pointwithdirection en misschien in extension class of zo
            => new(Col, Row);

        public Vec2D ReflectNegative()
            => new(-Col, -Row);
    }

    private record PointWithDirection(Vec2D Position, Vec2D Direction)
    {
        public PointWithDirection Move()
            => new(Position + Direction, Direction);
    }
}