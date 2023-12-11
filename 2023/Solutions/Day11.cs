namespace AoC2023;

class Day11 : BaseDay
{
    [Example(expected: 374, input: "...#......\n.......#..\n#.........\n..........\n......#...\n.#........\n.........#\n..........\n.......#..\n#...#.....")]
    [Puzzle(expected: 9556712)]
    public static long Part1(string input)
        => CalcDistances(input, 2);

    [Puzzle(expected: 678626199476)]
    public static long Part2(string input)
        => CalcDistances(input, 1000000);


    private static long CalcDistances(string input, int bonus)
    {
        var universe = ReadLines(input);
        var galaxies = new List<(int, int)>();
        for (int row = 0; row < universe.Count; row++)
        {
            for (int col = 0; col < universe[0].Length; col++)
            {
                if (universe[row][col] == '#') galaxies.Add((row, col));
            }
        }
        var rowsWithout = universe.Select((x, i) => (x, i)).Where(x => !x.x.Contains('#')).Select(x => x.i);
        var colsWithout = new List<int>();

        for (int col = 0; col < universe[0].Length; col++)
        {
            var without = true;
            for (int row = 0; row < universe.Count; row++)
            {
                if (universe[row][col] == '#')
                {
                    without = false;
                    break;
                }
            }
            if (without) colsWithout.Add(col);
        }

        var answer = 0L;
        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                var distanceRow = Math.Abs(galaxies[i].Item1 - galaxies[j].Item1);
                var distanceCol = Math.Abs(galaxies[i].Item2 - galaxies[j].Item2);
                var bonusRow = rowsWithout.Count(row => row > Math.Min(galaxies[i].Item1, galaxies[j].Item1) && row < Math.Max(galaxies[i].Item1, galaxies[j].Item1)) * (bonus - 1L);
                var bonusCol = colsWithout.Count(col => col > Math.Min(galaxies[i].Item2, galaxies[j].Item2) && col < Math.Max(galaxies[i].Item2, galaxies[j].Item2)) * (bonus - 1L);
                answer += distanceRow + distanceCol + bonusRow + bonusCol;
            }
        }
        return answer;
    }
}