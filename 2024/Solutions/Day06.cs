namespace AoC2024;

class Day06 : BaseDay
{
    [Example(expected: 41, input: "....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#...")]
    [Puzzle(expected: 5551)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);
        var pos = (0, 0);
        var obstacles = new HashSet<(int, int)>();
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                if (lines[i][j] == '^')
                {
                    pos = (i, j);
                }
                if (lines[i][j] == '#')
                {
                    obstacles.Add((i, j));
                }
            }
        }
        var dir = (-1, 0);
        var visited = GetVisited(pos, dir, obstacles, lines);
        return visited.Select(x => x.Item1).Distinct().Count();
    }

    private static List<((int, int), (int, int))> GetVisited((int, int) pos, (int, int) dir, HashSet<(int, int)> obstacles, List<string> lines)
    {
        var visited = new List<((int, int), (int, int))>() { (pos, dir) };
        while (true)
        {
            var newPos = (pos.Item1 + dir.Item1, pos.Item2 + dir.Item2);
            if (obstacles.Contains(newPos))
            {
                dir = (dir.Item2, -dir.Item1);
            }
            else
            {
                pos = newPos;
            }
            if (!Inbounds(pos, lines)) break;
            visited.Add((pos, dir));
        }
        return visited;
    }

    private static bool Inbounds((int, int) pos, List<string> lines)
    {
        return pos.Item1 >= 0 && pos.Item1 < lines.Count && pos.Item2 >= 0 && pos.Item2 < lines[0].Length;
    }

    [Example(expected: 6, input: "....#.....\n.........#\n..........\n..#.......\n.......#..\n..........\n.#..^.....\n........#.\n#.........\n......#...")]
    [Puzzle(expected: 1939)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input);
        var startPos = (0, 0);
        var obstacles = new HashSet<(int, int)>();
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                if (lines[i][j] == '^')
                {
                    startPos = (i, j);
                }
                if (lines[i][j] == '#')
                {
                    obstacles.Add((i, j));
                }
            }
        }
        var visited = GetVisited(startPos, (-1, 0), obstacles, lines);
        var checkedPositions = new HashSet<(int, int)> { startPos };
        var answer = 0;
        var alreadyVisited = new HashSet<((int, int), (int, int))>();
        for (int i = 1; i < visited.Count; i++)
        {
            var newObstacle = visited[i].Item1;
            var startPosDir = visited[i - 1]; ;
            alreadyVisited.Add(startPosDir);
            if (!checkedPositions.Contains(newObstacle) && CreatesLoop(obstacles, newObstacle, lines, alreadyVisited, startPosDir))
            {
                answer++;
            }
            checkedPositions.Add(newObstacle);
        }
        return answer;
    }

    private static bool CreatesLoop(HashSet<(int, int)> obstacles, (int, int) newObstacle, List<string> lines, HashSet<((int, int), (int, int))> visitedHashSet, ((int, int), (int, int)) value)
    {
        var dir = value.Item2;
        var startPos = value.Item1;
        var visited = new HashSet<((int, int), (int, int))>(visitedHashSet);
        var newObstacles = new HashSet<(int, int)>(obstacles)
        {
            newObstacle
        };
        var iter = 0;
        while (true)
        {
            iter++;
            var newPos = (startPos.Item1 + dir.Item1, startPos.Item2 + dir.Item2);
            if (newObstacles.Contains(newPos))
            {
                dir = (dir.Item2, -dir.Item1);
            }
            else
            {
                startPos = newPos;
            }
            if (!Inbounds(startPos, lines)) return false;
            if (visited.Contains((startPos, dir)))
            {
                return true;
            }
            visited.Add((startPos, dir));
        }
    }
}