namespace AoC2023;

class Day18 : BaseDay
{
    [Example(expected: 62, input: "R 6 (#70c710)\nD 5 (#0dc571)\nL 2 (#5713f0)\nD 2 (#d2c081)\nR 2 (#59c680)\nD 2 (#411b91)\nL 5 (#8ceee2)\nU 2 (#caa173)\nL 1 (#1b58a2)\nU 2 (#caa171)\nR 2 (#7807d2)\nU 3 (#a77fa3)\nL 2 (#015232)\nU 2 (#7a21e3)")]
    [Puzzle(expected: 72821)]
    public static long Part1(string input)
    {
        var steps = ReadLines(input).Select(x => x.Split(" (")[0].Split(' '));
        return SolvePart1(steps);
    }

    private static long SolvePart1(IEnumerable<string[]> steps)
    {
        var pos = (0L, 0L);
        var loop = new Dictionary<(long, long), bool> { { pos, true } };
        var maxRow = 0L;
        var maxCol = 0L;
        var minRow = 0L;
        var minCol = 0L;
        foreach (var step in steps)
        {
            var dir = GetDir(step[0]);
            var stepCount = long.Parse(step[1]);
            for (long i = 0; i < stepCount; i++)
            {
                pos = (pos.Item1 + dir.Item1, pos.Item2 + dir.Item2);
                loop[pos] = true;
                maxRow = long.Max(pos.Item1, maxRow);
                maxCol = long.Max(pos.Item2, maxCol);
                minRow = long.Min(pos.Item1, minRow);
                minCol = long.Min(pos.Item2, minCol);
            }
        }
        var insideCount = 0;
        for (long i = minRow; i < maxRow; i++)
        {
            var delta = (1, 1);
            var diagpos = (i, minCol);
            var inside = false;
            while (diagpos.Item1 <= maxRow && diagpos.Item2 <= maxCol)
            {
                if (loop.ContainsKey(diagpos) && EdgeHasEffect(loop, diagpos)) inside = !inside;
                if (inside && !loop.ContainsKey(diagpos)) insideCount++;
                diagpos = (diagpos.Item1 + delta.Item1, diagpos.Item2 + delta.Item2);
            }
        }
        for (long i = minCol + 1; i < maxCol; i++)
        {
            var delta = (1, 1);
            var diagpos = (minRow, i);
            var inside = false;
            while (diagpos.Item1 <= maxRow && diagpos.Item2 <= maxCol)
            {
                if (loop.ContainsKey(diagpos) && EdgeHasEffect(loop, diagpos)) inside = !inside;
                if (inside && !loop.ContainsKey(diagpos)) insideCount++;
                diagpos = (diagpos.Item1 + delta.Item1, diagpos.Item2 + delta.Item2);
            }
        }
        return insideCount + loop.Count;
    }

    private static bool EdgeHasEffect(Dictionary<(long, long), bool> loop, (long, long) diagpos)
    {
        var neighbours = new List<(long, long)> { (1, 0), (-1, 0), (0, 1), (0, -1) }
            .Where(x => loop.ContainsKey((diagpos.Item1 + x.Item1, diagpos.Item2 + x.Item2))).ToList();
        return Math.Sign(neighbours[0].Item1 - neighbours[1].Item1) != Math.Sign(neighbours[0].Item2 - neighbours[1].Item2);
    }

    private static (long, long) GetDir(string v, long dist = 1)
        => v switch
        {
            "R" => (0, 1 * dist),
            "L" => (0, -1 * dist),
            "D" => (1 * dist, 0),
            _ => (-1 * dist, 0)
        };

