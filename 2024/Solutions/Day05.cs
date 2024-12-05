namespace AoC2024;

class Day05 : BaseDay
{
    [Example(expected: 143, input: "47|53\n97|13\n97|61\n97|47\n75|29\n61|13\n75|53\n29|13\n97|29\n53|29\n61|53\n97|53\n61|29\n47|13\n75|47\n97|75\n47|61\n75|61\n47|29\n75|13\n53|13\n\n75,47,61,53,29\n97,61,53,29,13\n75,29,13\n75,97,47,61,53\n61,13,29\n97,13,75,29,47")]
    [Puzzle(expected: 5374)]
    public static int Part1(string input)
    {
        var lines = ReadLinesDouble(input);
        var ordering = BuildOrdering(lines[0]);
        var updates = lines[1].Select(x => x.Split(",").ToList()).ToList();
        return updates.Where(x => InOrder(x, ordering)).Sum(MiddleNumber);
    }

    private static Dictionary<string, List<string>> BuildOrdering(List<string> list)
    {
        var dict = new Dictionary<string, List<string>>();
        foreach (var line in list)
        {
            var parts = line.Split("|");
            if (!dict.ContainsKey(parts[0])) dict[parts[0]] = [];
            dict[parts[0]].Add(parts[1]);
        }

        return dict;
    }

    private static bool InOrder(List<string> arg1, Dictionary<string, List<string>> ordering)
    {
        for (var i = 0; i < arg1.Count; i++)
        {
            if (!ordering.ContainsKey(arg1[i])) continue;
            if (ordering[arg1[i]].Intersect(arg1[..i]).Any()) return false;
        }

        return true;
    }

    private static int MiddleNumber(List<string> arg1)
    {
        var length = arg1.Count - 1;
        return int.Parse(arg1[length / 2]);
    }

    [Example(expected: 123, input: "47|53\n97|13\n97|61\n97|47\n75|29\n61|13\n75|53\n29|13\n97|29\n53|29\n61|53\n97|53\n61|29\n47|13\n75|47\n97|75\n47|61\n75|61\n47|29\n75|13\n53|13\n\n75,47,61,53,29\n97,61,53,29,13\n75,29,13\n75,97,47,61,53\n61,13,29\n97,13,75,29,47")]
    [Puzzle(expected: 4260)]
    public static int Part2(string input)
    {
        var lines = ReadLinesDouble(input);
        var ordering = BuildOrdering(lines[0]);
        var updates = lines[1].Select(x => x.Split(",").ToList()).ToList();
        var unordered = updates.Where(x => !InOrder(x, ordering)).ToList();
        return unordered.Select(x => SortNumbers(x, ordering)).Sum(MiddleNumber);
    }

    private static List<string> SortNumbers(List<string> unordered, Dictionary<string, List<string>> ordering)
    {
        while (!InOrder(unordered, ordering))
        {
            for (var i = 0; i < unordered.Count; i++)
            {
                if (!ordering.ContainsKey(unordered[i])) continue;
                var wrong = ordering[unordered[i]].Intersect(unordered[..i]);
                if (wrong.Any())
                {
                    var index = unordered.FindIndex(x => x == wrong.First());
                    (unordered[index], unordered[i]) = (unordered[i], unordered[index]);
                }
            }
        }
        return unordered;
    }
}