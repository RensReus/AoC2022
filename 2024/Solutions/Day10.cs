namespace AoC2024;

class Day10 : BaseDay
{
    [Example(expected: 36, input: "89010123\n78121874\n87430965\n96549874\n45678903\n32019012\n01329801\n10456732")]
    [Puzzle(expected: 816)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);
        var ans = 0;
        for (var i = 0; i < lines.Count; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                ans += DistinctRouteDesitnations(i, j, lines).Distinct().Count();
            }
        }
        return ans;
    }

    [Example(expected: 81, input: "89010123\n78121874\n87430965\n96549874\n45678903\n32019012\n01329801\n10456732")]
    [Puzzle(expected: 1960)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input);
        var ans = 0;
        for (var i = 0; i < lines.Count; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                ans += DistinctRouteDesitnations(i, j, lines).Count;
            }
        }
        return ans;
    }

    private static List<(int, int)> DistinctRouteDesitnations(int i, int j, List<string> lines)
    {
        if (lines[i][j] != '0') return [];
        var destinations = new List<(int, int)>();
        var queue = new Queue<(int, int)>();
        queue.Enqueue((i, j));
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var currentVal = lines[current.Item1][current.Item2];
            if (currentVal == '9')
            {
                destinations.Add(current);
                continue;
            };
            foreach (var neighbor in GetNeighbors(current, lines))
            {
                var neighborVal = lines[neighbor.Item1][neighbor.Item2];
                if (neighborVal - currentVal != 1) continue;
                queue.Enqueue(neighbor);
            }
        }
        return destinations;
    }

    private static IEnumerable<(int, int)> GetNeighbors((int, int) current, List<string> lines)
    {
        var (i, j) = current;
        return new List<(int, int)> { (i - 1, j), (i + 1, j), (i, j - 1), (i, j + 1) }
            .Where(x => x.Item1 >= 0 && x.Item1 < lines.Count && x.Item2 >= 0 && x.Item2 < lines[0].Length);
    }
}