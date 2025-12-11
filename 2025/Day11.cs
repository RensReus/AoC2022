
namespace AoC2025;

class Day11 : BaseDay
{
    [Example(expected: 5, input: "aaa: you hhh\nyou: bbb ccc\nbbb: ddd eee\nccc: ddd eee fff\nddd: ggg\neee: out\nfff: out\nggg: out\nhhh: ccc fff iii\niii: out")]
    [Puzzle(expected: 658)]
    public static int Part1(string input)
    {
        var nodes = ReadLines(input).ToDictionary(
            line => line.Split(": ")[0],
            line => line.Split(": ")[1].Split().ToList()
        );
        return GetPathCount("you", "out", nodes);
    }

    private static int GetPathCount(string start, string end, Dictionary<string, List<string>> nodes)
    {
        if (start == "out")
        {
            return 0;
        }
        return nodes[start].Sum(output => output == end ? 1 : GetPathCount(output, end, nodes));
    }

    [Example(expected: 2, input: "svr: aaa bbb\naaa: fft\nfft: ccc\nbbb: tty\ntty: ccc\nccc: ddd eee\nddd: hub\nhub: fff\neee: dac\ndac: fff\nfff: ggg hhh\nggg: out\nhhh: out")]
    [Puzzle(expected: 371113003846800)]
    public static long Part2(string input)
    {
        var nodes = ReadLines(input).ToDictionary(
            line => line.Split(": ")[0],
            line => line.Split(": ")[1].Split().ToList()
        );
        nodes["out"] = [];

        var invertedNodes = new Dictionary<string, List<string>> { ["svr"] = [] };
        foreach (var (key, values) in nodes)
        {
            foreach (var value in values)
            {
                if (!invertedNodes.ContainsKey(value))
                {
                    invertedNodes[value] = [];
                }
                invertedNodes[value].Add(key);
            }
        }

        var toEval = nodes["svr"].ToHashSet();
        var possibleRoutesToX = new Dictionary<string, (long None, long DacOnly, long FftOnly, long Both)>
        {
            ["svr"] = (1, 0, 0, 0)
        };

        while (toEval.Count > 0)
        {
            var nextToEval = new HashSet<string>();
            foreach (var node in toEval)
            {
                if (invertedNodes[node].All(possibleRoutesToX.ContainsKey))
                {
                    var possibleRoutes = (None: 0L, DacOnly: 0L, FftOnly: 0L, Both: 0L);
                    nextToEval.UnionWith(nodes[node]);
                    foreach (var parent in invertedNodes[node])
                    {
                        var routes = possibleRoutesToX[parent];
                        possibleRoutes.Both += routes.Both;
                        if (node == "dac")
                        {
                            possibleRoutes.Both += routes.FftOnly;
                            possibleRoutes.DacOnly += routes.None;
                        }
                        else if (node == "fft")
                        {
                            possibleRoutes.Both += routes.DacOnly;
                            possibleRoutes.FftOnly += routes.None;
                        }
                        else
                        {
                            possibleRoutes.FftOnly += routes.FftOnly;
                            possibleRoutes.DacOnly += routes.DacOnly;
                            possibleRoutes.None += routes.None;
                        }
                    }
                    possibleRoutesToX[node] = possibleRoutes;
                }
                else
                {
                    nextToEval.Add(node);
                }
            }
            toEval = nextToEval;
        }
        return possibleRoutesToX["out"].Both;
    }
}