namespace AoC2023;

class Day12 : BaseDay
{
    public Day12()
    {
        Cache = [];
    }

    [Example(expected: 21, input: "???.### 1,1,3\n.??..??...?##. 1,1,3\n?#?#?#?#?#?#?#? 1,3,1,6\n????.#...#... 4,1,1\n????.######..#####. 1,6,5\n?###???????? 3,2,1")]
    [Example(expected: 1, input: "?#?#?#?#?#?#?#? 1,3,1,6")]
    [Puzzle(expected: 7541)]
    public long Part1(string input)
    {
        var answers = ReadLines(input).Select(ParseInput).Select(x => CalculatePermutations(x.Item1, x.Item2));
        foreach (var ans in answers)
        {
            Console.WriteLine(ans);
        }
        return answers.Sum();
    }

    [Example(expected: 21, input: "???.### 1,1,3\n.??..??...?##. 1,1,3\n?#?#?#?#?#?#?#? 1,3,1,6\n????.#...#... 4,1,1\n????.######..#####. 1,6,5\n?###???????? 3,2,1")]
    [Puzzle(expected: 7541)]
    public long Part2(string input)
        => ReadLines(input).Select(ParseInput).Sum(x => CalculatePermutations(x.Item1, x.Item2));

    private (char[], List<int>) ParseInput(string line)
    {
        var (sections, groups) = (line.Split(" ")[0], line.Split(" ")[1]);
        return (Reduce(sections), groups.Split(",").Select(int.Parse).ToList());
    }

    private long CalculatePermutations(char[] sections, List<int> groups)
    {
        if (Cache.TryGetValue(CachKey(sections, groups), out long ans)) return ans;
        var flex = Flexibility(sections, groups);
        if (!sections.Contains('?') || groups.Count == 0 || flex == 0) return 1;

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
                    sections[startCertain + j] = '#';
                }
            }
            var potentialSpots = 0;
            var singlePossibleStart = -1;
            for (int j = startPossible; j < startPossible + flex + 1; j++)
            {
                if (j > 0 && '#' == sections[j - 1]) continue;
                if (j < sections.Length - group - 1 && '#' == sections[j + group + 1]) continue;
                var upper = j + group;
                if (sections[j..upper].Any(x => x == '.')) continue;

                if (j > 0 && SimpleInvalid(sections.Take(j - 1), groups.Take(i))) continue;
                if (j + group < sections.Length && SimpleInvalid(sections.Skip(j + group + 1), groups.Skip(i + 1))) continue;

                potentialSpots++;
                if (potentialSpots > 1) break;
                singlePossibleStart = j;
            }
            if (potentialSpots == 1)
            {
                var seca = sections.Take(singlePossibleStart - 1).ToArray();
                var secb = sections.Skip(singlePossibleStart + group + 1).ToArray();
                var groupa = groups.Take(i).ToList();
                var groupb = groups.Skip(i + 1).ToList();
                return CalculatePermutations(sections.Take(singlePossibleStart - 1).ToArray(), groups.Take(i).ToList())
                    * CalculatePermutations(sections.Skip(singlePossibleStart + group + 1).ToArray(), groups.Skip(i + 1).ToList());
            }
        }
        var line = CachKey(sections, groups);
        // if alles ?? dan simpele berekening
        return BruteForce(line);
    }

    private static int BruteForce(string line)
    {
        if (!line.Contains('?')) return IsValid(line) ? 1 : 0;
        var regex = new Regex(Regex.Escape("?"));
        var permutations = new string[] { regex.Replace(line, "#", 1), regex.Replace(line, ".", 1) };
        return permutations.Sum(BruteForce);
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
    private bool SimpleInvalid(IEnumerable<char> sections, IEnumerable<int> groups)
    {
        if (!groups.Any()) return sections.Contains('#');
        // trim . en check of lengte nog voldoende
        // niet te veel te grote groepen
        return false;
    }

    private static int Flexibility(char[] sections, List<int> groups)
        => sections.Length - groups.Count - groups.Sum() + 1;

    private static string CachKey(char[] sections, List<int> groups)
        => new string(sections) + " " + string.Join(",", groups);

    public static char[] Reduce(string s)
    {
        s = s.Trim('.');
        while (s.Contains("..")) s = s.Replace("..", ".");
        return s.ToCharArray();
    }

    private Dictionary<string, long> Cache;

    // check if beperkt tot 1 ... set
    // gebruik dat voor extra beperking in positie
    // plaatst zekere posities
    // if compleet ding geplaatst
    // breek in stukken en reduce again (reduce altijd na plaatsen)
}
