
namespace AoC2024;

class Day22 : BaseDay
{
    [Example(expected: 37327623, input: "1\n10\n100\n2024")]
    [Puzzle(expected: 17965282217)]
    public static long Part1(string input)
    {
        var lines = ReadLines(input);
        return lines.Sum(x => GenerateSecret(x, 2000));
    }

    private static long GenerateSecret(string line, long iter)
    {
        var ans = long.Parse(line);
        for (long i = 0; i < iter; i++)
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

    private static long BitwiseXor(long v, long literal)
    {
        return v ^ literal;
    }

    [Example(expected: 23, input: "1\n2\n3\n2024")]
    [Puzzle(expected: 2152)]
    public static long Part2(string input)
    {
        var lines = ReadLines(input);
        var changeSequences = lines.Select(GetSequences).ToList();
        var allSequences = changeSequences.SelectMany(x => x.Keys).Distinct().OrderBy(x => x.Item1).ThenBy(x => x.Item2).ThenBy(x => x.Item3).ThenBy(x => x.Item4).ToList();
        var ans = 0;
        foreach (var sequence in allSequences)
        {
            var total = changeSequences.Sum(x => x.TryGetValue(sequence, out var value) ? value : 0);
            if (total > ans)
            {
                ans = total;
            }
        }
        return ans;
    }

    private static Dictionary<(int, int, int, int), int> GetSequences(string line)
    {
        var num = long.Parse(line);
        var prevPrice = num % 10;
        var ans = new Dictionary<(int, int, int, int), int>();
        var diffs = new Queue<int> { };
        for (long i = 0; i < 4; i++)
        {
            num = ApplyMulStep(num, 64);
            num = ApplyDivStep(num);
            num = ApplyMulStep(num, 2048);
            var newPrice = num % 10;
            diffs.Enqueue((int)(newPrice - prevPrice));
            prevPrice = newPrice;
        }
        ans.Add(ToTuple(diffs), (int)prevPrice);
        for (long i = 4; i < 2000; i++)
        {
            num = ApplyMulStep(num, 64);
            num = ApplyDivStep(num);
            num = ApplyMulStep(num, 2048);

            diffs.Dequeue();
            var newPrice = num % 10;
            diffs.Enqueue((int)(newPrice - prevPrice));
            prevPrice = newPrice;
            if (!ans.ContainsKey(ToTuple(diffs)))
            {
                ans.Add(ToTuple(diffs), (int)newPrice);
            }
        }
        return ans;
    }

    private static (int, int, int, int) ToTuple(Queue<int> diffs)
    {
        var arr = diffs.ToArray();
        return (arr[0], arr[1], arr[2], arr[3]);
    }
}