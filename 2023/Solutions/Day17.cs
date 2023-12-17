



namespace AoC2023;

class Day17 : BaseDay
{
    [Example(expected: 102, input: "2413432311323\n3215453535623\n3255245654254\n3446585845452\n4546657867536\n1438598798454\n4457876987766\n3637877979653\n4654967986887\n4564679986453\n1224686865563\n2546548887735\n4322674655533")]
    [Example(expected: 14, input: "41191119\n99111911\n99999994")]
    [Example(expected: 13, input: "41191119\n99111911\n99992114")]
    [Puzzle(expected: 758)]
    public static int Part1(string input)
    {
        var start = new Step { Pos = (0, 0), MovesLeft = (3, 3, 3, 3) };
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
                    if (n.Pos.Item1 == field.Count - 1 && n.Pos.Item2 == field[0].Length - 1) return heat + deltaHeat;
                    queue[heat + deltaHeat].Add(n);
                    visited[n] = true;
                }
            }
            heat++;
        }
    }

    private static bool InBounds(List<string> lines, (int, int) pos)
        => pos.Item1 >= 0 && pos.Item1 < lines.Count && pos.Item2 >= 0 && pos.Item2 < lines[0].Length;

    [Example(expected: 94, input: "2413432311323\n3215453535623\n3255245654254\n3446585845452\n4546657867536\n1438598798454\n4457876987766\n3637877979653\n4654967986887\n4564679986453\n1224686865563\n2546548887735\n4322674655533")]
    [Example(expected: 71, input: "111111111111\n999999999991\n999999999991\n999999999991\n999999999991")]
    [Puzzle(expected: 892)]
    public static int Part2(string input)
    {
        var start = new Step2 { Pos = (0, 0), MovesLeft = (10, 10, 10, 10) };
        var queue = new Dictionary<int, List<Step2>> { { 0, [start] } };
        for (int i = 1; i < 10; i++)
        {
            queue[i] = [];
        }
        var visited = new Dictionary<Step2, bool> { { start, true } };
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
                    if (n.Pos.Item1 == field.Count - 1 && n.Pos.Item2 == field[0].Length - 1 && (n.MovesLeft.Item1 + n.MovesLeft.Item2 + n.MovesLeft.Item3 + n.MovesLeft.Item4 <=36))
                    {
                        return heat + deltaHeat;
                    }
                    queue[heat + deltaHeat].Add(n);
                    visited[n] = true;
                }
            }
            heat++;
        }
        return 0;
    }

    public record Step
    {
        public (int, int) Pos;
        public (int, int, int, int) MovesLeft;

        internal IEnumerable<Step> PossibleMoves()
        {
            var possibleMoves = new List<Step>
            {
                new() {Pos =(Pos.Item1-1, Pos.Item2), MovesLeft = (MovesLeft.Item1-1,3,3,3)},
                new() {Pos =(Pos.Item1, Pos.Item2-1), MovesLeft = (3,MovesLeft.Item2-1,3,3)},
                new() {Pos =(Pos.Item1+1, Pos.Item2), MovesLeft = (3,3,MovesLeft.Item3-1,3)},
                new() {Pos =(Pos.Item1, Pos.Item2+1), MovesLeft = (3,3,3,MovesLeft.Item4-1)}
            };


            return possibleMoves.Where(x => x.MovesLeft.Item1 + x.MovesLeft.Item2 + x.MovesLeft.Item3 + x.MovesLeft.Item4 >= 9).Where(x => NotBackwards(x.MovesLeft, MovesLeft));

        }

        private bool NotBackwards((int, int, int, int) movesLeft1, (int, int, int, int) movesLeft2)
        {
            return !((movesLeft1.Item1 < 3 && movesLeft2.Item3 < 3) || (movesLeft1.Item3 < 3 && movesLeft2.Item1 < 3) || (movesLeft1.Item4 < 3 && movesLeft2.Item2 < 3) || (movesLeft1.Item2 < 3 && movesLeft2.Item4 < 3));
        }
    }

    public record Step2
    {
        public (int, int) Pos;
        public (int, int, int, int) MovesLeft;

        internal IEnumerable<Step2> PossibleMoves()
        {
            var possibleMoves = new List<Step2>
            {
                new() {Pos =(Pos.Item1-1, Pos.Item2), MovesLeft = (MovesLeft.Item1-1,10,10,10)},
                new() {Pos =(Pos.Item1, Pos.Item2-1), MovesLeft = (10,MovesLeft.Item2-1,10,10)},
                new() {Pos =(Pos.Item1+1, Pos.Item2), MovesLeft = (10,10,MovesLeft.Item3-1,10)},
                new() {Pos =(Pos.Item1, Pos.Item2+1), MovesLeft = (10,10,10,MovesLeft.Item4-1)}
            };

            return possibleMoves.Where(x => x.MovesLeft.Item1 + x.MovesLeft.Item2 + x.MovesLeft.Item3 + x.MovesLeft.Item4 >= 30)
                    .Where(x => ContinueAtLeast4(x.MovesLeft, MovesLeft))
                    .Where(x => NotBackwards(x.MovesLeft, MovesLeft));

        }

        private bool ContinueAtLeast4((int, int, int, int) movesLeft1, (int, int, int, int) movesLeft2)
        {
            var sum = movesLeft2.Item1 + movesLeft2.Item2 + movesLeft2.Item3 + movesLeft2.Item4;
            if (sum <= 36 || sum == 40) return true;
            return (movesLeft1.Item1 < 10 && movesLeft2.Item1 < 10) || (movesLeft1.Item2 < 10 && movesLeft2.Item2 < 10) || (movesLeft1.Item3 < 10 && movesLeft2.Item3 < 10) || (movesLeft1.Item4 < 10 && movesLeft2.Item4 < 10);
        }

        private bool NotBackwards((int, int, int, int) movesLeft1, (int, int, int, int) movesLeft2)
        {
            return !((movesLeft1.Item1 < 10 && movesLeft2.Item3 < 10) || (movesLeft1.Item3 < 10 && movesLeft2.Item1 < 10) || (movesLeft1.Item4 < 10 && movesLeft2.Item2 < 10) || (movesLeft1.Item2 < 10 && movesLeft2.Item4 < 10));
        }
    }
}