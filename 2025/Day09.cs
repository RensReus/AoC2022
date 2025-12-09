namespace AoC2025;

class Day09 : BaseDay
{
    [Example(expected: 50, input: "7,1\n11,1\n11,7\n9,7\n9,5\n2,5\n2,3\n7,3")]
    [Puzzle(expected: 4763509452)]
    public static long Part1(string input)
    {
        var coords = ReadLines(input).Select(line => (line.Split(',').Select(long.Parse).First(), line.Split(',').Select(long.Parse).ToList()[1])).ToList();
        var maxArea = 0L;
        for (int i = 0; i < coords.Count; i++)
        {
            for (int j = i + 1; j < coords.Count; j++)
            {
                var area = (Math.Abs(coords[i].Item1 - coords[j].Item1) + 1) * (Math.Abs(coords[i].Item2 - coords[j].Item2) + 1);
                if (area > maxArea)
                {
                    maxArea = area;
                }
            }

        }
        return maxArea;
    }

    [Example(expected: 24, input: "7,1\n11,1\n11,7\n9,7\n9,5\n2,5\n2,3\n7,3")]
    [Puzzle(expected: 1516897893)]
    public static long Part2(string input)
    {
        var coords = ReadLines(input).Select(line => (line.Split(',').Select(long.Parse).First(), line.Split(',').Select(long.Parse).ToList()[1])).ToList();

        var verticals = new Dictionary<long, (long, long)>(); // col, row1, row2

        for (int i = 0; i < coords.Count - 1; i++)
        {
            if (coords[i].Item1 == coords[i + 1].Item1)
            {
                // vertical line
                var col = coords[i].Item1;
                var row1 = Math.Min(coords[i].Item2, coords[i + 1].Item2);
                var row2 = Math.Max(coords[i].Item2, coords[i + 1].Item2);
                verticals[col] = (row1, row2);
            }
        }

        if (coords[0].Item1 == coords[^1].Item1)
        {
            // vertical line
            var col = coords[0].Item1;
            var row1 = Math.Min(coords[0].Item2, coords[^1].Item2);
            var row2 = Math.Max(coords[0].Item2, coords[^1].Item2);
            verticals[col] = (row1, row2);
        }

        var allCol = coords.Select(c => c.Item1).Distinct().Order().ToList();
        var allRow = coords.Select(c => c.Item2).Distinct().Order().ToList();

        var outsidePoints = new Dictionary<long, List<long>>();

        // effectively checks 1 point inside each subsquare formed by all possible row and col values
        foreach (var row in allRow[..^1])
        {
            var outside = true;
            var rowToCheck = row + 1;
            var outsideList = new List<long>();
            foreach (var col in allCol)
            {
                var line = verticals[col];
                if (rowToCheck >= line.Item1 && rowToCheck <= line.Item2)
                {
                    outside = !outside;
                }
                if (outside)
                {
                    outsideList.Add(col + 1);
                }
            }
            outsidePoints[rowToCheck] = outsideList;
        }

        var maxArea = 0L;
        for (int i = 0; i < coords.Count; i++)
        {
            for (int j = i + 1; j < coords.Count; j++)
            {
                var area = (Math.Abs(coords[i].Item1 - coords[j].Item1) + 1) * (Math.Abs(coords[i].Item2 - coords[j].Item2) + 1);
                if (area > maxArea)
                {
                    var coord1 = coords[i];
                    var coord2 = coords[j];
                    if (ValidArea(coord1, coord2, outsidePoints, allRow))
                    {
                        maxArea = area;
                    }
                }
            }
        }
        return maxArea;
    }

    private static bool ValidArea((long, long) value1, (long, long) value2, Dictionary<long, List<long>> outsidePoints, List<long> allRow)
    {
        var row1 = Math.Min(value1.Item2, value2.Item2);
        var row2 = Math.Max(value1.Item2, value2.Item2);
        var col1 = Math.Min(value1.Item1, value2.Item1);
        var col2 = Math.Max(value1.Item1, value2.Item1);


        foreach (var row in allRow)
        {
            if (row + 1 < row1 || row + 1 > row2)
            {
                continue;
            }
            var outsideList = outsidePoints.GetValueOrDefault(row + 1);
            if (outsideList != null && outsideList.Any(col => col >= col1 && col <= col2))
            {
                return false;
            }
        }
        return true;
    }
}