
namespace AoC2024;

class Day02 : BaseDay
{
    [Example(expected: 2, input: "7 6 4 2 1\n1 2 7 8 9\n9 7 6 2 1\n1 3 2 4 5\n8 6 4 4 1\n1 3 6 7 9")]
    [Puzzle(expected: 411)]
    public static int Part1(string input)
        => ReadLines(input).Count(IsValid);

    private static bool IsValid(string arg)
    {
        var entries = arg.Split().Select(int.Parse).ToArray();
        var sign = Math.Sign(entries[0] - entries[1]);
        for (int i = 0; i < entries.Length - 1; i++)
        {
            var diff = entries[i] - entries[i + 1];
            if (Math.Abs(diff) is < 1 or > 3 || Math.Sign(diff) != sign) return false;
        }
        return true;
    }

    [Example(expected: 4, input: "7 6 4 2 1\n1 2 7 8 9\n9 7 6 2 1\n1 3 2 4 5\n8 6 4 4 1\n1 3 6 7 9")]
    [Puzzle(expected: 465)]
    public static int Part2(string input)
        => ReadLines(input).Count(IsValidDamp);

    private static bool IsValidDamp(string arg)
    {
        if (IsValid(arg)) return true;
        var entryCount = arg.Split().Length;

        for (int i = 0; i < entryCount; i++)
        {
            var entries = arg.Split().Select(int.Parse).ToList();
            entries.RemoveAt(i);
            if (IsValid(string.Join(" ", entries))) return true;
        }
        return false;
    }
}