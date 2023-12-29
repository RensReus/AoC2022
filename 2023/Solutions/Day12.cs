
namespace AoC2023;

class Day12 : BaseDay
{
    private readonly Dictionary<string, long> Cache = [];

    [Example(expected: 21, input: "???.### 1,1,3\n.??..??...?##. 1,1,3\n?#?#?#?#?#?#?#? 1,3,1,6\n????.#...#... 4,1,1\n????.######..#####. 1,6,5\n?###???????? 3,2,1")]
    [Puzzle(expected: 7541)]
    public long Part1(string input)
    {
        var answer = 0L;
        var items = ReadLines(input).Select(x => ParseInput(x, false)).ToList();

        foreach (var item in items)
        {
            var count = BruteForce(item.Item1, item.Item2);
            Console.WriteLine(count + " " + item.Item1);
            answer += count;
        }
        return answer;
    }

    [Example(expected: 525152, input: "???.### 1,1,3\n.??..??...?##. 1,1,3\n?#?#?#?#?#?#?#? 1,3,1,6\n????.#...#... 4,1,1\n????.######..#####. 1,6,5\n?###???????? 3,2,1")]
    [Puzzle(expected: 17485169859432)]
    public long Part2(string input)
    {
        var answer = 0L;
        var items = ReadLines(input).Select(x => ParseInput(x, true)).ToList();

        foreach (var item in items)
        {
            var count = BruteForce(item.Item1, item.Item2);
            Console.WriteLine(count + " " + item.Item1);
            answer += count;
        }
        return answer;
    }

    private long BruteForce(string sections, List<int> groups)
    {
        var flex = Flexibility(sections, groups);
        if (Cache.TryGetValue(CachKey(sections, groups), out long ans)) return ans;

        if (!sections.Contains('?') || groups.Count == 0) return 1;
        if (flex < 0 || (flex == 0 && NotPossible(sections, groups))) return 0;
        var group = groups[0];
        var totals = 0L;
        var totalsSets = new List<(int, long)>();
        for (int j = 0; j < flex + 1; j++)
        {
            if (j > 0 && '#' == sections[j - 1]) continue;
            if (j + group < sections.Length && '#' == sections[j + group]) continue;
            var upper = j + group;
            if (sections[j..upper].Any(x => x == '.')) continue;

            if (j > 0 && SimpleInvalid(sections[..(j - 1)], groups.Take(0))) continue;
            if (j + group < sections.Length && SimpleInvalid(sections[(upper + 1)..], groups.Skip(1))) continue;
            var bStart = j + group + 1;
            var secb = bStart >= sections.Length ? "" : sections[bStart..];
            var combinations = BruteForce(Reduce(secb), groups.Skip(1).ToList()); ;
            totalsSets.Add((j, combinations));
            totals += combinations;
        }
        Cache[CachKey(sections, groups)] = totals;
        return totals;
    }

    private static string CachKey(string sections, List<int> groups)
    => new string(sections) + " " + string.Join(",", groups);


    private static bool NotPossible(string sections, List<int> groups)
    {
        for (int i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            var pos = groups.Take(i).Sum() + i;
            if (pos > 0 && '#' == sections[pos - 1]) return true;
            if (pos + group < sections.Length && '#' == sections[pos + group]) return true;
            var upper = pos + group;
            if (sections[pos..upper].Any(x => x == '.')) return true;
        }
        return false;
    }

    private static bool SimpleInvalid(string sections, IEnumerable<int> groups)
    {
        if (!groups.Any()) return sections.Contains('#');
        if (HasTooManyLargeGroups(sections, groups)) return true;
        // trim . en check of lengte nog voldoende
        return false;
    }

    private static bool HasTooManyLargeGroups(string sections, IEnumerable<int> groups)
    {
        var largestGroup = groups.Max();
        var largestGroupCount = groups.Count(x => x == largestGroup);
        var pos = 0;
        var groupCount = 0;
        var hekjeCounter = 0;
        while (pos < sections.Length)
        {
            if (sections[pos] == '.' || sections[pos] == '?')
            {
                if (hekjeCounter > largestGroup) return true;
                if (hekjeCounter == largestGroup) groupCount++;
                hekjeCounter = 0;
            }
            if (sections[pos] == '#')
            {
                hekjeCounter++;
            }
            pos++;
        }
        if (hekjeCounter > largestGroup) return true;
        if (hekjeCounter == largestGroup) groupCount++;
        return groupCount > largestGroupCount;
    }

    private static (string, List<int>) ParseInput(string line, bool timesFive)
    {
        var (sections, groups) = (line.Split(" ")[0], line.Split(" ")[1]);
        if (timesFive)
        {
            var oldSections = sections;
            var oldGroups = groups;
            for (int i = 0; i < 4; i++)
            {
                sections += "?" + oldSections;
                groups += "," + oldGroups;
            }
        }
        return (Reduce(sections), groups.Split(",").Select(int.Parse).ToList());
    }

    private static int Flexibility(string sections, List<int> groups)
    => sections.Length - groups.Count - groups.Sum() + 1;

    public static string Reduce(string s)
    {
        s = s.Trim('.');
        while (s.Contains("..")) s = s.Replace("..", ".");
        return s;
    }
}