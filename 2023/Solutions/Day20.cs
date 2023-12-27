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
        // var prevstate = StateString(nodes);
        // FindEffectiveOutput(nodes);
        for (int i = 0; i < 0; i++)
        {
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
            // Console.WriteLine(pulseCount);

            var state = StateString(nodes);
            // LogStateDiff(state, prevstate);
            // prevstate = state;
            // LogState(nodes);
        }
        return 1;
    }

    private static void FindEffectiveOutput(Dictionary<string, Node> nodes, string start)
    {
        var shouldBeInLoop = nodes.Where(x => x.Value.Outputs.Contains(start) && x.Key != "roadcaster").Select(x => x.Key).ToHashSet();
        var nettoOutput = start;
        var toEval = new Queue<string>();
        var visited = new List<string> { start };
        foreach (var n in nodes[start].Outputs)
        {
            toEval.Enqueue(n);
        }
        // ik moet alles wat nog niet geloopt is als potentiele exit bij houden
        // bij 1to1 kan een potentiele exit door schuiven
        // start is een hele tijd potentiele exit tot dat qq (en de andere start in en outputs) is afgerond
        // daarna gaan we door en komen we op jm en dat is een potentiele exit die we niet kunnen afronden dus dan is de stap er voor nog de exit

        while (toEval.Count > 1)
        {
            var next = toEval.Dequeue();
            visited.Add(next);
            if (nodes[next].Type == '&')
            {
                foreach (var item in nodes[next].RememberedSignals.Keys)
                {
                    shouldBeInLoop.Add(item);
                }
            }
            foreach (var item in nodes[next].RememberedSignals.Keys.Where(x => !visited.Contains(x) && !toEval.Contains(x)))
            {
                toEval.Enqueue(item);
            }
        }

        foreach (var node in nodes)
        {
            foreach (var output in node.Value.Outputs)
            {
                Console.WriteLine($"{node.Value.Name} --> {output}");
            }
        }
    }

    private static void LogStateDiff(object state, object prevstate)
    {
        throw new NotImplementedException();
    }

    private static object StateString(Dictionary<string, Node> nodes)
    {
        throw new NotImplementedException();
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
