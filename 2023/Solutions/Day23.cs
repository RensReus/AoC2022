namespace AoC2023;

class Day23 : BaseDay
{
    [Example(expected: 94, input: "#.#####################\n#.......#########...###\n#######.#########.#.###\n###.....#.>.>.###.#.###\n###v#####.#v#.###.#.###\n###.>...#.#.#.....#...#\n###v###.#.#.#########.#\n###...#.#.#.......#...#\n#####.#.#.#######.#.###\n#.....#.#.#.......#...#\n#.#####.#.#.#########v#\n#.#...#...#...###...>.#\n#.#.#v#######v###.###v#\n#...#.>.#...>.>.#.###.#\n#####v#.#.###v#.#.###.#\n#.....#...#...#.#.#...#\n#.#########.###.#.#.###\n#...###...#...#...#.###\n###.###.#.###v#####v###\n#...#...#.#.>.>.#.>.###\n#.###.###.#.###.#.#v###\n#.....###...###...#...#\n#####################.#")]
    [Puzzle(expected: 2070)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);
        var forest = new Dictionary<(int, int), char>();
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                forest[(i, j)] = lines[i][j];
            }
        }
        var toEval = new Queue<(int, int)>();
        toEval.Enqueue((0, 1));
        var nodes = new Dictionary<(int, int), List<((int, int), int)>>();
        while (toEval.Count != 0)
        {
            var node = toEval.Dequeue();
            var connections = PossibleNext(node, forest).Select(x => FindNextSplit(node, x, forest, lines.Count - 1)).Where(x => x.Item2 != -1);
            nodes[node] = connections.ToList();
            foreach (var c in connections.Where(c => !nodes.ContainsKey(c.Item1)))
            {
                toEval.Enqueue(c.Item1);
            }
        }

        return MaxDistance((0, 1), nodes, [], lines.Count - 1, 0);
    }

    private static int MaxDistance((int, int) nextNode, Dictionary<(int, int), List<((int, int), int)>> nodes, HashSet<(int, int)> prevVisited, int endRow, int totalSteps)
    {
        var visited = prevVisited.Select(x => x).ToHashSet();
        visited.Add(nextNode);
        if (nextNode.Item1 == endRow) return totalSteps;
        if (nodes[nextNode].All(x => visited.Contains(x.Item1))) return -1;
        return nodes[nextNode].Where(x => !visited.Contains(x.Item1)).Max(x => MaxDistance(x.Item1, nodes, visited, endRow, totalSteps + x.Item2));
    }

    private static ((int, int), int) FindNextSplit((int, int) node, (int, int) n, Dictionary<(int, int), char> forest, int endRow)
    {
        var visited = new HashSet<(int, int)> { node, n };
        var position = n;
        var steps = 0;
        while (true)
        {
            steps++;
            var possibleNext = PossibleNext(position, forest).Where(x => !visited.Contains(x));
            if (possibleNext.Count() == 0) return ((-1, -1), -1);
            if (possibleNext.Count() > 1) return (position, steps);
            position = possibleNext.Single();
            if (position.Item1 == endRow) return (position, steps + 1);
            visited.Add(position);
        }
    }

    private static List<(int, int)> PossibleNext((int, int) position, Dictionary<(int, int), char> forest)
    {
        var possible = new List<(int, int)>();
        var down = (position.Item1 + 1, position.Item2);
        var up = (position.Item1 - 1, position.Item2);
        var left = (position.Item1, position.Item2 - 1);
        var right = (position.Item1, position.Item2 + 1);
        if (forest.TryGetValue(down, out char c) && ".v".Contains(c)) possible.Add(down);
        if (forest.TryGetValue(up, out char c1) && ".^".Contains(c1)) possible.Add(up);
        if (forest.TryGetValue(left, out char c2) && ".<".Contains(c2)) possible.Add(left);
        if (forest.TryGetValue(right, out char c3) && ".>".Contains(c3)) possible.Add(right);
        return possible;
    }

    [Example(expected: 154, input: "#.#####################\n#.......#########...###\n#######.#########.#.###\n###.....#.>.>.###.#.###\n###v#####.#v#.###.#.###\n###.>...#.#.#.....#...#\n###v###.#.#.#########.#\n###...#.#.#.......#...#\n#####.#.#.#######.#.###\n#.....#.#.#.......#...#\n#.#####.#.#.#########v#\n#.#...#...#...###...>.#\n#.#.#v#######v###.###v#\n#...#.>.#...>.>.#.###.#\n#####v#.#.###v#.#.###.#\n#.....#...#...#.#.#...#\n#.#########.###.#.#.###\n#...###...#...#...#.###\n###.###.#.###v#####v###\n#...#...#.#.>.>.#.>.###\n#.###.###.#.###.#.#v###\n#.....###...###...#...#\n#####################.#")]
    [Puzzle(expected: 6498)]
    public static int Part2(string input)
        => Part1(input.Replace('v', '.').Replace('>', '.').Replace('<', '.').Replace('^', '.'));
}