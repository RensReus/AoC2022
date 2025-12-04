
namespace AoC2025;

class Day04 : BaseDay
{
    [Example(expected: 13, input: "..@@.@@@@.\n@@@.@.@.@@\n@@@@@.@.@@\n@.@@@@..@.\n@@.@@@@.@@\n.@@@@@@@.@\n.@.@.@.@@@\n@.@@@.@@@@\n.@@@@@@@@.\n@.@.@@@.@.")]
    [Puzzle(expected: 1527)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input).Select(l => l.ToCharArray()).ToList();
        return AccessibleCount(lines);
    }

    private static int AccessibleCount(List<char[]> lines)
    {
        var ans = 0;
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (lines[i][j] == '@' && GetNeighbors(lines, i, j).Count(c => c == '@') < 4)
                {
                    ans++;
                }
            }
        }
        return ans;
    }

    private static List<char> GetNeighbors(List<char[]> lines, int i, int j)
    {
        var neighbors = new List<char>();
        var directions = new (int, int)[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };

        foreach (var (di, dj) in directions)
        {
            int ni = i + di;
            int nj = j + dj;
            if (ni >= 0 && ni < lines.Count && nj >= 0 && nj < lines[ni].Length)
            {
                neighbors.Add(lines[ni][nj]);
            }
        }

        return neighbors;
    }

    [Example(expected: 43, input: "..@@.@@@@.\n@@@.@.@.@@\n@@@@@.@.@@\n@.@@@@..@.\n@@.@@@@.@@\n.@@@@@@@.@\n.@.@.@.@@@\n@.@@@.@@@@\n.@@@@@@@@.\n@.@.@@@.@.")]
    [Puzzle(expected: 8690)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input).Select(line => line.ToCharArray()).ToList();
        var ans = 0;
        while (AccessibleCount(lines) > 0)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '@' && GetNeighbors(lines, i, j).Count(c => c == '@') < 4)
                    {
                        lines[i][j] = '.';
                        ans++;
                    }
                }
            }
        }
        return ans;
    }
}