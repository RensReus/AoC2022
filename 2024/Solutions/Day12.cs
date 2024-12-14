namespace AoC2024;

class Day12 : BaseDay
{
    [Example(expected: 1930, input: "RRRRIICCFF\nRRRRIICCCF\nVVRRRCCFFF\nVVRCCCJFFF\nVVVVCJJCFE\nVVIVCCJJEE\nVVIIICJJEE\nMIIIIIJJEE\nMIIISIJEEE\nMMMISSJEEE")]
    [Puzzle(expected: 1396562)]
    public static int Part1(string input)
        => GetGroups(input).Sum(x => x.Item1 * x.Item3);

    [Example(expected: 1206, input: "RRRRIICCFF\nRRRRIICCCF\nVVRRRCCFFF\nVVRCCCJFFF\nVVVVCJJCFE\nVVIVCCJJEE\nVVIIICJJEE\nMIIIIIJJEE\nMIIISIJEEE\nMMMISSJEEE")]
    [Puzzle(expected: 844132)]
    public static int Part2(string input)
        => GetGroups(input).Sum(x => x.Item1 * x.Item2);

    private static List<(int, int, int)> GetGroups(string input)
    {
        var lines = ReadLines(input);
        var groups = new List<(int, int, int)>();
        var evaluated = new HashSet<(int, int)>();
        for (int x = 0; x < lines.Count; x++)
        {
            for (int y = 0; y < lines[x].Length; y++)
            {
                if (evaluated.Contains((x, y))) continue;
                var (group, corners, perimeter) = GetGroupInfo(x, y, lines);
                groups.Add((group.Count, corners, perimeter));
                evaluated.UnionWith(group);
            }
        }
        return groups;
    }

    public static (HashSet<(int, int)> Group, int CornerCount, int Perimeter) GetGroupInfo(int startX, int startY, List<string> lines)
    {
        var groupA = new HashSet<(int, int)>();
        var currChar = lines[startX][startY];
        var toEval = new Queue<(int, int)>();
        toEval.Enqueue((startX, startY));
        var perimeter = 0;
        var corners = 0;
        while (toEval.Any())
        {
            var (x, y) = toEval.Dequeue();
            if (groupA.Contains((x, y))) continue;
            groupA.Add((x, y));
            var neighbours = GetNeighbours(x, y, lines, currChar);
            perimeter += 4 - neighbours.Count;
            neighbours.Where(x => !groupA.Contains(x)).ToList().ForEach(toEval.Enqueue);
            corners += CornerCount(x, y, lines, currChar);
        }
        return (groupA, corners, perimeter);
    }

    public static int CornerCount(int startX, int startY, List<string> lines, char currChar)
    {
        var neighbours = GetNeighbours(startX, startY, lines, currChar);
        var neighbourCount = neighbours.Count;
        if (neighbourCount == 0) return 4;
        if (neighbourCount == 1) return 2;

        var corners = 0;
        if (neighbours.Count == 2) // outside corner
        {
            var (x1, y1) = neighbours[0];
            var (x2, y2) = neighbours[1];
            if (x1 != x2 && y1 != y2)
            {
                corners++;
            }
        }

        // inside corner
        for (int i = 0; i < neighbours.Count; i++)
        {
            var (x1, y1) = neighbours[i];
            for (int l = i + 1; l < neighbours.Count; l++)
            {
                var (x2, y2) = neighbours[l];
                if (x1 != x2 && y1 != y2 && (lines[x1][y2] != currChar || lines[x2][y1] != currChar))
                {
                    corners++;
                }
            }
        }

        return corners;
    }

    private static List<(int, int)> GetNeighbours(int x, int y, List<string> lines, char currChar)
    {
        var result = new List<(int, int)>();
        if (y > 0 && lines[x][y - 1] == currChar) result.Add((x, y - 1));
        if (x > 0 && lines[x - 1][y] == currChar) result.Add((x - 1, y));
        if (y < lines[x].Length - 1 && lines[x][y + 1] == currChar) result.Add((x, y + 1));
        if (x < lines.Count - 1 && lines[x + 1][y] == currChar) result.Add((x + 1, y));

        return result;
    }
}