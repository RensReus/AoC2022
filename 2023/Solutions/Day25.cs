



namespace AoC2023;

class Day25 : BaseDay
{
    [Example(expected: 54, input: "jqt: rhn xhk nvd\nrsh: frs pzl lsr\nxhk: hfx\ncmg: qnr nvd lhk bvb\nrhn: xhk bvb hfx\nbvb: xhk hfx\npzl: lsr hfx nvd\nqnr: nvd\nntq: jqt hfx bvb xhk\nnvd: lhk\nlsr: lhk\nrzs: qnr cmg lsr rsh\nfrs: qnr lhk lsr")]
    [Puzzle(expected: 567606)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);
        var nodes = new Dictionary<string, List<string>>();
        var connections = new List<(string, string)>();
        foreach (var line in lines)
        {
            var (first, rest) = (line.Split(": ")[0], line.Split(": ")[1].Split(" "));
            foreach (var r in rest)
            {
                if (nodes.TryGetValue(first, out List<string>? cons)) cons.Add(r);
                else nodes[first] = [r];

                if (nodes.TryGetValue(r, out List<string>? cons2)) cons2.Add(first);
                else nodes[r] = [first];
                connections.Add((first, r));
            }
        }
        var conWeight = connections.Select(x => (x, Isolation(x, nodes))).OrderByDescending(x => x.Item2).ToList();
        // LogConweights(conWeight, nodes);
        var isolatedCount = conWeight.Where(x => x.Item2 == 1).Count();
        var count = int.Max(isolatedCount, 30);
        for (int i = 0; i < count; i++)
        {
            for (int j = i + 1; j < count; j++)
            {
                for (int k = j + 1; k < count; k++)
                {
                    List<(string, string)> cutlines = [conWeight[i].x, conWeight[j].x, conWeight[k].x];
                    var groupSize = GroupSize(nodes, cutlines);
                    if (groupSize != nodes.Count)
                    {
                        Console.WriteLine($"{conWeight[i].x},{conWeight[i].x},{conWeight[i].x})");
                        return groupSize * (nodes.Count - groupSize);
                    }
                }
            }
        }

        return connections.Count;
    }

    private static int GroupSize(Dictionary<string, List<string>> nodes, List<(string, string)> list)
    {
        var first = nodes.First().Key;
        var visited = new HashSet<string> { first };
        var toVisit = new Queue<string>();
        toVisit.Enqueue(nodes.First().Key);
        while (toVisit.Count > 0)
        {
            var next = toVisit.Dequeue();
            visited.Add(next);
            foreach (var n in nodes[next].Where(x => !visited.Contains(x) && NotCutline(x, next, list)))
            {
                toVisit.Enqueue(n);
                visited.Add(n);
            }
        }
        return visited.Count;
    }

    private static bool NotCutline(string a, string b, List<(string, string)> list)
    {
        return !list.Any(x => (x.Item1 == a && x.Item2 == b) || (x.Item1 == b && x.Item2 == a));
    }

    private static decimal Isolation((string, string) x, Dictionary<string, List<string>> nodes)
    {
        var first = new List<string>();
        var second = new List<string>();

        first.AddRange(nodes[x.Item1]);
        foreach (var item in nodes[x.Item1].Where(y => y != x.Item2))
        {
            first.AddRange(nodes[item].Where(y => y != x.Item1));
            foreach (var item2 in nodes[item])
            {
                first.AddRange(nodes[item2]);

            }
        }
        second.AddRange(nodes[x.Item2]);
        foreach (var item in nodes[x.Item2].Where(y => y != x.Item1))
        {
            second.AddRange(nodes[item].Where(y => y != x.Item2));
            foreach (var item2 in nodes[item])
            {
                second.AddRange(nodes[item2]);
            }
        }
        first = first.Distinct().ToList();
        second = second.Distinct().ToList();
        var combined = first.Distinct().ToList();
        combined.AddRange(second);
        return (decimal)combined.Distinct().Count() / (first.Count + second.Count);
    }
}