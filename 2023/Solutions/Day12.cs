using System.Text;

namespace AoC2023;

class Day12 : BaseDay
{
    private Dictionary<string, long> Cache;
    private Dictionary<(int, int), long> AllQuestionCache;

    public Day12()
    {
        Cache = [];
        AllQuestionCache = [];
    }

    [Example(expected: 525152, input: "???.### 1,1,3\n.??..??...?##. 1,1,3\n?#?#?#?#?#?#?#? 1,3,1,6\n????.#...#... 4,1,1\n????.######..#####. 1,6,5\n?###???????? 3,2,1")]
    [Example(expected: 1, input: "???.### 1,1,3")]
    [Example(expected: 16384, input: ".??..??...?##. 1,1,3")]
    [Example(expected: 1, input: "?#?#?#?#?#?#?#? 1,3,1,6")]
    [Example(expected: 2500, input: "????.######..#####. 1,6,5")]
    [Example(expected: 16, input: "????.#...#... 4,1,1")]
    [Example(expected: 506250, input: "?###???????? 3,2,1")]
    //[Puzzle(expected: 7541)]
    public long Part1(string input)
    {
        var prev = DateTime.Now;
        var firstStart = DateTime.Now;

        var answer = 0L;
        var output = new List<(int, long, string)>();
        var items = ReadLines(input).Select(x => ParseInput(x, true)).ToList();
        var now = DateTime.Now;
        Console.WriteLine("inlezen " + (now - prev).TotalMilliseconds);
        prev = now;
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            // log timing
            var start = DateTime.Now;
            var count = CalculatePermutations(item.Item1, item.Item2);
            var diff = (DateTime.Now - start).Seconds;
            output.Add((diff, count, CachKey(item.Item1, item.Item2)));
            answer += count;
            now = DateTime.Now;
            Console.WriteLine((now - prev).Seconds);
            prev = now;
        }

        foreach (var item in output.OrderByDescending(x => x.Item1))
        {
            Console.WriteLine(item);
            //Console.WriteLine(item.Item2 + " " + item.Item3);
        }
        Console.WriteLine($"total: {TotalCalls}, cachehits: {CacheHits}");

        Console.WriteLine("anser " + answer);
        now = DateTime.Now;
        Console.WriteLine("na loop " + (now - prev).TotalMilliseconds);
        prev = now;

        Console.WriteLine("Totaal " + (DateTime.Now - firstStart).TotalMilliseconds);

