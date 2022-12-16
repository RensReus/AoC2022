using System.Text.RegularExpressions;

namespace AoC2022;

static class Day15
{
    static IList<string> ProcessInput(string input)
    {
        return input.Split(";").ToList();
    }

    [Example(expected: 26, input: 1)]
    [Puzzle(expected: 4725496)]
    public static int Part1(string input)
    {
        var lineToCheck = 10;
        var lineToCheck2 = 2000000;
        var noBeacon = new HashSet<int>();
        var processedInput = ProcessInput(input);
        foreach (var line in processedInput)
        {
            var groups = Regex.Match(line, @"Sensor at x=(-*\d+), y=(-*\d+): closest beacon is at x=(-*\d+), y=(-*\d+)").Groups;
            var x = int.Parse(groups[1].Value);
            var y = int.Parse(groups[2].Value);
            var manhattan = int.Abs(x - int.Parse(groups[3].Value)) + int.Abs(y - int.Parse(groups[4].Value));
            var lineLow = (x - manhattan) + int.Abs(lineToCheck - y);
            var lineHigh = (x + manhattan) - int.Abs(lineToCheck - y);
            for (int i = lineLow; i < lineHigh; i++)
            {
                noBeacon.Add(i);
            }
        }
        return noBeacon.Count();
    }

    [Example(expected: 56000011, input: 1)]
    [Puzzle(expected: 12051287042458)]
    public static long Part2(string input)
    {
        //var limits = 20;
        var limits = 4000000;
        var noBeacons = ProcessInput(input).Select(x => new NoBeaconField(x));
        for (int i = 0; i <= limits; i++)
        {
            var notExcluded = new List<(int, int)> { (0, limits) };
            foreach (var noBeacon in noBeacons)
            {
                notExcluded = UpdateNotExcluded(notExcluded, noBeacon, limits, i);
                if (notExcluded.Count() == 0) break;
            }
            if (notExcluded.Count() > 0)
            {
                return (long)i + (long)notExcluded[0].Item1 * 4000000;
            }
        }
        return 0;
    }

    private static List<(int, int)> UpdateNotExcluded(List<(int, int)> notExcluded, NoBeaconField noBeacon, int limits, int lineToCheck)
    {
        var newNotExcluded = new List<(int, int)> { };
        var lineLow = (noBeacon.Center.X - noBeacon.Manhattan) + int.Abs(lineToCheck - noBeacon.Center.Y);
        var lineHigh = (noBeacon.Center.X + noBeacon.Manhattan) - int.Abs(lineToCheck - noBeacon.Center.Y);
        if (lineLow >= lineHigh) return notExcluded;
        foreach (var segment in notExcluded)
        {
            var segLow = segment.Item1;
            var segHigh = segment.Item2;
            if (lineHigh < segLow || lineLow > segHigh) newNotExcluded.Add(segment);
            else if (lineLow <= segLow && lineHigh < segHigh) newNotExcluded.Add((lineHigh + 1, segHigh));
            else if (lineLow > segLow && lineHigh >= segHigh) newNotExcluded.Add((segLow, lineLow - 1));
            else if (lineLow > segLow && lineHigh < segHigh)
            {
                newNotExcluded.Add((segLow, lineLow - 1));
                newNotExcluded.Add((lineHigh + 1, segHigh));
            }
        }
        return newNotExcluded;
    }
}

public class NoBeaconField
{
    public IList<Corner> Corners = new List<Corner>();
    public Corner Center;
    public int X;
    public int Y;
    public int DeltaX;
    public int DeltaY;
    public int Manhattan;
    public NoBeaconField(string line)
    {
        var groups = Regex.Match(line, @"Sensor at x=(-*\d+), y=(-*\d+): closest beacon is at x=(-*\d+), y=(-*\d+)").Groups;
        var x = int.Parse(groups[1].Value);
        var y = int.Parse(groups[2].Value);
        Center = new(x, y);
        Manhattan = int.Abs(x - int.Parse(groups[3].Value)) + int.Abs(y - int.Parse(groups[4].Value));
        Corners.Add(new(x + Manhattan, y));
        Corners.Add(new(x - Manhattan, y));
        Corners.Add(new(x, y + Manhattan));
        Corners.Add(new(x, y - Manhattan));
        X = X - Manhattan;
        Y = y;
        DeltaX = Manhattan * 2;
        DeltaY = 0;
    }
}

public class Corner
{
    public int X;
    public int Y;
    public Corner(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }
    public override bool Equals(object? other)
    => other is Corner c && X == c.X && Y == c.Y;

    public override int GetHashCode()
        => HashCode.Combine(X, Y);
}