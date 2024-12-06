
using System.Formats.Asn1;

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
        var visited = new HashSet<(int, int)>() { pos };
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
            visited.Add(pos);
        }
        return visited.Count;
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
        var answer = 0;
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                if (lines[i][j] == '.')
                {
                    var newObstacles = new HashSet<(int, int)>(obstacles)
                    {
                        (i, j)
                    };
                    if (CreatesLoop(startPos, newObstacles, lines))
                    {
                        answer++;
                    }
                }
            }
        }
        return answer;
    }

    private static bool CreatesLoop((int, int) startPos, HashSet<(int, int)> obstacles, List<string> lines)
    {
        var dir = (-1, 0);
        var visited = new HashSet<((int, int), (int, int))>() { (startPos, dir) };
        while (true)
        {
            var newPos = (startPos.Item1 + dir.Item1, startPos.Item2 + dir.Item2);
            if (obstacles.Contains(newPos))
            {
                dir = (dir.Item2, -dir.Item1);
            }
            else
            {
                startPos = newPos;
            }
            if (!Inbounds(startPos, lines)) return false;
            if (visited.Contains((startPos, dir))) return true;
            visited.Add((startPos, dir));
        }
    }
}