        return answer;
    }

    [Example(expected: 21, input: "???.### 1,1,3\n.??..??...?##. 1,1,3\n?#?#?#?#?#?#?#? 1,3,1,6\n????.#...#... 4,1,1\n????.######..#####. 1,6,5\n?###???????? 3,2,1")]
    //[Puzzle(expected: 7541)]
    public long Part2(string input)
        => ReadLines(input).Select(x => ParseInput(x, true)).Sum(x => CalculatePermutations(x.Item1, x.Item2));

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
    private long TotalCalls = 0L;
    private long CacheHits = 0L;
    private long CalculatePermutations(string sections, List<int> groups)
    {
        TotalCalls++;
        if (Cache.TryGetValue(CachKey(sections, groups), out long ans))
        {
            CacheHits++;
            return ans;
        }
        var flex = Flexibility(sections, groups);
        if (!sections.Contains('?') || groups.Count == 0 || flex == 0) return 1;
        if (sections.All(x => x == '?'))
        {
            return AllQuestionMarks(groups.Count, flex + 1);
        }
        //var certainGroups = FindCertainGroups(sections);
        //if (certainGroups.Any(x => x.Item1 != -1))
        //{
        //    var groupLengths = certainGroups.Select(x => x.Item2).Where(x => x > 0);

        //    foreach (var l in groupLengths)
        //    {
        //        if (certainGroups.Count(x => x.Item2 == l) == groups.Count(x => x == l))
        //        {
        //            var anss = MultiSplitCombinations(sections, groups, certainGroups.Where(x => x.Item2 == l));
        //            Cache[CachKey(sections, groups)] = anss;
        //            return anss;
        //        }
        //    }
        //}
        for (int i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            var certainSpots = group - flex;
            var startPossible = groups[..i].Sum() + i;
            if (certainSpots > 0)
            {
                var startCertain = startPossible + group - certainSpots;
                for (int j = 0; j < certainSpots; j++)
                {
                    sections = SetHekje(sections, j + startCertain);
                }
            }
            var potentialSpots = 0;
            var singlePossibleStart = -1;
            for (int j = startPossible; j < startPossible + flex + 1; j++)
            {
                if (j > 0 && '#' == sections[j - 1]) continue;
                if (j + group < sections.Length && '#' == sections[j + group]) continue;
                var upper = j + group;
                if (sections[j..upper].Any(x => x == '.')) continue;

                if (j > 0 && SimpleInvalid(sections[..(j - 1)], groups.Take(i))) continue;
                if (j + group < sections.Length && SimpleInvalid(sections[(upper + 1)..], groups.Skip(i + 1))) continue;

                potentialSpots++;
                if (potentialSpots > 1) break;
                singlePossibleStart = j;
            }
            if (potentialSpots == 1)
            {
                var aEnd = singlePossibleStart - 1;
                var seca = aEnd < 0 ? "" : sections[..aEnd];
                var bStart = singlePossibleStart + group + 1;
                var secb = bStart >= sections.Length ? "" : sections[bStart..];
                var b = CalculatePermutations(seca, groups.Take(i).ToList()) * CalculatePermutations(secb, groups.Skip(i + 1).ToList());
                Cache[CachKey(sections, groups)] = b;
                return b;
            }
        }
        var line = CachKey(sections, groups);
        var a = BruteForceV2(sections, groups);
        // opslaan
        Cache[CachKey(sections, groups)] = a;
        return a;
    }

    private long MultiSplitCombinations(string sections, List<int> groups, IEnumerable<(int, int)> enumerable)
    {

        throw new NotImplementedException();
    }

    private static List<(int, int)> FindCertainGroups(string sections)
    {
        var hekjeCounter = 0;
        var encounteredQuestion = false;
        var resetByQuestion = false;
        var foundGroups = new List<(int, int)>();
        for (int pos = 0; pos < sections.Length; pos++)
        {
            var c = sections[pos];
            if (c == '.')
            {
                if (!resetByQuestion && hekjeCounter > 0)
                {
                    if (encounteredQuestion) foundGroups.Add((-1, -1));
                    foundGroups.Add((pos - hekjeCounter, hekjeCounter));
                    encounteredQuestion = false;
                }
                resetByQuestion = false;
                hekjeCounter = 0;
            }
            if (c == '?')
            {
                encounteredQuestion = true;
                resetByQuestion = true;
                hekjeCounter = 0;
            }
            if (c == '#')
            {
                hekjeCounter++;
            }
        }
        if (hekjeCounter > 0)
        {
            if (!resetByQuestion)
            {
                if (encounteredQuestion) foundGroups.Add((-1, -1));
                foundGroups.Add((sections.Length - hekjeCounter, hekjeCounter));
            }
            resetByQuestion = false;
            hekjeCounter = 0;
        }
        return foundGroups;
    }

    // private string PlaceCertainDots(string sections, List<int> groups)
    // {
    //     var updated = false;
    //     do
    //     {
    //         updated = false;

    //         for (int i = 0; i < sections.Length; i++)
    //         {
    //             if (sections[i] == '?')
    //             {
    //                 var leadingHeksjes = int.Max(0, i - sections.LastIndexOfAny(['?', '.'], i - 1));
    //                 var trailingHeksjes = int.Max(0, sections.IndexOfAny(['?', '.'], i + 1) - i);
    //                 if (leadingHeksjes + trailingHeksjes)
    //             }
    //         }
    //     } while (updated);
    //     throw new NotImplementedException();
    // }

    private static string SetHekje(string sections, int v)
    {
        StringBuilder strB = new(sections);
        strB[v] = '#';
        return strB.ToString();
    }

    public long AllQuestionMarks(int count, int options)
    {
        if (AllQuestionCache.TryGetValue((count, options), out long ans)) return ans;
        if (count == 1 || options == 1) return options;
        var total = 0L;
        for (int i = 1; i <= options; i++)
        {
            total += AllQuestionMarks(count - 1, i);
        }
        AllQuestionCache[(count, options)] = total;
        return total;
    }

    private long BruteForceV2(string sections, List<int> groups)
    {
        var flex = Flexibility(sections, groups);
        var group = groups[0];
        var certainSpots = group - flex;
        var startPossible = 0;
        var totals = 0L;
        for (int j = startPossible; j < startPossible + flex + 1; j++)
        {
            if (j > 0 && '#' == sections[j - 1]) continue;
            if (j + group < sections.Length && '#' == sections[j + group]) continue;
            var upper = j + group;
            if (sections[j..upper].Any(x => x == '.')) continue;

            if (j > 0 && SimpleInvalid(sections[..(j - 1)], groups.Take(0))) continue;
            if (j + group < sections.Length && SimpleInvalid(sections[(upper + 1)..], groups.Skip(1))) continue;
            var bStart = startPossible + group + 1;
            var secb = bStart >= sections.Length ? "" : sections[bStart..];
            totals += CalculatePermutations(secb, groups.Skip(1).ToList());
        }
        return totals;
    }

    private static bool IsValid(string line)
    {
        var (springs, configuration) = (line.Split(" ")[0], line.Split(" ")[1]);
        var springGroups = GetSpringGroups(springs);
        return springGroups == configuration;
    }

    private static string GetSpringGroups(string springs)
        => string.Join(",", springs.Split('.').Where(x => x.Length != 0).Select(x => x.Length));

    // TODO hier misschien ook cachen
    private bool SimpleInvalid(string sections, IEnumerable<int> groups)
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

    private static int Flexibility(string sections, List<int> groups)
        => sections.Length - groups.Count - groups.Sum() + 1;

    private static string CachKey(string sections, List<int> groups)
        => new string(sections) + " " + string.Join(",", groups);

    public static string Reduce(string s)
    {
        s = s.Trim('.');
        while (s.Contains("..")) s = s.Replace("..", ".");
        return s;
    }

    // check if beperkt tot 1 ... set
    // gebruik dat voor extra beperking in positie
    // plaatst zekere posities
    // if compleet ding geplaatst
    // breek in stukken en reduce again (reduce altijd na plaatsen)
}
