





namespace AoC2023;

class Day17 : BaseDay
{
    [Example(expected: 102, input: "2413432311323\n3215453535623\n3255245654254\n3446585845452\n4546657867536\n1438598798454\n4457876987766\n3637877979653\n4654967986887\n4564679986453\n1224686865563\n2546548887735\n4322674655533")]
    [Example(expected: 14, input: "41191119\n99111911\n99999994")]
    [Example(expected: 13, input: "41191119\n99111911\n99992114")]
    [Puzzle(expected: 758)]
    public static int Part1(string input)
        => FindColdestRoute(input, 3, 0);

    [Example(expected: 94, input: "2413432311323\n3215453535623\n3255245654254\n3446585845452\n4546657867536\n1438598798454\n4457876987766\n3637877979653\n4654967986887\n4564679986453\n1224686865563\n2546548887735\n4322674655533")]
    [Example(expected: 71, input: "111111111111\n999999999991\n999999999991\n999999999991\n999999999991")]
    [Puzzle(expected: 892)]
    public static int Part2(string input)
        => FindColdestRoute(input, 10, 4);

    private static int FindColdestRoute(string input, int maxSteps, int minSteps)
    {
        var start = new Step { Pos = (0, 0), MovesLeft = (maxSteps, maxSteps, maxSteps, maxSteps), MaxSteps = maxSteps, MinSteps = minSteps };
        var queue = new Dictionary<int, List<Step>> { { 0, [start] } };
        for (int i = 1; i < 10; i++)
        {
            queue[i] = [];
        }
        var visited = new Dictionary<Step, bool> { { start, true } };
        var field = ReadLines(input);
        var heat = 0;
        while (true)
        {
            var toEval = queue[heat];
            queue[heat + 10] = [];
            foreach (var step in toEval)
            {
                foreach (var n in step.PossibleMoves().Where(x => !visited.ContainsKey(x)).Where(x => InBounds(field, x.Pos)))
                {
                    var deltaHeat = field[n.Pos.Item1][n.Pos.Item2] - '0';
                    if (n.Pos.Item1 == field.Count - 1 && n.Pos.Item2 == field[0].Length - 1 && n.CanStopOrRotate())
                    {
                        return heat + deltaHeat;
                    }
                    queue[heat + deltaHeat].Add(n);
                    visited[n] = true;
                }
            }
            heat++;
        }
    }

    private static bool InBounds(List<string> lines, (int, int) pos)
        => pos.Item1 >= 0 && pos.Item1 < lines.Count && pos.Item2 >= 0 && pos.Item2 < lines[0].Length;

    public record Step
    {
        public (int, int) Pos;
        public (int, int, int, int) MovesLeft;
        public int MaxSteps;
        public int MinSteps;

        internal bool CanStopOrRotate()
        {
            var sum = MovesLeft.Item1 + MovesLeft.Item2 + MovesLeft.Item3 + MovesLeft.Item4;
            return sum <= (4 * MaxSteps - MinSteps) || sum == 4 * MaxSteps;
        }

        internal IEnumerable<Step> PossibleMoves()
            => new List<Step>
            {
                new() { Pos =(Pos.Item1-1, Pos.Item2), MovesLeft = (MovesLeft.Item1-1,MaxSteps,MaxSteps,MaxSteps), MaxSteps= MaxSteps, MinSteps = MinSteps},
                new() { Pos =(Pos.Item1, Pos.Item2-1), MovesLeft = (MaxSteps,MovesLeft.Item2-1,MaxSteps,MaxSteps), MaxSteps= MaxSteps, MinSteps = MinSteps},
                new() { Pos =(Pos.Item1+1, Pos.Item2), MovesLeft = (MaxSteps,MaxSteps,MovesLeft.Item3-1,MaxSteps), MaxSteps= MaxSteps, MinSteps = MinSteps},
                new() { Pos =(Pos.Item1, Pos.Item2+1), MovesLeft = (MaxSteps,MaxSteps,MaxSteps,MovesLeft.Item4-1), MaxSteps= MaxSteps, MinSteps = MinSteps}
            }.Where(x => x.MovesLeft.Item1 + x.MovesLeft.Item2 + x.MovesLeft.Item3 + x.MovesLeft.Item4 >= 3 * MaxSteps)
             .Where(x => AtLeastMinsteps(x.MovesLeft, MovesLeft))
             .Where(x => NotBackwards(x.MovesLeft, MovesLeft));

        private bool AtLeastMinsteps((int, int, int, int) dest, (int, int, int, int) movesLeft2)
            => CanStopOrRotate()
                || (dest.Item1 < MaxSteps && movesLeft2.Item1 < MaxSteps)
                || (dest.Item2 < MaxSteps && movesLeft2.Item2 < MaxSteps)
                || (dest.Item3 < MaxSteps && movesLeft2.Item3 < MaxSteps)
                || (dest.Item4 < MaxSteps && movesLeft2.Item4 < MaxSteps);

        private bool NotBackwards((int, int, int, int) movesLeft1, (int, int, int, int) movesLeft2)
            => !((movesLeft1.Item1 < MaxSteps && movesLeft2.Item3 < MaxSteps)
                || (movesLeft1.Item3 < MaxSteps && movesLeft2.Item1 < MaxSteps)
                || (movesLeft1.Item4 < MaxSteps && movesLeft2.Item2 < MaxSteps)
                || (movesLeft1.Item2 < MaxSteps && movesLeft2.Item4 < MaxSteps));
    }
}