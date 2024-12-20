namespace AoC2024;

class Day20 : BaseDay
{
    [Puzzle(expected: 1307)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);
        var maze = BuildMaze(lines);
        var start = maze.First(x => x.Value == 'S').Key;
        var end = maze.First(x => x.Value == 'E').Key;
        var path = new Dictionary<(int, int), int> { [start] = 0 };
        var steps = 0;
        var nextStep = start;
        while (true)
        {
            steps++;
            var neighbor = GetNeighbor(nextStep, maze, path);
            path[neighbor] = steps;
            if (neighbor == end)
            {
                break;
            }
            nextStep = neighbor;
        }

        var jumpCounts = new Dictionary<int, int>();
        var ans = 0;
        foreach (var item in path)
        {
            var possibleJumps = GetJumps(item, maze, path);
            foreach (var jump in possibleJumps)
            {
                if (!jumpCounts.ContainsKey(jump.Item2))
                {
                    jumpCounts[jump.Item2] = 0;
                }
                jumpCounts[jump.Item2]++;
                if (jump.Item2 >= 100) ans++;
            }
        }

        return ans;
    }

    private static List<((int, int), int)> GetJumps(KeyValuePair<(int, int), int> item, Dictionary<(int X, int Y), char> maze, Dictionary<(int, int), int> path)
    {
        var x = item.Key.Item1;
        var y = item.Key.Item2;
        var jumps = new List<(int, int)> { (x - 2, y), (x + 2, y), (x, y - 2), (x, y + 2) };
        var currSteps = item.Value;
        return jumps
            .Where(x => maze.TryGetValue(x, out var field) && field is '.' or 'E' && path[x] > currSteps)
            .Select(x => (x, path[x] - currSteps - 2)).ToList();
    }

    private static (int, int) GetNeighbor((int, int) item, Dictionary<(int X, int Y), char> maze, Dictionary<(int, int), int> path)
    {
        var x = item.Item1;
        var y = item.Item2;
        var possible = new List<(int, int)> { (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1) };
        foreach (var neighbor in possible)
        {
            if (!path.ContainsKey(neighbor) && maze[neighbor] is '.' or 'E')
            {
                return neighbor;
            }
        }
        return (-1, -1);
    }

    private static Dictionary<(int X, int Y), char> BuildMaze(List<string> list)
    {
        var maze = new Dictionary<(int X, int Y), char>();
        for (var y = 0; y < list.Count; y++)
        {
            for (var x = 0; x < list[y].Length; x++)
            {
                maze[(x, y)] = list[y][x];
            }
        }

        return maze;
    }

    [Example(expected: 0, input: "###############\n#...#...#.....#\n#.#.#.#.#.###.#\n#S#...#.#.#...#\n#######.#.#.###\n#######.#.#...#\n#######.#.###.#\n###..E#...#...#\n###.#######.###\n#...###...#...#\n#.#####.#.###.#\n#.#...#.#.#...#\n#.#.#.#.#.#.###\n#...#...#...###\n###############")]
    [Puzzle(expected: 986545)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input);
        var maze = BuildMaze(lines);
        var start = maze.First(x => x.Value == 'S').Key;
        var end = maze.First(x => x.Value == 'E').Key;
        var path = new Dictionary<(int, int), int> { [start] = 0 };
        var steps = 0;
        var nextStep = start;
        while (true)
        {
            steps++;
            var neighbor = GetNeighbor(nextStep, maze, path);
            path[neighbor] = steps;
            if (neighbor == end)
            {
                break;
            }
            nextStep = neighbor;
        }

        var jumpCounts = new Dictionary<int, int>();
        var ans = 0;
        foreach (var item in path)
        {
            var possibleJumps = GetJumps2(item, maze, path);
            foreach (var jump in possibleJumps)
            {
                if (!jumpCounts.ContainsKey(jump.Value))
                {
                    jumpCounts[jump.Value] = 0;
                }
                jumpCounts[jump.Value]++;
                if (jump.Value >= 100) ans++;
            }
        }
        var a = jumpCounts.OrderBy(x => x.Key).ToList();
        return ans;
    }

    private static Dictionary<(int, int), int> GetJumps2(KeyValuePair<(int, int), int> item, Dictionary<(int X, int Y), char> maze, Dictionary<(int, int), int> path)
    {
        var response = new Dictionary<(int, int), int>();
        var currSteps = item.Value;
        var toEvalList = new List<(int, int)> { item.Key };
        var evaluated = new HashSet<(int, int)> { item.Key };
        for (int sec = 1; sec <= 20; sec++)
        {
            if (toEvalList.Count == 0) break;
            var newToEval = new List<(int, int)>();
            foreach (var toEval in toEvalList)
            {
                var neighbours = new List<(int, int)> { (toEval.Item1 - 1, toEval.Item2), (toEval.Item1 + 1, toEval.Item2), (toEval.Item1, toEval.Item2 - 1), (toEval.Item1, toEval.Item2 + 1) };
                foreach (var neighbor in neighbours)
                {
                    if (path.TryGetValue(neighbor, out var steps) && (steps - sec) > currSteps)
                    {
                        if (!response.ContainsKey(neighbor))
                        {
                            response[neighbor] = steps - currSteps - sec;
                        }
                    }
                    if (maze.TryGetValue(neighbor, out var field) && !evaluated.Contains(neighbor))
                    {
                        newToEval.Add(neighbor);
                        evaluated.Add(neighbor);
                    }
                }
            }
            toEvalList = newToEval;
        }
        return response;
    }
}