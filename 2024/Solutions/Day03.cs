namespace AoC2024;

class Day03 : BaseDay
{
    [Example(expected: 161, input: "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))")]
    // [Puzzle(expected: 174336360)]
    public static int Part1(string input)
        => SumMultiplication(string.Join("", ReadLines(input)));

    private static int SumMultiplication(string line)
    {
        var ans = 0;
        var groups = Regex.Matches(line, "mul\\((\\d{1,3}),(\\d{1,3})\\)").ToList();
        foreach (var item in groups)
        {
            var numbers = item.Groups.Values.ToList();
            ans += int.Parse(numbers[1].Value) * int.Parse(numbers[2].Value);
        }
        return ans;
    }

    [Example(expected: 48, input: "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))")]
    // [Puzzle(expected: 88802350)]
    public static int Part2(string input)
        => string.Join("", ReadLines(input))
            .Split("do")
            .Where(item => !item.StartsWith("n't"))
            .Sum(SumMultiplication);
}