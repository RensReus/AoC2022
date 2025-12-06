
namespace AoC2025;

class Day06 : BaseDay
{
    [Example(expected: 4277556, input: "123 328  51 64\n 45 64  387 23 \n  6 98  215 314\n*   +   *   +  ")]
    [Puzzle(expected: 4951502530386)]
    public static long Part1(string input)
    {
        var lines = ReadLines(input).Select(x => x.Split().Where(s => !string.IsNullOrWhiteSpace(s)).ToArray());
        var operators = lines.Last().ToArray();
        var ans = 0L;
        for (int i = 0; i < lines.First().Length; i++)
        {
            if (operators[i] == "*")
            {
                var subans = 1L;
                for (int j = 0; j < lines.Count() - 1; j++)
                {
                    subans *= long.Parse(lines.ElementAt(j)[i]);
                }
                ans += subans;
            }
            else
            {
                var subans = 0L;
                for (int j = 0; j < lines.Count() - 1; j++)
                {
                    subans += long.Parse(lines.ElementAt(j)[i]);
                }
                ans += subans;
            }
        }
        return ans;
    }


    [Example(expected: 3263827, input: "123 328  51 64 \n 45 64  387 23 \n  6 98  215 314\n*   +   *   +  ")]
    [Puzzle(expected: 8486156119946)]
    public static decimal Part2(string input)
    {
        var lines = ReadLines(input);
        var numLines = lines.Take(lines.Count - 1);
        var operators = lines.Last();
        var ans = 0m;
        var currOperator = ' ';
        var numbers = new List<decimal>();
        for (int i = 0; i < operators.Length; i++)
        {
            if (operators[i] is not ' ')
            {
                if (currOperator == '*')
                {
                    ans += numbers.Aggregate(1m, (acc, val) => acc * val);
                }
                else if (currOperator == '+')
                {
                    ans += numbers.Sum();
                }
                numbers.Clear();
                currOperator = operators[i];
            }
            var numString = numLines.Select(line => line[i]).Where(c => c is not ' ').ToArray();
            if (numString.Length > 0)
            {
                var num = decimal.Parse(new string(numString));
                numbers.Add(num);
            }
        }
        if (currOperator == '*')
        {
            var subans = numbers.Aggregate(1m, (acc, val) => acc * val);
            ans += subans;
        }
        else if (currOperator == '+')
        {
            var subans = numbers.Sum();
            ans += subans;
        }
        return ans;
    }
}