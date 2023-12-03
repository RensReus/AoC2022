
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace AoC2023;

class Day03 : BaseDay
{

    [Example(expected: 4361, input: "467..114..\n...*......\n..35..633.\n......#...\n617*......\n.....+.58.\n..592.....\n......755.\n...$.*....\n.664.598..")]
    [Puzzle(expected: 539590)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);
        var num = "";
        var output = 0;
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                var curChar = lines[i][j];
                if (char.IsNumber(curChar))
                {
                    num += curChar;
                }
                else if (!string.IsNullOrEmpty(num))
                {
                    output += PartValue(lines, i, j, num);
                    num = "";
                }
            }
            if (!string.IsNullOrEmpty(num))
            {
                output += PartValue(lines, i, lines[i].Length, num);
                num = "";
            }
        }
        return output;
    }

    private static int PartValue(IList<string> lines, int i, int j, string num)
    {
        var left = int.Max(j - 1 - num.Length, 0);
        var right = int.Min(j, lines[i].Length - 1);
        var top = int.Max(i - 1, 0);
        var bottom = int.Min(i + 1, lines.Count - 1);

        for (int x = left; x <= right; x++)
        {
            for (int y = top; y <= bottom; y++)
            {
                if (lines[y][x] != '.' && !char.IsNumber(lines[y][x]))
                    return int.Parse(num);
            }
        }
        return 0;
    }

    [Example(expected: 467835, input: "467..114..\n...*......\n..35..633.\n......#...\n617*......\n.....+.58.\n..592.....\n......755.\n...$.*....\n.664.598..")]
    [Puzzle(expected: 222222)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input);
        var num = "";
        var output = 0;
        var possibleGears = new List<(int, int, int)>();
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                var curChar = lines[i][j];
                if (char.IsNumber(curChar))
                {
                    num += curChar;
                }
                else if (!string.IsNullOrEmpty(num))
                {
                    var possibleGear = PossibleGear(lines, i, j, num);
                    if (possibleGear.Item1 != 0) possibleGears.Add(possibleGear);
                    num = "";
                }
            }
            if (!string.IsNullOrEmpty(num))
            {
                var possibleGear = PossibleGear(lines, i, lines[i].Length, num);
                if (possibleGear.Item1 != 0) possibleGears.Add(possibleGear);
                num = "";
            }
        }
        for (int i = 0; i < possibleGears.Count; i++)
        {
            var currGear = possibleGears[i];
            var sharedGears = new int[] { currGear.Item1 };
            for (int j = i + 1; j < possibleGears.Count; j++)
            {
                var possiblePartner = possibleGears[j];
                if (currGear.Item2 == possiblePartner.Item2 && currGear.Item3 == possiblePartner.Item3) sharedGears.Append(possiblePartner.Item1);
            }
            if (sharedGears.Count() == 2)
                output += sharedGears[0] * sharedGears[1];
        }
        return output;
    }

    private static (int, int, int) PossibleGear(IList<string> lines, int i, int j, string num)
    {
        var left = int.Max(j - 1 - num.Length, 0);
        var right = int.Min(j, lines[i].Length - 1);
        var top = int.Max(i - 1, 0);
        var bottom = int.Min(i + 1, lines.Count - 1);

        for (int x = left; x <= right; x++)
        {
            for (int y = top; y <= bottom; y++)
            {
                if (lines[y][x] == '*')
                    return (int.Parse(num), x, y);
            }
        }
        return (0, 0, 0);
    }
}