using AoC2022.Days;

namespace AoC2022.Days08;

class Day : BaseDay
{
    static IList<String> ProcessInput(string filename)
        => ReadFile("08/" + filename);

    public override int Part1(string filename)
    {
        var input = ProcessInput(filename).ToList();
        var height = input.Count();
        var width = input[0].Count();
        int answer = height * 2 + width * 2 - 4; // edges minus double counted corners
        for (int row = 1; row < height - 1; row++)
        {
            for (int col = 1; col < width - 1; col++)
            {
                if (TreeVisible(input, row, col))
                {
                    answer++;
                }
            }
        }
        return answer;
    }

    public override int Part2(string filename)
    {
        var input = ProcessInput(filename);
        int answer = 0;
        for (int row = 1; row < input.Count() - 1; row++)
        {
            for (int col = 1; col < input[0].Count() - 1; col++)
            {
                answer = int.Max(answer, ScenicScore(input, row, col));
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

    public override List<Case> Part1Cases() => new() { new("1a", 21), new("p1", 1776) };

    public override List<Case> Part2Cases() => new() { new("1a", 8), new("p1", 234416) };
}