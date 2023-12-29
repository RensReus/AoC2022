
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

    private static long CountPossibleFields(int startRow, int startCol, List<string> lines, int limit, bool even)
    {
        var visitedEven = new Dictionary<(int, int), bool>();
        var visitedOdd = new Dictionary<(int, int), bool>();
        var toEval = new List<(int, int)> { (startRow, startCol) };
        var size = lines.Count;
        for (long i = 1; i <= limit; i++)
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
        return even ? visitedEven.Count : visitedOdd.Count;
    }

    private static List<(int, int)> GetNeighbours((int, int) item)
        => [(item.Item1 + 1, item.Item2), (item.Item1 - 1, item.Item2), (item.Item1, item.Item2 - 1), (item.Item1, item.Item2 + 1)];

    [Puzzle(expected: 609708004316870)]
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
        var totalEvenGrids = 1L;
        var totalOddGrids = 0L;
        for (int i = 0; i < totalGridSteps; i++) // Kan wss met formule
        {
            if (i % 2 == 0) totalEvenGrids += i * 4;
            else totalOddGrids += i * 4;
        }
        var visitableInEvenGrid = CountPossibleFields(startRow, startCol, lines, gridwidth, false);
        var visitableInOddGrid = CountPossibleFields(startRow, startCol, lines, gridwidth, true);

        var countCompletedGrids = visitableInEvenGrid * totalEvenGrids + visitableInOddGrid * totalOddGrids;

        return countCompletedGrids
            + StraightBonus(remainingSteps, totalGridSteps % 2 == 0, startRow, startCol, lines, gridwidth)
            + DiagonalBonus(gridwidth, lines, remainingSteps, totalGridSteps);
    }

    private static long DiagonalBonus(int gridwidth, List<string> lines, int remainingSteps, int totalGridSteps)
        => DiagonalBonusEven(gridwidth, lines, remainingSteps - 1, totalGridSteps) + DiagonalBonusOdd(gridwidth, lines, remainingSteps, totalGridSteps);

    private static long DiagonalBonusOdd(int gridwidth, List<string> lines, int remainingSteps, int totalGridSteps)
    {
        var evenGridSteps = totalGridSteps % 2 == 0;
        var bonusSteps = !evenGridSteps ? gridwidth : 0;
        var actualRemainingSteps = remainingSteps + bonusSteps;
        var topRight = CountPossibleFields(0, gridwidth - 1, lines, actualRemainingSteps, true);
        var topLeft = CountPossibleFields(0, 0, lines, actualRemainingSteps, true);
        var bottomRight = CountPossibleFields(gridwidth - 1, gridwidth - 1, lines, actualRemainingSteps, true);
        var bottomLeft = CountPossibleFields(gridwidth - 1, 0, lines, actualRemainingSteps, true);

        var oddDiagonalCount = !evenGridSteps ? totalGridSteps - 1 : totalGridSteps;

        return (topRight + topLeft + bottomLeft + bottomRight) * oddDiagonalCount;
    }

    private static long DiagonalBonusEven(int gridwidth, List<string> lines, int remainingSteps, int totalGridSteps)
    {
        var evenGridSteps = totalGridSteps % 2 == 0;
        var bonusSteps = evenGridSteps ? gridwidth : 0;
        var actualRemainingSteps = remainingSteps + bonusSteps;
        var topRight = CountPossibleFields(0, gridwidth - 1, lines, actualRemainingSteps, false);
        var topLeft = CountPossibleFields(0, 0, lines, actualRemainingSteps, false);
        var bottomRight = CountPossibleFields(gridwidth - 1, gridwidth - 1, lines, actualRemainingSteps, false);
        var bottomLeft = CountPossibleFields(gridwidth - 1, 0, lines, actualRemainingSteps, false);

        var evenDiagonalCount = evenGridSteps ? totalGridSteps - 1 : totalGridSteps;
        return (topRight + topLeft + bottomLeft + bottomRight) * evenDiagonalCount;
    }

    private static long StraightBonus(int remainingSteps, bool isEven, int startRow, int startCol, List<string> lines, int gridwidth)
    {
        var hoeveelsnellerEigenlijk = gridwidth / 2;
        var actualRemainingSteps = remainingSteps + hoeveelsnellerEigenlijk;
        if (actualRemainingSteps >= gridwidth) throw new Exception("moet nog een 4 extra grids meepakken");
        var leftBonus = CountPossibleFields(startRow, 0, lines, actualRemainingSteps, isEven);
        var rightBonus = CountPossibleFields(startRow, gridwidth - 1, lines, actualRemainingSteps, isEven);
        var topBonus = CountPossibleFields(0, startCol, lines, actualRemainingSteps, isEven);
        var bottomBonus = CountPossibleFields(gridwidth - 1, startCol, lines, actualRemainingSteps, isEven);

        return leftBonus + rightBonus + topBonus + bottomBonus;
    }
}