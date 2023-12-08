
namespace AoC2023;

class Day08 : BaseDay
{
    [Example(expected: 2, input: "RL\n\nAAA = (BBB, CCC)\nBBB = (DDD, EEE)\nCCC = (ZZZ, GGG)\nDDD = (DDD, DDD)\nEEE = (EEE, EEE)\nGGG = (GGG, GGG)\nZZZ = (ZZZ, ZZZ)")]
    [Example(expected: 6, input: "LLR\n\nAAA = (BBB, BBB)\nBBB = (AAA, ZZZ)\nZZZ = (ZZZ, ZZZ)")]
    [Puzzle(expected: 21797)]
    public static int Part1(string input)
    {
        var (steps, nodes) = ProcessInput(input);

        var curr = "AAA";
        var target = "ZZZ";
        var totalSteps = 0;
        while (curr != target)
        {
            var nextStep = steps[totalSteps % steps.Length];
            curr = nextStep == 'L' ? nodes[curr].Item1 : nodes[curr].Item2;
            totalSteps++;
        }
        return totalSteps;
    }

    [Example(expected: 6, input: "LR\n\n11A = (11B, XXX)\n11B = (XXX, 11Z)\n11Z = (11B, XXX)\n22A = (22B, XXX)\n22B = (22C, 22C)\n22C = (22Z, 22Z)\n22Z = (22B, 22B)\nXXX = (XXX, XXX)")]
    [Puzzle(expected: 23977527174353)]
    public static long Part2(string input)
    {
        var (steps, nodes) = ProcessInput(input);

        var lengths = new List<long>();
        var starts = nodes.Select(x => x.Key).Where(x => x[2] == 'A').ToArray();
        foreach (var start in starts)
        {
            var curr = start;
            var totalSteps = 0;
            var visited = new Dictionary<string, int>();
            while (true)
            {
                var stepPos = totalSteps % steps.Length;
                var nextStep = steps[stepPos];
                curr = nextStep == 'L' ? nodes[curr].Item1 : nodes[curr].Item2;
                totalSteps++;
                if (curr[2] == 'Z')
                {
                    if (visited.TryGetValue(curr, out int value))
                    {
                        lengths.Add(totalSteps - value);
                        break;
                    }
                    visited.Add(curr, totalSteps);
                }
            }
        }
        var answer = 0L;
        while (true)
        {
            answer += lengths.Max();
            if (lengths.All(x => answer % x == 0)) break;
        }
        return answer;
    }

    private static (string steps, Dictionary<string, (string, string)>) ProcessInput(string input)
    {
        var lines = ReadLines(input);
        var steps = lines[0];
        var nodes = new Dictionary<string, (string, string)>();
        foreach (var line in lines[2..])
        {
            var parsed = Regex.Match(line, @"(\w+) = \((\w+), (\w+)\)");
            nodes.Add(parsed.Groups[1].Value, (parsed.Groups[2].Value, parsed.Groups[3].Value));
        }
        return (steps, nodes);
    }
}