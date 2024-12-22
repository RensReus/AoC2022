namespace AoC2024;

class Day22 : BaseDay
{
    [Example(expected: 37327623, input: "1\n10\n100\n2024")]
    [Puzzle(expected: 17965282217)]
    public static long Part1(string input)
        => ReadLines(input).Sum(GenerateSecret);

    private static long GenerateSecret(string line)
    {
        var ans = long.Parse(line);
        for (long i = 0; i < 2000; i++)
        {
            ans = ApplySteps(ans);
        }
        return ans;
    }

    private static long ApplySteps(long ans)
    {
        ans = ApplyMulStep(ans, 64);
        ans = ApplyDivStep(ans);
        return ApplyMulStep(ans, 2048);
    }

    private static long ApplyMulStep(long ans, long v)
    {
        var newAns = ans * v;
        newAns = BitwiseXor(newAns, ans);
        return newAns % 16777216;
    }

    private static long ApplyDivStep(long ans)
    {
        var newAns = ans / 32;
        newAns = BitwiseXor(newAns, ans);
        return newAns % 16777216;
    }

    private static long BitwiseXor(long a, long b)
    {
        return a ^ b;
    }

    [Example(expected: 23, input: "1\n2\n3\n2024")]
    [Puzzle(expected: 2152)]
    public static long Part2(string input)
    {
        var totals = new Dictionary<(int, int, int, int), int>();
        foreach (var line in ReadLines(input))
        {
            AddToTotals(totals, line);
        }
        return totals.Values.Max();
    }

    private static void AddToTotals(Dictionary<(int, int, int, int), int> totals, string line)
    {
        var alreadyAdded = new HashSet<(int, int, int, int)>();
        var num = long.Parse(line);
        var prevPrice = num % 10;
        var diffs = (-10, -10, -10, -10);
        for (int i = 0; i < 2000; i++)
        {
            num = ApplySteps(num);
            var newPrice = num % 10;
            diffs = (diffs.Item2, diffs.Item3, diffs.Item4, (int)(newPrice - prevPrice));
            TryAddToTotal(totals, diffs, newPrice, alreadyAdded);
            prevPrice = newPrice;
        }
    }

    private static void TryAddToTotal(Dictionary<(int, int, int, int), int> totals, (int, int, int, int) diffs, long prevPrice, HashSet<(int, int, int, int)> alreadyAdded)
    {
        if (alreadyAdded.Contains(diffs) || diffs.Item1 == -10) return;
        if (!totals.ContainsKey(diffs))
        {
            totals[diffs] = 0;
        }
        totals[diffs] += (int)prevPrice;
        alreadyAdded.Add(diffs);
    }
}