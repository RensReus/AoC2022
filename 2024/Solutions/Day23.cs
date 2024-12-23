namespace AoC2024;

class Day23 : BaseDay
{
    [Example(expected: 7, input: "kh-tc\nqp-kh\nde-cg\nka-co\nyn-aq\nqp-ub\ncg-tb\nvc-aq\ntb-ka\nwh-tc\nyn-cg\nkh-ub\nta-co\nde-co\ntc-td\ntb-wq\nwh-td\nta-ka\ntd-qp\naq-cg\nwq-ub\nub-vc\nde-ta\nwq-aq\nwq-vc\nwh-yn\nka-de\nkh-ta\nco-tc\nwh-qp\ntb-vc\ntd-yn")]
    [Puzzle(expected: 1344)]
    public static int Part1(string input)
    {
        var nodes = BuildNodes(input);
        var triples = new HashSet<(string, string, string)>();
        foreach (var node in nodes.Values)
        {
            foreach (var neighbor in node.Neighbors)
            {
                var commonNeighbors = node.Neighbors.Intersect(nodes[neighbor].Neighbors);
                foreach (var commonNeighbor in commonNeighbors)
                {
                    var triple = new[] { node.Name, neighbor, commonNeighbor }.OrderBy(x => x).ToArray();
                    triples.Add((triple[0], triple[1], triple[2]));
                }
            }
        }
        return triples.Count(x => x.Item1.StartsWith('t') || x.Item2.StartsWith('t') || x.Item3.StartsWith('t'));
    }

    private static Dictionary<string, Node> BuildNodes(string input)
    {
        var nodes = new Dictionary<string, Node>();
        var lines = ReadLines(input);
        foreach (var line in lines)
        {
            var parts = line.Split('-');
            var a = parts[0];
            var b = parts[1];
            if (!nodes.ContainsKey(a))
            {
                nodes[a] = new Node(a);
            }
            nodes[a].Neighbors.Add(b);

            if (!nodes.ContainsKey(b))
            {
                nodes[b] = new Node(b);
            }
            nodes[b].Neighbors.Add(a);
        }
        return nodes;
    }

    [Example(expected: "co,de,ka,ta", input: "kh-tc\nqp-kh\nde-cg\nka-co\nyn-aq\nqp-ub\ncg-tb\nvc-aq\ntb-ka\nwh-tc\nyn-cg\nkh-ub\nta-co\nde-co\ntc-td\ntb-wq\nwh-td\nta-ka\ntd-qp\naq-cg\nwq-ub\nub-vc\nde-ta\nwq-aq\nwq-vc\nwh-yn\nka-de\nkh-ta\nco-tc\nwh-qp\ntb-vc\ntd-yn")]
    [Puzzle(expected: "ab,al,cq,cr,da,db,dr,fw,ly,mn,od,py,uh")]
    public static string Part2(string input)
    {
        var nodes = BuildNodes(input);
        var biggest = new HashSet<string>();
        var memory = new Dictionary<string, HashSet<string>>();
        foreach (var node in nodes.Values)
        {
            var neighbors = node.Neighbors;
            var combinations = GetBestPossibleCombination(neighbors, nodes, biggest.Count - 1, memory);
            combinations.Add(node.Name);
            if (combinations.Count > biggest.Count)
            {
                biggest = combinations;
            }
        }
        return string.Join(',', biggest.OrderBy(x => x));
    }

    private static HashSet<string> GetBestPossibleCombination(HashSet<string> neighbors, Dictionary<string, Node> nodes, int count, Dictionary<string, HashSet<string>> memory)
    {
        var key = string.Join(',', neighbors.OrderBy(x => x));
        if (memory.TryGetValue(key, out var output))
        {
            return output;
        }
        if (neighbors.Count <= Math.Max(count, 0))
        {
            memory[key] = [];
            return [];
        }

        if (AllNeighborsAreConnected(neighbors, nodes))
        {
            memory[key] = neighbors;
            return neighbors;
        }

        var bestPossible = new HashSet<string>();
        foreach (var neighbor in neighbors)
        {
            var newNeighbors = new HashSet<string>(neighbors);
            newNeighbors.Remove(neighbor);
            var result = GetBestPossibleCombination(newNeighbors, nodes, count, memory);
            if (result.Count > count && result.Count > bestPossible.Count)
            {
                bestPossible = result;
            }
        }
        memory[key] = bestPossible;
        return bestPossible;
    }

    private static bool AllNeighborsAreConnected(HashSet<string> neighbors, Dictionary<string, Node> nodes)
    {
        foreach (var neighbor in neighbors)
        {
            var newNeighbors = new HashSet<string>(neighbors);
            newNeighbors.Remove(neighbor);
            if (newNeighbors.Any(n => !nodes[neighbor].Neighbors.Contains(n)))
            {
                return false;
            }
        }
        return true;
    }

    private record Node(string Name)
    {
        public HashSet<string> Neighbors { get; } = [];
    }
}