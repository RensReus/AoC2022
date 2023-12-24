
namespace AoC2023;

class Day21 : BaseDay
{
    [Puzzle(expected: 3671)]
    public static long Part1(string input)
    {
        var lines = ReadLines(input);
        var startRow = lines.FindIndex(x => x.Contains('S'));
        var startCol = lines[startRow].IndexOf('S');
        return CountPossibleFields(startRow, startCol, lines, 64, true);
    }

    private static long CountPossibleFields(int startRow, int startCol, List<string> lines, int limit, bool even, long lastStep = 0)
    {
        var visitedEven = new Dictionary<(int, int), bool>();
        var visitedOdd = new Dictionary<(int, int), bool>();
        var toEval = new List<(int, int)> { (startRow, startCol) };
        var size = lines.Count;
        for (long i = 1 + lastStep; i <= limit; i++)
        {
            var nextToEval = new List<(int, int)>();
            foreach (var item in toEval)
            {
                foreach (var n in GetNeighbours(item))
                {
                    if (n.Item1 < 0 || n.Item1 >= size || n.Item2 < 0 || n.Item2 >= size || lines[n.Item1][n.Item2] == '#') continue;
                    if (i % 2 == 0 && !visitedEven.ContainsKey(n))
                    {
                        visitedEven[n] = true;
                        nextToEval.Add(n);
                    }
                    if (i % 2 != 0 && !visitedOdd.ContainsKey(n))
                    {
                        visitedOdd[n] = true;
                        nextToEval.Add(n);
                    }
                }
            }
            toEval = nextToEval;
        }
        Console.WriteLine(visitedEven.Last());
        return even ? visitedEven.Count : visitedOdd.Count;
    }

    private static List<(int, int)> GetNeighbours((int, int) item)
        => [(item.Item1 + 1, item.Item2), (item.Item1 - 1, item.Item2), (item.Item1, item.Item2 - 1), (item.Item1, item.Item2 + 1)];

    [Puzzle(expected: 610275506007391)] // too high  609701953729856 // too low because missing non complete tiles
    public static long Part2(string input)
    {
        var lines = ReadLines(input);
        var targetSteps = 26501365;
        var visitedEven = new Dictionary<(int, int), bool>();
        var visitedOdd = new Dictionary<(int, int), bool>();
        var startRow = lines.FindIndex(x => x.Contains('S'));
        var startCol = lines[startRow].IndexOf('S');
        var gridwidth = lines.Count;
        int totalGridSteps = targetSteps / gridwidth;
        int remainingSteps = targetSteps % gridwidth;
        var totalOddGrids = 1L;
        var totalEvenGrids = 0L;
        Console.WriteLine(gridwidth);
        Console.WriteLine(remainingSteps);
        for (int i = 1; i < totalGridSteps; i++) // Kan met formule
        {
            if (i % 2 == 0) totalOddGrids += i * 4;
            else totalEvenGrids += i * 4;
        }
        var visitableInEvenGrid = CountPossibleFields(startRow, startCol, lines, gridwidth, true);
        var visitableInOddGrid = CountPossibleFields(startRow, startCol, lines, gridwidth, false);
        var countCompletedGrids = visitableInEvenGrid * totalOddGrids + visitableInOddGrid * totalEvenGrids;

        //var bonusSteps = remainingSteps + gridwidth / 2;
        //var leftBonus = CountPossibleFields(startRow, 0, lines, bonusSteps, true);
        //var rightBonus = CountPossibleFields(startRow, gridwidth, lines, bonusSteps, true);
        //var topBonus = CountPossibleFields(0, startCol, lines, bonusSteps, true);
        //var bottomBonus = CountPossibleFields(gridwidth, startCol, lines, bonusSteps, true);
        //var topRight = CountPossibleFields(0, gridwidth, lines, remainingSteps, true);
        //var topLeft = CountPossibleFields(0, 0, lines, remainingSteps, true);
        //var bottomRight = CountPossibleFields(gridwidth, gridwidth, lines, remainingSteps, true);
        //var bottomLeft = CountPossibleFields(gridwidth, 0, lines, remainingSteps, true);
        return countCompletedGrids
            + StraightBonus(targetSteps - remainingSteps, totalGridSteps % 2 == 0, startRow, startCol, lines, targetSteps, gridwidth)
            + DiagonalBonus();
        // + leftBonus + rightBonus + topBonus + bottomBonus
        // + (topRight + topLeft + bottomRight + bottomLeft) * (totalGridSteps - 2);
    }

    private static long DiagonalBonus()
    {
        // 2 soorten diagonals
        throw new NotImplementedException();
    }

    private static long StraightBonus(long stepsMade, bool isEven, int startRow, int startCol, List<string> lines, int targetSteps, int gridwidth)
    {
        var hoeveelsnellerEigenlijk = gridwidth / 2;
        var lastStep = stepsMade - hoeveelsnellerEigenlijk;
        if (targetSteps - lastStep > gridwidth) throw new Exception("moet nog een 4 extra grids meepakken");
        var leftBonus = CountPossibleFields(startRow, 0, lines, targetSteps, isEven, lastStep);
        var rightBonus = CountPossibleFields(startRow, gridwidth, lines, targetSteps, isEven, lastStep);
        var topBonus = CountPossibleFields(0, startCol, lines, targetSteps, isEven, lastStep);
        var bottomBonus = CountPossibleFields(gridwidth, startCol, lines, targetSteps, isEven, lastStep);

        return leftBonus + rightBonus + topBonus + bottomBonus;
    }
}