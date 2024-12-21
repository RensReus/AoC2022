
namespace AoC2024;

class Day21 : BaseDay
{
    private static readonly Dictionary<char, (int, int)> Keypad = new()
    {
        {'7', (0, 0)}, {'8', (0, 1)}, {'9', (0, 2)},
        {'4', (1, 0)}, {'5', (1, 1)}, {'6', (1, 2)},
        {'1', (2, 0)}, {'2', (2, 1)}, {'3', (2, 2)},
        {' ', (3, 0)}, {'0', (3, 1)}, {'A', (3, 2)}
    };

    private static readonly Dictionary<char, (int, int)> DirKeypad = new()
    {
        {' ', (0, 0)}, {'^', (0, 1)}, {'A', (0, 2)},
        {'<', (1, 0)}, {'v', (1, 1)}, {'>', (1, 2)}
    };

    [Example(expected: 126384, input: "029A\n980A\n179A\n456A\n379A")]
    [Puzzle(expected: 134120)]
    public static long Part1(string input)
        => MinimumButtonPresses(input, 2);

    [Puzzle(expected: 167389793580400)]
    public static long Part2(string input)
        => MinimumButtonPresses(input, 25);

    private static long MinimumButtonPresses(string input, int maxIter)
    {
        var memory = new Dictionary<(string, int), long>();
        return ReadLines(input).Sum(line => MinimumButtonPresses(line, memory, maxIter));
    }

    private static long MinimumButtonPresses(string line, Dictionary<(string, int), long> memory, int maxIter)
    {
        var keyPadSteps = GetSteps(line, Keypad);
        var minsteps = keyPadSteps.Min(s => GetMinStepsCount(s, maxIter, memory));
        var number = int.Parse(line[..^1]);
        return minsteps * number;
    }

    private static HashSet<string> GetSteps(string line, Dictionary<char, (int, int)> keypad)
    {
        var pos = keypad['A'];
        var steps = new HashSet<string> { "" };
        foreach (var c in line)
        {
            var target = keypad[c];
            var delta = (target.Item1 - pos.Item1, target.Item2 - pos.Item2);
            var vert = new string(delta.Item1 > 0 ? 'v' : '^', Math.Abs(delta.Item1));
            var hori = new string(delta.Item2 > 0 ? '>' : '<', Math.Abs(delta.Item2));

            var nextSteps = new HashSet<string>();
            if (keypad[' '] != (pos.Item1 + delta.Item1, pos.Item2)) nextSteps.Add(vert + hori + "A");
            if (keypad[' '] != (pos.Item1, pos.Item2 + delta.Item2)) nextSteps.Add(hori + vert + "A");

            var newSteps = new HashSet<string>();
            foreach (var step in steps)
            {
                foreach (var nextStep in nextSteps)
                {
                    newSteps.Add(step + nextStep);
                }
            }
            steps = newSteps;
            pos = target;
        }
        return steps;
    }

    private static long GetMinStepsCount(string line, int iteration, Dictionary<(string, int), long> memory)
    {
        if (memory.TryGetValue((line, iteration), out var ans)) return ans;
        if (iteration == 0)
        {
            memory[(line, iteration)] = line.Length;
            return line.Length;
        }

        var indexA = line.IndexOf('A');
        if (indexA == line.Length - 1)
        {
            var steps = GetSteps(line, DirKeypad);
            memory[(line, iteration)] = steps.Min(s => GetMinStepsCount(s, iteration - 1, memory));
            return memory[(line, iteration)];
        }

        memory[(line, iteration)] = GetMinStepsCount(line[..(indexA + 1)], iteration, memory) + GetMinStepsCount(line[(indexA + 1)..], iteration, memory);
        return memory[(line, iteration)];
    }
}