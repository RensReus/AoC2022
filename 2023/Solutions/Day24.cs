


namespace AoC2023;

class Day24 : BaseDay
{
    [Puzzle(expected: 25810)]
    public static int Part1(string input)
    {
        var hailstones = ReadLines(input).Select(x => new Hailstone(x)).ToList();
        var answer = 0;
        for (int i = 0; i < hailstones.Count; i++)
        {
            for (int j = i + 1; j < hailstones.Count; j++)
            {
                if (Intersection(hailstones[i], hailstones[j]).Item3) answer++;
            }
        }
        return answer;
    }

    private static (decimal, decimal, bool) Intersection(Hailstone one, Hailstone two)
    {
        //long minArea = 7;
        //long maxArea = 27;
        long minArea = 200000000000000;
        long maxArea = 400000000000000;
        var a1 = one.Dy / one.Dx;
        var a2 = two.Dy / two.Dx;
        if (a1 == a2) return (0, 0, false);
        var xIntersect = (one.Py - a1 * one.Px - two.Py + a2 * two.Px) / (a2 - a1);
        var yIntersect = one.Py - a1 * one.Px + a1 * xIntersect;
        var inFuture = one.InFuture(xIntersect, yIntersect) && two.InFuture(xIntersect, yIntersect);
        var inArea = xIntersect >= minArea && xIntersect <= maxArea && yIntersect >= minArea && yIntersect <= maxArea;
        return (xIntersect, yIntersect, inFuture && inArea);
    }

    [Example(expected: 2, input: "19, 13, 30 @ -2,  1, -2\n18, 19, 22 @ -1, -1, -2\n20, 25, 34 @ -2, -2, -4\n12, 31, 28 @ -1, -2, -1\n20, 19, 15 @  1, -5, -3")]
    [Puzzle(expected: 222222)]
    public static long Part2(string input)
    {
        var hailstones = ReadLines(input).Select(x => new Hailstone(x)).ToList();
        var lowlim = long.MinValue;
        var uplim = long.MaxValue;
        var validRanges = new List<(long, long)>();
        for (int i = 0; i < hailstones.Count; i++)
        {
            for (int j = i + 1; j < hailstones.Count; j++)
            {
                if (hailstones[i].Dx == hailstones[j].Dx)
                {
                    var dx = (long)hailstones[j].Dx;
                    long dp = (long)Math.Abs(hailstones[i].Px - hailstones[j].Px);
                    validRanges.Add((dp, dx));
                    lowlim = long.Max(lowlim, -dp + dx);
                    uplim = long.Min(uplim, dp + dx);
                }
            }
        }
        validRanges = validRanges.OrderBy(x => x.Item2).ToList();

        foreach (var range in validRanges[0..1])
        {
            for (long i = range.Item2 + range.Item1; i >= range.Item2 - range.Item1; i += range.Item2)
            {
                var a = 1;
                // Console.Write(i + ", ");
            }
            // Console.WriteLine();
        }
        var combinedRange = validRanges[0];
        for (int i = 1; i < validRanges.Count; i++)
        {

        }

        return 0;
    }

    private class Hailstone
    {
        public decimal Px;
        public decimal Py;
        public decimal Pz;
        public decimal Dx;
        public decimal Dy;
        public decimal Dz;

        public Hailstone(string line)
        {
            var p = line.Split(" @ ")[0].Split(", ");
            Px = decimal.Parse(p[0]);
            Py = decimal.Parse(p[1]);
            Pz = decimal.Parse(p[2]);
            var d = line.Split(" @ ")[1].Split(", ");
            Dx = decimal.Parse(d[0]);
            Dy = decimal.Parse(d[1]);
            Dz = decimal.Parse(d[2]);
        }

        internal bool InFuture(decimal xIntersect, decimal yIntersect)
        {
            var xFuture = Dx > 0 ? xIntersect >= Px : xIntersect <= Px;
            var yFuture = Dy > 0 ? yIntersect >= Py : yIntersect <= Py;
            return xFuture && yFuture;
        }

        public override string ToString()
        {
            return $"{Px}, {Py}, {Pz} @ {Dx}, {Dy}, {Dz}";
        }
    }
}