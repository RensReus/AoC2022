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
    [Puzzle(expected: 127844509405501)]
    public static long Part2(string input)
    {
        var steps = ReadLines(input).Select(ConvertInstruction);
        var pos = (0L, 0L);
        var originalCorners = new List<(long, long)> { pos };
        foreach (var step in steps)
        {
            var dir = GetDir(step[0], long.Parse(step[1]));
            pos = (pos.Item1 + dir.Item1, pos.Item2 + dir.Item2);
            originalCorners.Add(pos);
        }
        var corners = new Dictionary<(long, long), bool>();

        var allRow = originalCorners.Select(x => x.Item1).Distinct().Order().ToList();
        var allCol = originalCorners.Select(x => x.Item2).Distinct().Order().ToList();

        var verticalLines = new List<((long, long), long)>();
        for (int i = 0; i < originalCorners.Count - 1; i++)
        {
            var a = originalCorners[i];
            var b = originalCorners[i + 1];
            var rowRange = (long.Min(a.Item1, b.Item1), long.Max(a.Item1, b.Item1));
            if (b.Item2 == a.Item2) verticalLines.Add((rowRange, a.Item2));
        }
        var a2 = originalCorners[0];
        var b2 = originalCorners[^1];
        var rowRange2 = (long.Min(a2.Item1, b2.Item1), long.Max(a2.Item1, b2.Item1));
        if (b2.Item2 == a2.Item2) verticalLines.Add((rowRange2, a2.Item2));
        verticalLines = verticalLines.OrderBy(x => x.Item2).ToList();
        var totalSize = 0L;
        for (int i = 0; i < allRow.Count - 1; i++)
        {
            var row = allRow[i];
            var rowDiff = allRow[i + 1] - row;
            for (int j = 0; j < allCol.Count - 1; j++)
            {
                var (col1, col2) = (allCol[j], allCol[j + 1]);

                if (InsideLoop(row + 1L, col1 + 1L, allCol, verticalLines)) totalSize += (col2 - col1) * rowDiff;
            }
        }

        return totalSize + RightDownRemainder(steps) + 1;
    }

    private static bool InsideLoop(long row, long col, List<long> allCol, IEnumerable<((long, long), long)> verticalLines)
    {
        var inside = false;
        foreach (var col2 in allCol.Where(c => c < col))
        {
            if (OnLoop(row, col2, verticalLines)) inside = !inside;
        }
        return inside;
    }

    private static bool OnLoop(long row, long col, IEnumerable<((long, long), long)> verticalLines)
    {
        foreach (var line in verticalLines)
        {
            if (line.Item2 == col && row > line.Item1.Item1 && row < line.Item1.Item2) return true;
        }
        return false;
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