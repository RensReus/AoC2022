namespace AoC2022;

static class Day25
{
    [Example(expected: "2=-1=0", input: "1=-0-2;12111;2=0=;21;2=01;111;20012;112;1=-1=;1-12;12;1=;122")]
    [Puzzle(expected: "2-02===-21---2002==0")]
    public static string Part1(string input)
     => ToSnafu(input.Split(";").Select(ToDecimal).Sum());

    private static long ToDecimal(string x)
        => x.Reverse().Select((x, i) => (long)GetDecimalValue(x, i)).Sum();

    private static double GetDecimalValue(char x, int i)
        => GetDiff(x) * Math.Pow(5, i);

    private static long GetDiff(char newDigit)
        => newDigit switch
        {
            '1' => 1,
            '2' => 2,
            '-' => -1,
            '=' => -2,
            _ => 0,
        };

    private static string ToSnafu(long sum)
    {
        var snafu = "";
        var power = 0;
        while (sum != 0)
        {
            var newDigit = (sum / Math.Pow(5, power) % 5) switch
            {
                1 => '1',
                2 => '2',
                3 => '=',
                4 => '-',
                _ => '0',
            };
            snafu += newDigit;
            sum -= (long)(GetDiff(newDigit) * Math.Pow(5, power));
            power++;
        }
        return new(snafu.Reverse().ToArray());
    }
}