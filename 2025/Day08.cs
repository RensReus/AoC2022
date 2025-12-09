namespace AoC2025;

class Day08 : BaseDay
{
    [Example(expected: 40, input: "162,817,812\n57,618,57\n906,360,560\n592,479,940\n352,342,300\n466,668,158\n542,29,236\n431,825,988\n739,650,466\n52,470,668\n216,146,977\n819,987,18\n117,168,530\n805,96,715\n346,949,466\n970,615,88\n941,993,340\n862,61,35\n984,92,344\n425,690,689")]
    [Puzzle(expected: 123930)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);

        var (connectionsToMake, groups) = GetSortedConnections(lines);

        var targetConnections = lines.Count > 100 ? 1000 : 10;

        foreach (var (from, to, _) in connectionsToMake.Take(targetConnections))
        {
            var group1 = groups[from];
            var group2 = groups[to];

            foreach (var (key, value) in groups)
            {
                if (value == group2)
                {
                    groups[key] = group1;
                }
            }
        }

        var groupsDistinct = groups.Values.Distinct().ToList();
        var groupsWithSize = groupsDistinct.Select(g => groups.Count(kv => kv.Value == g)).OrderByDescending(s => s).ToList();

        return groupsWithSize[0] * groupsWithSize[1] * groupsWithSize[2];
    }

    private static (List<(string from, string to, double dist)> connectionsToMake, Dictionary<string, int> groups) GetSortedConnections(List<string> lines)
    {
        var connectionsToMake = new List<(string from, string to, double dist)>();
        var groups = new Dictionary<string, int>();
        for (int i = 0; i < lines.Count; i++)
        {
            var (x1, y1, z1) = GetCoords(lines[i]);
            groups[lines[i]] = i;
            for (int j = i + 1; j < lines.Count; j++)
            {
                var (x2, y2, z2) = GetCoords(lines[j]);
                var dist = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2));
                connectionsToMake.Add((lines[i], lines[j], dist));
            }
        }

        connectionsToMake = connectionsToMake.OrderBy(c => c.dist).ToList();
        return (connectionsToMake, groups);
    }

    private static (int x1, int y1, int z1) GetCoords(string v)
    {
        var parts = v.Split(',');
        return (int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
    }

    [Example(expected: 25272, input: "162,817,812\n57,618,57\n906,360,560\n592,479,940\n352,342,300\n466,668,158\n542,29,236\n431,825,988\n739,650,466\n52,470,668\n216,146,977\n819,987,18\n117,168,530\n805,96,715\n346,949,466\n970,615,88\n941,993,340\n862,61,35\n984,92,344\n425,690,689")]
    [Puzzle(expected: 27338688)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input);

        var (connectionsToMake, groups) = GetSortedConnections(lines);

        foreach (var (from, to, _) in connectionsToMake)
        {
            var group1 = groups[from];
            var group2 = groups[to];
            if (group1 == group2)
            {
                continue;
            }
            foreach (var (key, value) in groups)
            {
                if (value == group2)
                {
                    groups[key] = group1;
                }
            }
            if (groups.Values.Distinct().Count() == 1)
            {
                return int.Parse(from.Split(',')[0]) * int.Parse(to.Split(',')[0]);
            }
        }
        return -1;
    }
}
