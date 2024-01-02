


using System.Numerics;

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

    [Puzzle(expected: 652666650475950)]
    public static long Part2(string input)
    {
        var hailstones = ReadLines(input).Select(x => new Hailstone(x)).ToList();

        // finding possible speed values only returns a single value for each direction so no need to check combinations
        var dx = PossibleValues(hailstones.Select(h => (h.Px, h.Dx)))[0];
        var dy = PossibleValues(hailstones.Select(h => (h.Py, h.Dy)))[0];
        var dz = PossibleValues(hailstones.Select(h => (h.Pz, h.Dz)))[0];

        var (xMin, xMax) = FindMinMax(dx, hailstones.Select(h => (h.Px, h.Dx)));
        var (yMin, yMax) = FindMinMax(dy, hailstones.Select(h => (h.Py, h.Dy)));
        var (zMin, zMax) = FindMinMax(dz, hailstones.Select(h => (h.Pz, h.Dz)));

        var zStart = zMax;
        var hailstone = hailstones[0];
        // z min and max are equal in my input so collision time for all hailstones is known and xy start can simply be calculated
        var dt = (zStart - hailstone.Pz) / (hailstone.Dz - dz);
        var xStart = hailstone.Px + (hailstone.Dx - dx) * dt;
        var yStart = hailstone.Py + (hailstone.Dy - dy) * dt;
        return (long)(xStart + yStart + zStart);
    }

    private static (decimal, decimal) FindMinMax(decimal delta, IEnumerable<(decimal P, decimal V)> hailstones)
    {
        var min = decimal.MinValue;
        var max = decimal.MaxValue;
        foreach (var (P, V) in hailstones)
        {
            if (V - delta >= 1) min = decimal.Max(P, min);
            else if (V - delta <= -1) max = decimal.Min(P, max);
            else
            {
                min = decimal.Max(P, min);
                max = decimal.Min(P, max);
            }
        }
        return (min, max);
    }

    private static List<decimal> PossibleValues(IEnumerable<(decimal P, decimal V)> hailstones)
    {
        var parallels = new Dictionary<decimal, List<(decimal P, decimal V)>>();
        foreach (var hailstone in hailstones)
        {
            if (!parallels.ContainsKey(hailstone.V)) parallels[hailstone.V] = [];
            parallels[hailstone.V].Add(hailstone);
        }

        var possibleVs = new List<List<decimal>>();
        foreach (var group in parallels.Where(x => x.Value.Count == 3))
        {
            var possibleV = new List<decimal>();
            var ordered = group.Value.OrderByDescending(x => x.P).ToList();
            var dp1 = ordered[0].P - ordered[1].P;
            var dp2 = ordered[1].P - ordered[2].P;
            var dx = group.Key;
            var ratio = BigInteger.GreatestCommonDivisor((BigInteger)dp1, (BigInteger)dp2);
            var dt1Inc = dp1 / (decimal)ratio;
            for (decimal dt1 = dt1Inc; dt1 <= dp1; dt1 += dt1Inc)
            {
                if (dp1 % dt1 == 0)
                {
                    possibleV.Add(dp1 / dt1 + dx);
                    possibleV.Add(-dp1 / dt1 + dx);
                }
            }
            possibleVs.Add(possibleV);
        }
        var realllypossible = possibleVs[0];
        foreach (var item in possibleVs.Skip(1))
        {
            realllypossible = realllypossible.Intersect(item).ToList();
        }
        return realllypossible;
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