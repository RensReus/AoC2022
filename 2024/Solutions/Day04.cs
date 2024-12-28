namespace AoC2024;

class Day04 : BaseDay
{
    [Example(expected: 18, input: "MMMSXXMASM\nMSAMXMSMSA\nAMXSXMAAMM\nMSAMASMSMX\nXMASAMXAMM\nXXAMMXXAMA\nSMSMSASXSS\nSAXAMASAAA\nMAMMMXMMMM\nMXMXAXMASX")]
    [Puzzle(expected: 2685)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);

        // for each horizontal 
        var ans = lines.Sum(CountXmas);

        // for each vertical
        for (int col = 0; col < lines[0].Length; col++)
        {
            string vertical = "";
            for (int row = 0; row < lines.Count; row++)
            {
                vertical += lines[row][col];
            }
            ans += CountXmas(vertical);
        }
        // for each diagonal 1
        for (int startCol = 0; startCol < lines[0].Length; startCol++)
        {
            ans += BuildDiagString1(lines, startCol, 0);
        }

        for (int startRow = 1; startRow < lines.Count; startRow++)
        {
            ans += BuildDiagString1(lines, lines[0].Length - 1, startRow);
        }

        // for each diagonal 2
        for (int startCol = 0; startCol < lines[0].Length; startCol++)
        {
            ans += BuildDiagString2(lines, startCol, 0);
        }

        for (int startRow = 1; startRow < lines.Count; startRow++)
        {
            ans += BuildDiagString2(lines, 0, startRow);
        }

        return ans;
    }

    public static int BuildDiagString1(List<string> letters, int col, int row)
    {
        string diagonal = "";
        while (col >= 0 && row < letters.Count)
        {
            diagonal += letters[row][col];
            col--;
            row++;
        }

        return CountXmas(diagonal);
    }

    public static int BuildDiagString2(List<string> letters, int col, int row)
    {
        string diagonal = "";
        while (col < letters[0].Length && row < letters.Count)
        {
            diagonal += letters[row][col];
            col++;
            row++;
        }
        return CountXmas(diagonal);
    }

    public static int CountXmas(string s)
    {
        return Regex.Matches(s, "XMAS").Count + Regex.Matches(s, "SAMX").Count;
    }

    [Example(expected: 9, input: ".M.S......\n..A..MSMS.\n.M.S.MAA..\n..A.ASMSM.\n.M.S.M....\n..........\nS.S.S.S.S.\n.A.A.A.A..\nM.M.M.M.M.\n..........")]
    [Puzzle(expected: 2048)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input);

        var ans2 = 0;
        for (int row = 0; row < lines.Count - 2; row++)
        {
            for (int col = 0; col < lines[0].Length - 2; col++)
            {
                if (CrossMas(lines, row, col)) ans2 += 1;
            }
        }

        return ans2;
    }

    public static bool CrossMas(List<string> letters, int row, int col)
    {
        var diag1 = new string([letters[row][col], letters[row + 1][col + 1], letters[row + 2][col + 2]]);
        var diag2 = new string([letters[row + 2][col], letters[row + 1][col + 1], letters[row][col + 2]]);

        return (diag1 is "MAS" or "SAM") && (diag2 is "MAS" or "SAM");
    }
}