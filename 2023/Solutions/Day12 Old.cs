using System.Text;

namespace AoC2023;

class Day121 : BaseDay
{
    [Example(expected: 21, input: "???.### 1,1,3\n.??..??...?##. 1,1,3\n?#?#?#?#?#?#?#? 1,3,1,6\n????.#...#... 4,1,1\n????.######..#####. 1,6,5\n?###???????? 3,2,1")]
    [Example(expected: 4, input: ".??..??...?##. 1,1,3")]
    [Example(expected: 4, input: "??.#??#?????##? 2,1,3,4")]
    [Puzzle(expected: 7541)]
    public static int Part1(string input)
        => ReadLines(input).Sum(CalculatePermutations);

    private static int CalculatePermutations(string line)
    {
        var (springs, config) = (line.Split(" ")[0], line.Split(" ")[1].Split(',').Select(int.Parse).ToList());
        while (springs.Contains("..")) springs = springs.Replace("..", ".");
        springs = springs.Trim('.');
        Console.WriteLine(line);
        var answer = CalculatePermutations(springs, config);
        Console.WriteLine(answer);
        return answer;
    }

    private static int CalculatePermutations(string springs, List<int> config)
    {
        var totalHekje = config.Sum();
        var totalOpen = springs.Length - totalHekje;
        var totalVariable = springs.Length - totalHekje - config.Count + 1;
        if (totalVariable == 0) return 1;
        while (true)
        {
            var newSprings = ReplaceSureFields(springs, config, totalHekje, totalOpen, totalVariable);
            (newSprings, config) = RemoveCompletedBlocks(newSprings, config);

            if (newSprings == springs) break;
            springs = newSprings;
            if (!springs.Contains('?')) return IsValid(springs, config) ? 1 : 0;
        }

        if (!springs.Contains('?')) return IsValid(springs, config) ? 1 : 0;
        if (PrematureInvalid(springs, config)) return 0;
        var regex = new Regex(Regex.Escape("?"));
        var permutations = new string[] { regex.Replace(springs.ToString(), "#", 1), regex.Replace(springs.ToString(), ".", 1) };
        return permutations.Sum(x => CalculatePermutations(x, config));
    }

    private static (string, List<int>) RemoveCompletedBlocks(string springs, List<int> config)
    {
        var newConfig = new List<int>(config);
        while (true)
        {
            if (newConfig.Count == 0) break;
            var newSprings = springs;
            var leadingHeksjes = springs.IndexOfAny(['.', '?']);
            if (leadingHeksjes == newConfig[0])
            {
                newSprings = springs[(leadingHeksjes + 1)..].Trim('.');
                newConfig.RemoveAt(0);
            }
            if (newConfig.Count == 0) { springs = newSprings; break; }
            var trailingHeksjes = newSprings.Length - newSprings.LastIndexOfAny(['.', '?']);
            if (trailingHeksjes == newConfig[^1] + 1 && newSprings.LastIndexOfAny(['.', '?']) != -1)
            {
                newSprings = newSprings[..^trailingHeksjes].Trim('.');
                newConfig.RemoveAt(newConfig.Count - 1);
            }

            // Todo remove blocks in the middle
            if (newSprings == springs) { springs = newSprings; break; }
            springs = newSprings;
        }

        return (springs, newConfig);
    }

    private static string ReplaceSureFields(string springs, List<int> config, int totalHekje, int totalOpen, int totalVariable)
    {
        var newSprings = PlaceSureHekje(springs, config, totalHekje, totalOpen, totalVariable);
        newSprings = DueToImpossiblity(newSprings, config);
        return newSprings;
    }

    private static string PlaceSureHekje(string springs, List<int> config, int totalHekje, int totalOpen, int totalVariable)
    {
        var sureHekjes = config.Select(x => x - totalVariable).ToList();
        if (sureHekjes.All(x => x == 0)) return springs;
        var pos = 0;
        for (int i = 0; i < config.Count; i++)
        {
            var sure = sureHekjes[i];
            if (sure > 0)
            {
                var updater = new StringBuilder(springs);
                for (int j = pos + config[i] - sure; j < pos + config[i]; j++)
                {
                    updater[j] = '#';
                }
                springs = updater.ToString();
            }
            pos += 1 + config[i];
        }
        return springs;
    }

    private static string DueToImpossiblity(string springs, List<int> config)
    {
        for (int i = 0; i < springs.Length; i++)
        {
            if (springs[i] != '?') continue;
            var possHekje = new StringBuilder(springs);
            possHekje[i] = '#';
            var possDot = new StringBuilder(springs);
            possDot[i] = '.';
            if (PrematureInvalid(possDot.ToString(), config)) springs = possHekje.ToString();
            else if (PrematureInvalid(possHekje.ToString(), config)) springs = possDot.ToString();
        }
        return springs;
    }

    private static bool PrematureInvalid(string springs, List<int> config)
    {
        if (springs.Count(x => x == '#') > config.Sum()) return true;
        if (springs.Count(x => x == '.') > springs.Length - config.Sum()) return true;
        var springCounts1 = springs.Split(new char[] { '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Length);
        var tooLong = springCounts1.Any(count => count > config.Max());
        // check if it contains any with a length that doesn't exist
        var springCounts2 = springs.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Where(x => !x.Contains('?')).Select(x => x.Length);
        var nonExistant = springCounts2.Any(x => !config.Contains(x));
        return tooLong || nonExistant;
    }

    private static bool IsValid(string springs, List<int> config)
    {
        var springGroups = GetSpringGroups(springs);
        return springGroups == string.Join(",", config);
    }

    private static string GetSpringGroups(string springs)
        => string.Join(",", springs.ToString().Split('.').Where(x => x.Length != 0).Select(x => x.Length));

    [Example(expected: 525152, input: "???.### 1,1,3\n.??..??...?##. 1,1,3\n?#?#?#?#?#?#?#? 1,3,1,6\n????.#...#... 4,1,1\n????.######..#####. 1,6,5\n?###???????? 3,2,1")]
    [Example(expected: 525152, input: ".??..??...?##. 1,1,3")]
    [Puzzle(expected: 222222)]
    public static int Part2(string input)
        => ReadLines(input).Select(Multiply).Sum(CalculatePermutations);

    private static string Multiply(string line)
    {
        var (springs, configuration) = (line.Split(" ")[0], line.Split(" ")[1]);
        return string.Concat(Enumerable.Repeat(springs + '?', 5))[..^1] + " " + string.Concat(Enumerable.Repeat(configuration + ',', 5))[..^1];
    }
}