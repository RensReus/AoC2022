namespace AoC2023;

class Day121 : BaseDay
{
    [Example(expected: 21, input: "???.### 1,1,3\n.??..??...?##. 1,1,3\n?#?#?#?#?#?#?#? 1,3,1,6\n????.#...#... 4,1,1\n????.######..#####. 1,6,5\n?###???????? 3,2,1")]
    [Puzzle(expected: 7541)]
    public static int Part1(string input)
    {
        var answer = 0;
        var output = new List<(int, long, string)>();
        foreach (var item in ReadLines(input))
        {
            // log timing
            var start = DateTime.Now;
            var count = CalculatePermutations(item);
            var diff = DateTime.Now.Millisecond - start.Millisecond;
            output.Add((diff, count, item));
            answer += count;
        }

        foreach (var item in output)
        {
            Console.WriteLine(item.Item2 + " " + item.Item3);
            // Console.WriteLine(item);
        }
        return answer;
    }

    private static int CalculatePermutations(string line)
    {
        if (!line.Contains('?')) return IsValid(line) ? 1 : 0;
        var regex = new Regex(Regex.Escape("?"));
        var permutations = new string[] { regex.Replace(line, "#", 1), regex.Replace(line, ".", 1) };
        // optional early cutoff
        return permutations.Sum(CalculatePermutations);
    }

    private static bool IsValid(string line)
    {
        var (springs, configuration) = (line.Split(" ")[0], line.Split(" ")[1]);
        var springGroups = GetSpringGroups(springs);
        return springGroups == configuration;
    }

    private static string GetSpringGroups(string springs)
        => string.Join(",", springs.Split('.').Where(x => x.Length != 0).Select(x => x.Length));
}