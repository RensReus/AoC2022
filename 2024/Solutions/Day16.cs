
namespace AoC2024;

class Day16 : BaseDay
{
    [Example(expected: 7036, input: "###############\n#.......#....E#\n#.#.###.#.###^#\n#.....#.#...#^#\n#.###.#####.#^#\n#.#.#.......#^#\n#.#.#####.###^#\n#..>>>>>>>>v#^#\n###^#.#####v#^#\n#>>^#.....#v#^#\n#^#.#.###.#v#^#\n#^....#...#v#^#\n#^###.#.#.#v#^#\n#S..#.....#>>^#\n###############")]
    [Example(expected: 11048, input: "#################\n#...#...#...#..E#\n#.#.#.#.#.#.#.#.#\n#.#.#.#...#...#.#\n#.#.#.#.###.#.#.#\n#...#.#.#.....#.#\n#.#.#.#.#.#####.#\n#.#...#.#.#.....#\n#.#.#####.#.###.#\n#.#.#.......#...#\n#.#.###.#####.###\n#.#.#...#.....#.#\n#.#.#.#####.###.#\n#.#.#.........#.#\n#.#.#.#########.#\n#S#.............#\n#################")]
    [Puzzle(expected: 95476)]
    public static int Part1(string input)
    {
        var maze = ReadLines(input);
        var start = ((maze.Count - 2, 1), (0, 1));
        var visited = new Dictionary<((int Row, int Col) Pos, (int Row, int Col) Dir), int> { [start] = 0 };
        var queue = new Dictionary<int, HashSet<((int Row, int Col) Pos, (int Row, int Col) Dir)>> { [0] = [start] };
        var end = (1, maze[0].Length - 2);

        var currSteps = -1;
        while (true)
        {
            currSteps++;
            if (!queue.ContainsKey(currSteps)) continue;
            var toEval = queue[currSteps];
            foreach (var item in toEval)
            {
                var newToEval = GetRotations(item, currSteps);
                newToEval.Add((((item.Pos.Row + item.Dir.Row, item.Pos.Col + item.Dir.Col), item.Dir), currSteps + 1));
                foreach (var (PosDir, Steps) in newToEval)
                {
                    if (PosDir.Pos == end) return Steps;
                    if (maze[PosDir.Pos.Row][PosDir.Pos.Col] == '#') continue;
                    if (visited.TryGetValue(PosDir, out var visitedSteps) && visitedSteps <= Steps) continue;
                    visited[PosDir] = Steps;
                    if (!queue.ContainsKey(Steps)) queue[Steps] = [];
                    queue[Steps].Add(PosDir);
                }
            }
        }
    }

    private static List<(((int Row, int Col) Pos, (int Row, int Col) Dir) PosDir, int Steps)> GetRotations(((int Row, int Col) Pos, (int Row, int Col) Dir) item, int steps)
    {
        var directions = new[] { (0, 1), (1, 0), (0, -1), (-1, 0) };
        var result = new List<(((int Row, int Col) Pos, (int Row, int Col) Dir), int)>();
        for (int i = 0; i < directions.Length; i++)
        {
            var newDir = directions[i];
            var currIndex = Array.IndexOf(directions, item.Dir);
            if (i == currIndex) continue;
            var cost = Math.Abs(currIndex - i) * 1000;
            if (cost == 3000) cost = 1000;
            result.Add(((item.Pos, newDir), steps + cost));
        }
        return result;
    }

    [Example(expected: 45, input: "###############\n#.......#....E#\n#.#.###.#.###^#\n#.....#.#...#^#\n#.###.#####.#^#\n#.#.#.......#^#\n#.#.#####.###^#\n#..>>>>>>>>v#^#\n###^#.#####v#^#\n#>>^#.....#v#^#\n#^#.#.###.#v#^#\n#^....#...#v#^#\n#^###.#.#.#v#^#\n#S..#.....#>>^#\n###############")]
    [Example(expected: 64, input: "#################\n#...#...#...#..E#\n#.#.#.#.#.#.#.#.#\n#.#.#.#...#...#.#\n#.#.#.#.###.#.#.#\n#...#.#.#.....#.#\n#.#.#.#.#.#####.#\n#.#...#.#.#.....#\n#.#.#####.#.###.#\n#.#.#.......#...#\n#.#.###.#####.###\n#.#.#...#.....#.#\n#.#.#.#####.###.#\n#.#.#.........#.#\n#.#.#.#########.#\n#S#.............#\n#################")]
    [Puzzle(expected: 511)]
    public static int Part2(string input)
    {
        var maze = ReadLines(input);
        var start = ((maze.Count - 2, 1), (0, 1));
        var visited = new Dictionary<((int Row, int Col) Pos, (int Row, int Col) Dir), int> { [start] = 0 };
        var queue = new Dictionary<int, HashSet<(((int Row, int Col) Pos, (int Row, int Col) Dir), HashSet<(int Row, int Col)>)>> { [0] = [(start, [])] };
        var end = (1, maze[0].Length - 2);

        var currSteps = -1;
        var posOnBestRoutes = new HashSet<(int Row, int Col)>();
        while (true)
        {
            currSteps++;
            if (!queue.ContainsKey(currSteps)) continue;
            var toEval = queue[currSteps];
            foreach (var (item, prevVisited) in toEval)
            {
                var newToEval = GetRotations(item, currSteps);
                newToEval.Add((((item.Pos.Row + item.Dir.Row, item.Pos.Col + item.Dir.Col), item.Dir), currSteps + 1));
                foreach (var (PosDir, Steps) in newToEval)
                {
                    if (PosDir.Pos == end)
                    {
                        posOnBestRoutes.UnionWith(prevVisited);
                        continue;
                    }
                    if (maze[PosDir.Pos.Row][PosDir.Pos.Col] == '#') continue;
                    if (visited.TryGetValue(PosDir, out var visitedSteps) && visitedSteps < Steps) continue;

                    var newPrevVisited = new HashSet<(int Row, int Col)>(prevVisited) { PosDir.Pos };
                    visited[PosDir] = Steps;
                    if (!queue.ContainsKey(Steps)) queue[Steps] = [];
                    queue[Steps].Add((PosDir, newPrevVisited));
                }
            }
            if (posOnBestRoutes.Count > 0) break;
        }
        return posOnBestRoutes.Count + 1;
    }
}