

using NUnit.Framework.Internal.Commands;

namespace AoC2023;

class Day14 : BaseDay
{
    [Example(expected: 136, input: "OOOO.#.O..\nOO..#....#\nOO..O##..O\nO..#.OO...\n........#.\n..#....#.#\n..O..#.O.O\n..O.......\n#....###..\n#....#....")]
    [Puzzle(expected: 109665)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);
        var height = lines.Count;
        var answer = 0;
        for (int col = 0; col < lines[0].Length; col++)
        {
            var occupied = 0;
            for (int row = 0; row < height; row++)
            {
                if (lines[row][col] == 'O')
                {
                    answer += height - occupied;
                    occupied++;
                }
                if (lines[row][col] == '#')
                {
                    occupied = row + 1;
                }
            }
        }
        return answer;
    }

    [Example(expected: 64, input: "O....#....\nO.OO#....#\n.....##...\nOO.#O....O\n.O.....O#.\nO.#..O.#.#\n..O..#O..O\n.......O..\n#....###..\n#OO..#....")]
    [Puzzle(expected: 96061)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input).Select(x => x.ToCharArray()).ToList();
        var previousStates = new Dictionary<string, int>();
        for (int i = 0; i < 1000000000; i++)
        {
            lines = MoveNorth(lines);
            (previousStates, i) = UpdatePrevOrSkip(lines, i, previousStates, 'N');
            lines = MoveWest(lines);
            (previousStates, i) = UpdatePrevOrSkip(lines, i, previousStates, 'W');
            lines = MoveSouth(lines);
            (previousStates, i) = UpdatePrevOrSkip(lines, i, previousStates, 'S');
            lines = MoveEast(lines);
            (previousStates, i) = UpdatePrevOrSkip(lines, i, previousStates, 'E');
        }

        return CalculateAnswer(lines);
    }

    private static int CalculateAnswer(List<char[]> lines)
    {
        var height = lines.Count;
        var answer = 0;
        for (int col = 0; col < lines[0].Length; col++)
        {
            for (int row = 0; row < height; row++)
            {
                if (lines[row][col] == 'O')
                {
                    answer += height - row;
                }
            }
        }
        return answer;
    }

    private static List<char[]> MoveWest(List<char[]> lines)
    {
        for (int row = 0; row < lines.Count; row++)
        {
            var occupied = 0;
            for (int col = 0; col < lines[0].Length; col++)
            {
                if (lines[row][col] == 'O')
                {
                    lines[row][col] = '.';
                    lines[row][occupied] = 'O';
                    occupied++;
                }
                if (lines[row][col] == '#')
                {
                    occupied = col + 1;
                }
            }
        }
        return lines;
    }

    private static List<char[]> MoveEast(List<char[]> lines)
    {
        for (int row = 0; row < lines.Count; row++)
        {
            var occupied = lines[0].Length - 1;
            for (int col = lines[0].Length - 1; col >= 0; col--)
            {
                if (lines[row][col] == 'O')
                {
                    lines[row][col] = '.';
                    lines[row][occupied] = 'O';
                    occupied--;
                }
                if (lines[row][col] == '#')
                {
                    occupied = col - 1;
                }
            }
        }
        return lines;
    }

    private static (Dictionary<string, int> previousStates, int i) UpdatePrevOrSkip(List<char[]> lines, int i, Dictionary<string, int> previousStates, char dir)
    {
        var key = string.Concat(lines.Select(l => new string(l))) + dir;
        if (previousStates.TryGetValue(key, out int loopStart))
        {
            var loopSize = i - loopStart;
            int loopsToSkip = (1000000000 - i - 1) / loopSize;
            i += loopsToSkip * loopSize;
        }
        else
        {
            previousStates[key] = i;
        }
        return (previousStates, i);
    }

    private static List<char[]> MoveNorth(List<char[]> lines)
    {
        for (int col = 0; col < lines[0].Length; col++)
        {
            var occupied = 0;
            for (int row = 0; row < lines.Count; row++)
            {
                if (lines[row][col] == 'O')
                {
                    lines[row][col] = '.';
                    lines[occupied][col] = 'O';
                    occupied++;
                }
                if (lines[row][col] == '#')
                {
                    occupied = row + 1;
                }
            }
        }
        return lines;
    }

    private static List<char[]> MoveSouth(List<char[]> lines)
    {
        for (int col = 0; col < lines[0].Length; col++)
        {
            var occupied = lines.Count - 1;
            for (int row = lines.Count - 1; row >= 0; row--)
            {
                if (lines[row][col] == 'O')
                {
                    lines[row][col] = '.';
                    lines[occupied][col] = 'O';
                    occupied--;
                }
                if (lines[row][col] == '#')
                {
                    occupied = row - 1;
                }
            }
        }
        return lines;
    }
}