
using AoC2022;

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

    [Puzzle(expected: 222222)]
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
        for (int i = 0; i < 100; i++)
        {
            // LogState(nodes);
            var pulseCount = 0;
            if (nodes["jm"].RememberedSignals.Values.Any(x => x))
            {
                Console.WriteLine(i);
                Console.WriteLine(string.Join(", ", nodes["jm"].RememberedSignals.Select(x => $"{x.Key}, {x.Value}")));
            }

            foreach (var output in nodes["roadcaster"].Outputs)
            {
                pulsesToEval.Enqueue(("roadcaster", output, false));
            }
            while (pulsesToEval.Count != 0)
            {
                pulseCount++;
                var pulse = pulsesToEval.Dequeue();
                if (pulse.Item2 == "rx" && !pulse.Item3) return i;
                if (!nodes.TryGetValue(pulse.Item2, out Node? node)) continue;
                var nextPulses = node.ProcessSignal(pulse);
                foreach (var newPulse in nextPulses)
                {
                    pulsesToEval.Enqueue(newPulse);
                }
            }
            Console.WriteLine(pulseCount);
        }
        return 1;
    }

    private static void LogState(Dictionary<string, Node> nodes)
    {
        Console.WriteLine();
        foreach (var node in nodes)
        {
            if (node.Value.Type == '%') Console.Write(node.Value.TurnedOn ? '1' : '0');
            foreach (var item in node.Value.RememberedSignals)
            {
                Console.Write(node.Value.TurnedOn ? '1' : '0');
            }
            Console.Write(",");
        }
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
