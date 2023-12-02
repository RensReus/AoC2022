namespace AoC2022;

class Day08 : BaseDay
{
    [Example(expected: 21, input: "30373\n25512\n65332\n33549\n35390")]
    [Puzzle(expected: 1776)]
    public int Part1(string input)
    {
        var processedInput = ReadLines(input);
        var height = processedInput.Count();
        var width = processedInput[0].Count();
        int answer = height * 2 + width * 2 - 4; // edges minus double counted corners
        for (int row = 1; row < height - 1; row++)
        {
            for (int col = 1; col < width - 1; col++)
            {
                if (TreeVisible(processedInput, row, col))
                {
                    answer++;
                }
            }
        }
        return answer;
    }

    [Example(expected: 8, input: "30373\n25512\n65332\n33549\n35390")]
    [Puzzle(expected: 234416)]
    public int Part2(string input)
    {
        var processedInput = ReadLines(input);
        int answer = 0;
        for (int row = 1; row < processedInput.Count() - 1; row++)
        {
            for (int col = 1; col < processedInput[0].Count() - 1; col++)
            {
                answer = int.Max(answer, ScenicScore(processedInput, row, col));
            }
        }
        return answer;
    }

    private bool TreeVisible(IList<string> input, int row, int col)
    {
        var height = input[row][col];
        var horizon = input[row];
        var vertica = input.Select(x => x[col]).ToArray();
        return horizon[0..col].All(x => x < height)
            || vertica[0..row].All(x => x < height)
            || horizon[(col + 1)..].All(x => x < height)
            || vertica[(row + 1)..].All(x => x < height);
    }

    private int ScenicScore(IList<string> input, int row, int col)
    {
        var height = input[row][col];
        var horizon = input[row];
        var rolCount = input.Count();
        var vertica = input.Select(x => x[col]).ToArray();
        var lefpos = horizon[0..col].ToList().FindLastIndex(x => x >= height);
        var toppos = vertica[0..row].ToList().FindLastIndex(x => x >= height);
        var rightposRel = horizon[(col + 1)..].ToList().FindIndex(x => x >= height);
        var bottoposRel = vertica[(row + 1)..].ToList().FindIndex(x => x >= height);
        return (col - (lefpos == -1 ? 0 : lefpos))
            * (row - (toppos == -1 ? 0 : toppos))
            * (rightposRel == -1 ? rolCount - col - 1 : rightposRel + 1)
            * (bottoposRel == -1 ? rolCount - row - 1 : bottoposRel + 1);
    }
}