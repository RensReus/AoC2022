namespace AoC2022;

static class Day18
{
    static HashSet<Coord3D> ProcessInput(string input)
    {
        return input.Split(";").Select(x => new Coord3D(x)).ToHashSet();
    }

    [Example(expected: 64, input: "2,2,2;1,2,2;3,2,2;2,1,2;2,3,2;2,2,1;2,2,3;2,2,4;2,2,6;1,2,5;3,2,5;2,1,5;2,3,5")]
    [Puzzle(expected: 3396)]
    public static int Part1(string input)
        => CalcSurface(ProcessInput(input));

    private static int CalcSurface(HashSet<Coord3D> processedInput)
    {
        var totalSurface = 0;
        var input = processedInput.ToList();
        for (int i = 0; i < input.Count; i++)
        {
            totalSurface += 6;
            for (int j = i + 1; j < input.Count; j++)
            {
                if (TouchFace(input[i], input[j])) totalSurface -= 2;
            }
        }
        return totalSurface;
    }

    private static bool TouchFace(Coord3D block1, Coord3D block2)
    {
        var diff0 = int.Abs(block1.X - block2.X);
        var diff1 = int.Abs(block1.Y - block2.Y);
        var diff2 = int.Abs(block1.Z - block2.Z);
        return diff0 + diff1 + diff2 == 1;
    }

    [Example(expected: 58, input: "2,2,2;1,2,2;3,2,2;2,1,2;2,3,2;2,2,1;2,2,3;2,2,4;2,2,6;1,2,5;3,2,5;2,1,5;2,3,5")]
    [Puzzle(expected: 2044)]
    public static int Part2(string input)
    {
        var processedInput = ProcessInput(input);
        var max = GetMaximum(processedInput);
        var min = GetMinimum(processedInput);
        var allShapes = new List<HashSet<Coord3D>> { processedInput };
        for (int i = min.X; i < max.X; i++)
        {
            for (int j = min.Y; j < max.Y; j++)
            {
                for (int k = min.Z; k < max.Z; k++)
                {
                    var block = new Coord3D(i, j, k);
                    if (allShapes.Any(shape => shape.Contains(block))) continue;
                    allShapes.Add(GetAirShape(new Coord3D(i, j, k), min, max, allShapes));
                }
            }
        }
        return CalcSurface(processedInput) - allShapes.Skip(2).Sum(x => CalcSurface(x));
    }

    // TODO vind de bug oorzaak
    private static HashSet<Coord3D> GetAirShapeRecursive(Coord3D block, Coord3D min, Coord3D max, List<HashSet<Coord3D>> allShapes, HashSet<Coord3D> newShape, int depth)
    {
        if (depth == 3300)
        {
            Console.WriteLine(newShape.Count);
        }
        if (newShape.Count % 10 == 0 && newShape.Count > 3600)
        {
            Console.WriteLine(newShape.Count);
        }
        var neighbours = GetUnevalNeighbours(block, allShapes, newShape, min, max);
        foreach (var neighbour in neighbours)
        {
            if (allShapes.Any(shape => shape.Contains(neighbour)) || newShape.Contains(neighbour) || OutsideScope(neighbour, min, max)) continue;
            newShape.Add(neighbour);
            GetAirShapeRecursive(neighbour, min, max, allShapes, newShape, depth + 1);
        }
        return newShape;
    }

    private static HashSet<Coord3D> GetAirShape(Coord3D block, Coord3D min, Coord3D max, List<HashSet<Coord3D>> allShapes)
    {
        var newShape = new HashSet<Coord3D>();
        var newBlocks = new HashSet<Coord3D> { block };

        while (newBlocks.Count() > 0)
        {
            newShape.UnionWith(newBlocks);
            var toEvalBlocks = newBlocks;
            newBlocks = new HashSet<Coord3D>();

            foreach (var n in toEvalBlocks)
            {
                newBlocks.UnionWith(GetUnevalNeighbours(n, allShapes, newShape, min, max));
            }
        }
        return newShape;
    }

    private static HashSet<Coord3D> GetUnevalNeighbours(Coord3D block, List<HashSet<Coord3D>> allShapes, HashSet<Coord3D> newShape, Coord3D min, Coord3D max)
    {
        (int i, int j, int k) = (block.X, block.Y, block.Z);
        return new List<Coord3D> {
            new Coord3D(i+1, j, k ), new Coord3D(i-1, j, k ),
            new Coord3D(i, j+1, k ), new Coord3D(i, j-1, k ),
            new Coord3D(i, j, k+1 ), new Coord3D(i, j, k-1 ) }
            .Where(c => IsUnevaluated(c, allShapes, newShape, min, max)).ToHashSet();
    }

    private static bool IsUnevaluated(Coord3D c, List<HashSet<Coord3D>> allShapes, HashSet<Coord3D> newShape, Coord3D min, Coord3D max)
    {
        var inPrevshapes = allShapes.Any(shape => shape.Contains(c));
        var inNewShape = newShape.Contains(c);
        return !inPrevshapes && !inNewShape && InsideScope(c, min, max);
    }

    private static bool InsideScope(Coord3D neighbour, Coord3D min, Coord3D max)
        => neighbour.X >= min.X && neighbour.X <= max.X
        && neighbour.Y >= min.Y && neighbour.Y <= max.Y
        && neighbour.Z >= min.Z && neighbour.Z <= max.Z;

    private static bool OutsideScope(Coord3D neighbour, Coord3D min, Coord3D max)
        => neighbour.X < min.X || neighbour.X > max.X
        || neighbour.Y < min.Y || neighbour.Y > max.Y
        || neighbour.Z < min.Z || neighbour.Z > max.Z;


    private static Coord3D GetMinimum(HashSet<Coord3D> processedInput)
    {
        var x = int.MaxValue;
        var y = int.MaxValue;
        var z = int.MaxValue;
        foreach (var block in processedInput)
        {
            x = int.Min(x, block.X);
            y = int.Min(y, block.Y);
            z = int.Min(z, block.Z);
        }
        return new(x - 1, y - 1, z - 1);
    }

    private static Coord3D GetMaximum(HashSet<Coord3D> processedInput)
    {
        var x = int.MinValue;
        var y = int.MinValue;
        var z = int.MinValue;
        foreach (var block in processedInput)
        {
            x = int.Max(x, block.X);
            y = int.Max(y, block.Y);
            z = int.Max(z, block.Z);
        }
        return new(x + 1, y + 1, z + 1);
    }
}

internal class Coord3D
{
    public int X;
    public int Y;
    public int Z;

    public Coord3D(string x)
    {
        var a = x.Split(",").Select(y => int.Parse(y)).ToArray();
        X = a[0];
        Y = a[1];
        Z = a[2];
    }

    public Coord3D(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override bool Equals(object? other)
        => other is Coord3D c && X == c.X && Y == c.Y && Z == c.Z;

    public override int GetHashCode()
        => HashCode.Combine(X, Y, Z);
}