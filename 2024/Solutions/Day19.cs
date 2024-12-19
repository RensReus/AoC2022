namespace AoC2024;

class Day19 : BaseDay
{
    [Example(expected: 6, input: "r, wr, b, g, bwu, rb, gb, br\n\nbrwrr\nbggr\ngbbr\nrrbgbr\nubwu\nbwurrg\nbrgr\nbbrgwb")]
    [Puzzle(expected: 263)]
    public static int Part1(string input)
    {
        var lines = ReadLinesDouble(input);
        var parts = lines[0][0].Split(", ");
        var targets = lines[1];

        return targets.Count(x => IsBuildable(x, parts));
    }

    private static bool IsBuildable(string x, string[] parts)
    {
        foreach (var part in parts)
        {
            if (x.StartsWith(part))
            {
                var rest = x.Substring(part.Length);
                if (string.IsNullOrEmpty(rest) || IsBuildable(rest, parts))
                {
                    return true;
                };
            }
        }
        return false;
    }

    [Example(expected: 16, input: "r, wr, b, g, bwu, rb, gb, br\n\nbrwrr\nbggr\ngbbr\nrrbgbr\nubwu\nbwurrg\nbrgr\nbbrgwb")]
    [Puzzle(expected: 723524534506343)]
    public static long Part2(string input)
    {
        var lines = ReadLinesDouble(input);
        var parts = lines[0][0].Split(", ");
        var targets = lines[1];

        var cache = new Dictionary<string, long>();
        return targets.Sum(x => IsBuildableSum(x, parts, cache));
    }

    private static long IsBuildableSum(string x, string[] parts, Dictionary<string, long> cache)
    {
        if (cache.TryGetValue(x, out var cached))
        {
            return cached;
        }
        var ans = 0L;
        foreach (var part in parts)
        {
            if (x.StartsWith(part))
            {
                var rest = x.Substring(part.Length);
                if (string.IsNullOrEmpty(rest))
                {
                    ans++;
                }
                else
                {
                    ans += IsBuildableSum(rest, parts, cache);
                }
            }
        }
        cache[x] = ans;
        return ans;
    }
}