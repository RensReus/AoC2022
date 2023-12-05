






namespace AoC2023;

partial class Day05 : BaseDay
{
    [Example(expected: 35, input: 1)]
    [Puzzle(expected: 251346198)]
    public static long Part1(string input)
    {
        var blocks = input.Split("\n\n");
        var seeds = NumberRegex().Matches(blocks[0]).Select(x => long.Parse(x.Value));
        var maps = blocks[1..];
        return seeds.Min(seed => MapSeed(seed, maps));
    }

    private static long MapSeed(long seed, string[] maps)
    {
        foreach (var map in maps)
        {
            seed = ApplyMap(map, seed);
        }
        return seed;
    }

    private static long ApplyMap(string map, long seed)
    {
        foreach (var rule in map.Split("\n")[1..])
        {
            var ruleValues = NumberRegex().Matches(rule).Select(x => long.Parse(x.Value)).ToList();
            var dest = ruleValues[0];
            var source = ruleValues[1];
            var range = ruleValues[2];
            if (seed >= source && seed < source + range) return dest + (seed - source);
        }
        return seed;
    }

    [Example(expected: 46, input: 1)]
    [Puzzle(expected: 72263011)]
    public static long Part2(string input)
    {
        var blocks = input.Split("\n\n");
        var seedRanges = GetSeedRanges(blocks[0]);
        var maps = blocks[1..];
        return MapSeedRanges(seedRanges, maps);
    }

    private static long MapSeedRanges(IEnumerable<Range> rangesToMap, string[] maps)
    {
        foreach (var map in maps)
        {
            var nextRanges = new List<Range>();
            foreach (var range in rangesToMap)
            {
                nextRanges.AddRange(MapSeedRangeFullMap(range, map));
            }
            rangesToMap = nextRanges;
            var a = rangesToMap.Where(x => x.start < 5);
        }
        return rangesToMap.Min(x => x.start);
    }

    private static IEnumerable<Range> MapSeedRangeFullMap(Range inputRange, string map)
    {
        var mappedRanges = new List<Range>();
        var rangesToMap = new List<Range> { inputRange };
        foreach (var rule in map.Split("\n")[1..])
        {
            var nextRanges = new List<Range>();
            foreach (var range in rangesToMap)
            {
                var (left, overlap, right) = MapSeedRange(range, rule);
                if (overlap.length > 0) mappedRanges.Add(overlap);
                if (left.length > 0) nextRanges.Add(left);
                if (right.length > 0) nextRanges.Add(right);
            }
            rangesToMap = nextRanges;
        }
        mappedRanges.AddRange(rangesToMap);
        var a = mappedRanges.Where(x => x.start < 5);
        return mappedRanges;
    }

    private static (Range, Range, Range) MapSeedRange(Range range, string rule)
    {
        var ruleValues = NumberRegex().Matches(rule).Select(x => long.Parse(x.Value)).ToList();
        var mapStart = ruleValues[1];
        var mapLength = ruleValues[2];
        var mapEnd = mapStart + mapLength - 1;
        var rangeEnd = range.start + range.length - 1;
        var leftSide = new Range(range.start, long.Min(mapStart - range.start, range.length));
        var rightStart = long.Max(mapEnd + 1, range.start);
        var rightSide = new Range(rightStart, rangeEnd - (rightStart));
        var noOverlap = rangeEnd < mapStart || range.start > mapEnd;
        if (noOverlap) return (leftSide, new(0, 0), rightSide);
        var startOverlap = leftSide.length > 0 ? mapStart : range.start;
        var endOverlap = rightSide.length > 0 ? mapEnd : rangeEnd;
        var lengthOverlap = endOverlap - startOverlap + 1;
        var startMapped = startOverlap - mapStart + ruleValues[0];
        return (leftSide, new(startMapped, lengthOverlap), rightSide);
    }

    private static IEnumerable<Range> GetSeedRanges(string v)
    {
        var pairs = DoubleNumberRegex().Matches(v).Select(x => x.Value);
        return pairs.Select(x => new Range(long.Parse(x.Split()[0]), long.Parse(x.Split()[1])));
    }

    [GeneratedRegex(@"(\d+)")]
    private static partial Regex NumberRegex();

    [GeneratedRegex(@"(\d+ \d+)")]
    private static partial Regex DoubleNumberRegex();

    private record Range(long start, long length);
}