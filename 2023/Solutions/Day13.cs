

namespace AoC2023;

class Day13 : BaseDay
{
    [Example(expected: 405, input: "#.##..##.\n..#.##.#.\n##......#\n##......#\n..#.##.#.\n..##..##.\n#.#.##.#.\n\n#...##..#\n#....#..#\n..##..###\n#####.##.\n#####.##.\n..##..###\n#....#..#")]
    [Puzzle(expected: 28651)] // 16117, 17509 too low, 30691 too high
    public static int Part1(string input)
    {
        var blocks = input.Split("\n\n");
        var answer = 0;
        foreach (var block in blocks)
        {
            var lines = ReadLines(block);
            for (int i = 1; i < lines.Count; i++)
            {
                if (MirrorLine(lines, i))
                {
                    answer += i * 100;
                }
            }

            var verticalLines = new List<string>();
            for (int i = 0; i < lines[0].Length; i++)
            {
                var newline = string.Concat(lines.Select(line => line[i]));
                verticalLines.Add(newline);
            }

            for (int i = 1; i < verticalLines.Count; i++)
            {
                if (MirrorLine(verticalLines, i))
                {
                    answer += i;
                }
            }
        }
        return answer;
    }

    private static bool MirrorLine(List<string> lines, int line)
    {
        var x = int.Min(lines.Count - line, line);
        for (int i = 0; i < x; i++)
        {
            if (lines[line - i - 1] != lines[line + i]) return false;
        }
        return true;
    }

    private static bool MirrorLineSmudge(List<string> lines, int line)
    {
        var x = int.Min(lines.Count - line, line);
        var smudges = 0;
        for (int i = 0; i < x; i++)
        {
            smudges += CountDiff(lines[line - i - 1], lines[line + i]);
        }
        return smudges == 1;
    }

    private static int CountDiff(string v1, string v2)
    {
        var diff = 0;
        for (int i = 0; i < v1.Length; i++)
        {
            if (v1[i] != v2[i])
            {
                diff++;
            }
        }
        return diff;
    }

    [Example(expected: 400, input: "#.##..##.\n..#.##.#.\n##......#\n##......#\n..#.##.#.\n..##..##.\n#.#.##.#.\n\n#...##..#\n#....#..#\n..##..###\n#####.##.\n#####.##.\n..##..###\n#....#..#")]
    [Puzzle(expected: 25450)]
    public static int Part2(string input)
    {
        var blocks = input.Split("\n\n");
        var answer = 0;
        foreach (var block in blocks)
        {
            var lines = ReadLines(block);
            for (int i = 1; i < lines.Count; i++)
            {
                if (MirrorLineSmudge(lines, i))
                {
                    answer += i * 100;
                }
            }
            var verticalLines = new List<string>();
            for (int i = 0; i < lines[0].Length; i++)
            {
                var newline = string.Concat(lines.Select(line => line[i]));
                verticalLines.Add(newline);
            }

            for (int i = 1; i < verticalLines.Count; i++)
            {
                if (MirrorLineSmudge(verticalLines, i))
                {
                    answer += i;
                    Console.WriteLine("vert " + i);
                }
            }
        }
        return answer;
    }
}