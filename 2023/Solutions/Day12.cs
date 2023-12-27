


namespace AoC2023;

class Day12 : BaseDay
{
    public Day12()
    {
        Cache = [];
    }

    [Example(expected: 21, input: "???.### 1,1,3\n.??..??...?##. 1,1,3\n?#?#?#?#?#?#?#? 1,3,1,6\n????.#...#... 4,1,1\n????.######..#####. 1,6,5\n?###???????? 3,2,1")]
    [Puzzle(expected: 7541)]
    public long Part1(string input)
        => ReadLines(input).Select(ParseInput).Sum(x => CalculatePermutations(x.Item1, x.Item2));

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
        if (!sections.Contains('?') || flex == 0) return 1;


        // for each
        // try position
        // check validity (dit is een diepe recursieve optie) maar misschien gewoon dat doen en kijken of die 0 of meer opties geeft
        // if has single possible spot use to split
        // 
        for (int i = 0; i < groups.Count; i++)
        {
            var group = groups[i];
            var prev = groups.Take(i);
            var certainSpots = group - flex;
            var startPossible = prev.Sum() + i;
            if (certainSpots > 0)
            {
                var startCertain = startPossible + group - certainSpots;
                for (int j = 0; j < certainSpots; j++)
                {
                    sections[startCertain + j] = '#';
                }
            }

            var overig = groups.Skip(i + 1);
            var endPossible = overig.Count() + overig.Sum();
            // validity check dmv splitten?
            for (int j = 0; j < endPossible; j++)
            {
                // 
            }
            // exit condition if fully placed group
        }
        // sections begint altijd met ? of #
        // probeer te kijken waar de eerste kan/ al zit
        // als meer dan 1 doe dit voor de volgende 
        // if placed group split
        return 0;//?????
    }

    private static int Flexibility(char[] sections, List<int> groups)
        => sections.Length - groups.Count - groups.Sum() + 1;

    private static string CachKey(char[] sections, List<int> groups)
        => sections + "$" + string.Join(",", groups);

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
