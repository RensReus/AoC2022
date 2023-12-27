using System.Numerics;

namespace AoC2023;

class Day20 : BaseDay
{
    [Example(expected: 32000000, input: "broadcaster -> a, b, c\n%a -> b\n%b -> c\n%c -> inv\n&inv -> a")]
    [Example(expected: 11687500, input: "broadcaster -> a\n%a -> inv, con\n&inv -> b\n%b -> con\n&con -> output")]
    [Puzzle(expected: 841763884)]
    public static long Part1(string input)
    {
        var nodes = ReadLines(input).Select(line => new Node(line)).ToDictionary(x => x.Name, x => x);
        var pulsesToEval = new Queue<(string, string, bool)>();
        foreach (var node in nodes)
        {
            foreach (var output in node.Value.Outputs)
            {
                if (nodes.TryGetValue(output, out Node? value) && value.Type == '&') value.RememberedSignals[node.Key] = false;
            }
        }
        var lowPulses = 0L;
        var highPulses = 0L;
        for (int i = 0; i < 1000; i++)
        {
            lowPulses++;
            foreach (var output in nodes["roadcaster"].Outputs)
            {
                pulsesToEval.Enqueue(("roadcaster", output, false));
            }
            while (pulsesToEval.Count != 0)
            {
                var pulse = pulsesToEval.Dequeue();
                if (pulse.Item3) highPulses++;
                else lowPulses++;
                if (!nodes.TryGetValue(pulse.Item2, out Node? node)) continue;
                var nextPulses = node.ProcessSignal(pulse);
                foreach (var newPulse in nextPulses)
                {
                    pulsesToEval.Enqueue(newPulse);
                }
            }
        }
        return lowPulses * highPulses;
    }

    [Puzzle(expected: 246006621493687)]
    public static long Part2(string input)
    {
        var nodes = ReadLines(input).Select(line => new Node(line)).ToDictionary(x => x.Name, x => x);
        var pulsesToEval = new Queue<(string, string, bool)>();
        foreach (var node in nodes)
        {
            foreach (var output in node.Value.Outputs)
            {
                if (nodes.TryGetValue(output, out Node? value) && value.Type == '&') value.RememberedSignals[node.Key] = false;
            }
        }
        var cycles = new Dictionary<string, int>();
        var i = 0;
        while (cycles.Count < nodes["jm"].RememberedSignals.Count)
        {
            i++;

            foreach (var output in nodes["roadcaster"].Outputs)
            {
                pulsesToEval.Enqueue(("roadcaster", output, false));
            }
            while (pulsesToEval.Count != 0)
            {
                var pulse = pulsesToEval.Dequeue();
                if (pulse.Item2 == "rx" && !pulse.Item3) return i;
                if (!nodes.TryGetValue(pulse.Item2, out Node? node)) continue;
                var nextPulses = node.ProcessSignal(pulse);
                foreach (var newPulse in nextPulses)
                {
                    pulsesToEval.Enqueue(newPulse);
                }

                if (nodes["jm"].RememberedSignals.ContainsKey(pulse.Item2) && !pulse.Item3 && !cycles.ContainsKey(pulse.Item2))
                {
                    cycles[pulse.Item2] = i;
                }
            }
        }

        var minPeriod = cycles.Values
            .Select(x => new BigInteger(x))
            .Aggregate((x, y) => x * y / BigInteger.GreatestCommonDivisor(x, y));
        return (long)minPeriod;
    }

    private class Node
    {
        public char Type;
        public string Name;
        public Dictionary<string, bool> RememberedSignals = [];
        public List<string> Outputs;
        public bool TurnedOn;

        public Node(string line)
        {
            var (nametype, outputs) = (line.Split(" -> ")[0], line.Split(" -> ")[1]);
            Type = nametype[0];
            Name = nametype[1..];
            Outputs = outputs.Split(", ").ToList();
        }

        internal IEnumerable<(string, string, bool)> ProcessSignal((string, string, bool) pulse)
        {
            if (Type == '%')
            {
                if (pulse.Item3) return [];
                TurnedOn = !TurnedOn;
                return Outputs.Select(x => (Name, x, TurnedOn));
            }
            RememberedSignals[pulse.Item1] = pulse.Item3;
            var output = RememberedSignals.Values.Any(x => !x);
            return Outputs.Select(x => (Name, x, output));
        }
    }
}