    [Example(expected: 952408144115, input: "R 6 (#70c710)\nD 5 (#0dc571)\nL 2 (#5713f0)\nD 2 (#d2c081)\nR 2 (#59c680)\nD 2 (#411b91)\nL 5 (#8ceee2)\nU 2 (#caa173)\nL 1 (#1b58a2)\nU 2 (#caa171)\nR 2 (#7807d2)\nU 3 (#a77fa3)\nL 2 (#015232)\nU 2 (#7a21e3)")]
    [Example(expected: 179300, input: "R 155\nD 1000\nL 200\nU 540\nR 45\nU 460")]
    [Puzzle(expected: 119114864896236)] // Too low
    public static long Part2(string input)
    {
        var steps = ReadLines(input).Select(ConvertInstruction);
        //var steps = ReadLines(input).Select(x => x.Split(' '));
        var pos = (0L, 0L);
        var originalCorners = new Dictionary<(long, long), bool> { { pos, true } };
        foreach (var step in steps)
        {
            var dir = GetDir(step[0], long.Parse(step[1]));
            pos = (pos.Item1 + dir.Item1, pos.Item2 + dir.Item2);
            originalCorners[pos] = true;
        }
        var corners = new Dictionary<(long, long), bool>();

        var allRow = originalCorners.Keys.Select(x => x.Item1).Distinct().Order().ToList();
        var allCol = originalCorners.Keys.Select(x => x.Item2).Distinct().Order().ToList();
        foreach (var row in allRow)
        {
            foreach (var col in allCol)
            {
                corners[(row, col)] = true;
            }
        }
        var totalSize = 0L;
        foreach (var corner in corners.Keys)
        {
            var curRow = allRow.IndexOf(corner.Item1);
            var curCol = allCol.IndexOf(corner.Item2);
            var size = 0L;
            if (curRow == allRow.Count - 1 || curCol == allCol.Count - 1) continue;

            size = (allRow[curRow + 1] - corner.Item1) * (allCol[curCol + 1] - corner.Item2);
            if (InsideLoop(corner.Item1 + 208, corner.Item2 + 15, allRow, allCol, originalCorners)) totalSize += size;
        }

        return totalSize + RightDownRemainder(steps);
    }

    private static bool InsideLoop(long row, long col, List<long> allRow, List<long> allCol, Dictionary<(long, long), bool> originalCorners)
    {
        var inside = false;
        var rowIndex = allRow.FindIndex(0, x => x > row);
        var colIndex = allCol.FindIndex(0, x => x > col);
        while (row < allRow.Last() && col < allCol.Last())
        {
            var rowDelta = allRow[rowIndex] - row;
            var colDelta = allCol[colIndex] - col;
            if (rowDelta < colDelta)
            {
                row += rowDelta;
                col += rowDelta;
                rowIndex++;
                var corners = originalCorners.Keys.Where(x => x.Item1 == row).Select(x => x.Item2);
                var corners2 = originalCorners.Keys.Where(x => x.Item1 == row);
                if (corners.Count() % 2 != 0) throw new Exception("oneven matches");
                if (col > corners.Min() && col < corners.Max()) inside = !inside;
            }
            else if (rowDelta > colDelta)
            {
                row += colDelta;
                col += colDelta;
                colIndex++;
                var corners = originalCorners.Keys.Where(x => x.Item2 == col).Select(x => x.Item1);
                var corners2 = originalCorners.Keys.Where(x => x.Item2 == col);
                if (row > corners.Min() && row < corners.Max()) inside = !inside;
                if (corners.Count() % 2 != 0) throw new Exception("oneven matches");
            }
            else
            {
                if (colIndex == 0 || colIndex == allCol.Count - 1 || rowIndex == 0 || rowIndex == allRow.Count - 1)
                    throw new Exception("hopelijk komt dit niet voor");
                row += colDelta;
                col += colDelta;
                colIndex++;
                rowIndex++;
                var corners = originalCorners.Keys.Where(x => x.Item1 == row).Select(x => x.Item2);
                if (corners.Count() % 2 != 0) throw new Exception("oneven matches");
            }
        }
        return inside;
    }

    private static long RightDownRemainder(IEnumerable<string[]> steps)
        => steps.Where(step => "LD".Contains(step[0])).Sum(step => long.Parse(step[1]));

    private static string[] ConvertInstruction(string x)
    {
        var hex = x.Split('#')[1].Trim(')');
        var stepSize = Convert.ToInt64(hex[..5], 16);
        var step = GetStep(hex[5]);
        return [step, stepSize.ToString()];
    }

    private static string GetStep(char v)
        => v switch
        {
            '0' => "R",
            '1' => "D",
            '2' => "L",
            _ => "U",
        };
}