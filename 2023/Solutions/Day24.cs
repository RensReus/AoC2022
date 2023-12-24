


namespace AoC2023;

class Day24 : BaseDay
{
    [Puzzle(expected: 25810)]
    public static int Part1(string input)
    {
        var hailstones = ReadLines(input).Select(x => new Hailstone1(x)).ToList();
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

    private static (decimal, decimal, bool) Intersection(Hailstone1 one, Hailstone1 two)
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
        var hailstones = ReadLines(input).Select(x => new Hailstone1(x)).ToList();
        var dxMax = long.MaxValue;
        var dyMax = long.MaxValue;
        var dzMax = long.MaxValue;
        var dxMin = long.MinValue;
        var dyMin = long.MinValue;
        var dzMin = long.MinValue;
        var dxOverlaps = new Dictionary<long, List<Hailstone1>>();
        for (int i = 0; i < hailstones.Count; i++)
        {
            for (int j = i + 1; j < hailstones.Count; j++)
            {
                if (hailstones[i].Dx == hailstones[j].Dx)
                {
                    long newMax = (long)(Math.Abs(hailstones[i].Px - hailstones[j].Px) + Math.Abs(hailstones[j].Dx));
                    dxMax = long.Min(dxMax, newMax);
                    long newMin = (long)Math.Abs(hailstones[j].Dx);
                    dxMin = long.Max(dxMin, newMin);
                    Console.WriteLine($"x={hailstones[i].Dx}t+{hailstones[i].Px}");
                    Console.WriteLine($"x={hailstones[j].Dx}t+{hailstones[j].Px}");
                }
                if (hailstones[i].Dy == hailstones[j].Dy)
                {
                    long newMax = (long)(Math.Abs(hailstones[i].Py - hailstones[j].Py) + Math.Abs(hailstones[j].Dy));
                    dyMax = long.Min(dyMax, newMax);
                    long newMin = (long)Math.Abs(hailstones[j].Dy);
                    dyMin = long.Max(dyMin, newMin);
                    // Console.WriteLine($"dy {hailstones[i].Py - hailstones[j].Py}");
                }
                if (hailstones[i].Dz == hailstones[j].Dz)
                {
                    long newMax = (long)(Math.Abs(hailstones[i].Pz - hailstones[j].Pz) + Math.Abs(hailstones[j].Dz));
                    dzMax = long.Min(dzMax, newMax);
                    long newMin = (long)Math.Abs(hailstones[j].Dz);
                    dzMin = long.Max(dzMin, newMin);
                    // Console.WriteLine($"dz {hailstones[i].Pz - hailstones[j].Pz}");
                }
                // Console.WriteLine(Dpt(hailstones[i], hailstones[j]));
            }
        }
        Console.WriteLine(dxMax);
        Console.WriteLine(dyMax);
        Console.WriteLine(dzMax);
        // 
        // Console.WriteLine(dxMin); // misschien niet geldig
        // Console.WriteLine(dyMin);
        // Console.WriteLine(dzMin);
        return dxMax * 2 + dyMax * 2 + dzMax * 2;
    }

    private static List<decimal> Divisors(List<decimal> dxLimits)
    {
        var response = new List<decimal>();
        var limit = dxLimits.Max();
        for (int i = 1; i <= limit; i++)
        {
            if (dxLimits.All(x => x % i == 0)) response.Add(i);
        }
        return response;
    }

    private static string Dpt(Hailstone1 a, Hailstone1 b)
    {
        return $"({a.Px - b.Px}, {a.Py - b.Py}, {a.Pz - b.Pz}) + ({a.Dx - b.Dx}, {a.Dy - b.Dy}, {a.Dz - b.Dz})*t ";
    }

    private class Hailstone1
    {
        public decimal Px;
        public decimal Py;
        public decimal Pz;
        public decimal Dx;
        public decimal Dy;
        public decimal Dz;

        public Hailstone1(string line)
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

    private class Hailstone2
    {
        public long Px;
        public long Py;
        public long Pz;
        public long Dx;
        public long Dy;
        public long Dz;

        public Hailstone2(string line)
        {
            var p = line.Split(" @ ")[0].Split(", ");
            Px = long.Parse(p[0]);
            Py = long.Parse(p[1]);
            Pz = long.Parse(p[2]);
            var d = line.Split(" @ ")[1].Split(", ");
            Dx = long.Parse(d[0]);
            Dy = long.Parse(d[1]);
            Dz = long.Parse(d[2]);
        }

        public override string ToString()
        {
            return $"{Px}, {Py}, {Pz} @ {Dx}, {Dy}, {Dz}";
        }
    }
}