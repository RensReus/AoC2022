


namespace AoC2025;

class Day12 : BaseDay
{
    // [Example(expected: 2, input: "0:\n###\n##.\n##.\n\n1:\n###\n##.\n.##\n\n2:\n.##\n###\n##.\n\n3:\n##.\n###\n##.\n\n4:\n###\n#..\n###\n\n5:\n###\n.#.\n###\n\n4x4: 0 0 0 0 2 0\n12x5: 1 0 1 0 2 2\n12x5: 1 0 1 0 3 2")]
    [Puzzle(expected: 546)]
    public static int Part1(string input)
    {
        var lines = ReadLinesDouble(input);
        var areas = lines[^1].Select(ParseArea).ToList();
        var shapeSizes = lines[..^1].Select(CountShapeSize).ToList();
        var shapes = lines[..^1].Select(l => l[1..]).ToList();
        var wel = 0;
        var niet = 0;
        var open = 0;
        foreach (var (row, col, packageRequirements) in areas)
        {
            var hetPast = KijkOfHetEchtPast(row, col, packageRequirements, shapeSizes);
            _ = hetPast switch
            {
                true => wel++,
                false => niet++,
                null => open++
            };
        }

        return wel;
    }

    private static bool? KijkOfHetEchtPast(int row, int col, List<int> packageRequirements, List<int> shapeSizes)
    {
        var required = 0;
        var available = row * col;
        foreach (var (shapeCount, i) in packageRequirements.Select((value, index) => (value, index)))
        {
            required += shapeSizes[i] * shapeCount;
        }
        if (required > available) { return false; }

        var totalShapes = packageRequirements.Sum();
        var total3x3 = (row / 3) * (col / 3);
        if (totalShapes <= total3x3) { return true; }

        return null;
    }

    private static (int row, int col, List<int> packageRequirements) ParseArea(string arg1)
    {
        var parts = arg1.Split(':');
        var row = int.Parse(parts[0].Split('x')[0]);
        var col = int.Parse(parts[0].Split('x')[1]);
        var packageRequirements = parts[1].Trim().Split().Select(int.Parse).ToList();
        return (row, col, packageRequirements);
    }

    private static int CountShapeSize(List<string> list)
    {
        return list[1..].Sum(line => line.Count(c => c == '#'));
    }
}