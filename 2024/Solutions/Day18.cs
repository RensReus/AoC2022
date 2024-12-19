namespace AoC2024;

class Day18 : BaseDay
{
    [Example(expected: 22, input: "6\n12\n\n5,4\n4,2\n4,5\n3,0\n2,1\n6,3\n2,4\n1,5\n0,6\n3,3\n2,6\n5,1\n1,2\n5,5\n2,5\n6,5\n1,4\n0,4\n6,4\n1,1\n6,1\n1,0\n0,5\n1,6\n2,0")]
    [Puzzle(expected: 404)]
    public static int Part1(string input)
    {
        var lines = ReadLinesDouble(input);
        var size = int.Parse(lines[0][0]);
        var maze = BuildMaze(lines[1], int.Parse(lines[0][1]));
        var visited = new HashSet<(int, int)>();
        var start = (0, 0);
        var end = (size, size);
        var toEval = new List<(int, int)> { start };

        var steps = 0;
        while (!toEval.Contains(end))
        {
            var newToEval = new List<(int, int)>();
            foreach (var item in toEval)
            {
                // getneighbors
                var neighbors = GetNeighbors(item, size);
                foreach (var neighbor in neighbors)
                {
                    if (visited.Contains(neighbor) || maze.Contains(neighbor))
                    {
                        continue;
                    }
                    visited.Add(neighbor);
                    newToEval.Add(neighbor);
                }
            }

            toEval = newToEval;
            steps++;
        }
        return steps;
    }

    [Example(expected: "6,1", input: "6\n12\n\n5,4\n4,2\n4,5\n3,0\n2,1\n6,3\n2,4\n1,5\n0,6\n3,3\n2,6\n5,1\n1,2\n5,5\n2,5\n6,5\n1,4\n0,4\n6,4\n1,1\n6,1\n1,0\n0,5\n1,6\n2,0")]
    [Puzzle(expected: "27,60")]
    public static string Part2(string input)
    {
        var lines = ReadLinesDouble(input);
        var size = int.Parse(lines[0][0]);
        var nextToAdd = int.Parse(lines[0][1]);
        var maze = BuildMaze(lines[1], nextToAdd);

        while (MazeCanBeSolved(maze, size))
        {
            var points = lines[1][nextToAdd].Split(',');
            var x = int.Parse(points[0]);
            var y = int.Parse(points[1]);
            maze.Add((x, y));
            nextToAdd++;
        }
        return lines[1][nextToAdd - 1];
    }

    private static bool MazeCanBeSolved(HashSet<(int, int)> maze, int size)
    {
        var visited = new HashSet<(int, int)>();
        var start = (0, 0);
        var end = (size, size);
        var toEval = new List<(int, int)> { start };

        var steps = 0;

        while (toEval.Count > 0)
        {
            var newToEval = new List<(int, int)>();
            foreach (var item in toEval)
            {
                var neighbors = GetNeighbors(item, size);
                if (neighbors.Contains(end))
                {
                    return true;
                }
                foreach (var neighbor in neighbors)
                {
                    if (visited.Contains(neighbor) || maze.Contains(neighbor))
                    {
                        continue;
                    }
                    visited.Add(neighbor);
                    newToEval.Add(neighbor);
                }
            }

            toEval = newToEval;
            steps++;
        }
        return false;
    }

    private static List<(int, int)> GetNeighbors((int, int) item, int size)
    {
        var neighbors = new List<(int, int)>();
        var x = item.Item1;
        var y = item.Item2;
        if (x - 1 >= 0) neighbors.Add((x - 1, y));
        if (x + 1 <= size) neighbors.Add((x + 1, y));
        if (y - 1 >= 0) neighbors.Add((x, y - 1));
        if (y + 1 <= size) neighbors.Add((x, y + 1));

        return neighbors;
    }

    private static HashSet<(int, int)> BuildMaze(IList<string> lines, int stop)
    {
        var maze = new HashSet<(int, int)>();
        for (int i = 0; i < stop; i++)
        {
            if (i >= lines.Count) break;
            var points = lines[i].Split(',');
            var x = int.Parse(points[0]);
            var y = int.Parse(points[1]);
            maze.Add((x, y));
        }

        return maze;
    }
}