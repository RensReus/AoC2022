

using NUnit.Framework;

namespace AoC2025;

class Day02 : BaseDay
{
    private static string[] ParseLine(string line)
    {
        return line.Split();
    }

    [Example(expected: 1227775554, input: "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124")]
    [Puzzle(expected: 38437576669)]
    public static long Part1(string input)
    {
        Console.WriteLine("--------------------------------");
        var ranges = ReadLines(input).Single().Split(',').Select(x => (x.Split('-')[0], x.Split('-')[1]));
        var answer = 0L;
        foreach (var (startStr, endStr) in ranges)
        {
            answer += MyFunct(startStr, endStr, 2).Sum();
        }

        return answer;
    }

    private static long[] MyFunct(string startStr, string endStr, int divisions)
    {
        var answer = Array.Empty<long>();
        var a = startStr.Length % divisions;
        var b = endStr.Length % divisions;
        if (startStr.Length % divisions != 0 && endStr.Length % divisions != 0) return [];
        var (startStr2, endStr2) = (startStr, endStr);
        if (startStr.Length != endStr.Length && startStr.Length % divisions != 0)
        {
            startStr2 = "1" + new string('0', startStr.Length);
        }
        if (startStr2.Length != endStr2.Length && endStr.Length % divisions != 0)
        {
            endStr2 = new string('9', startStr.Length);
        }
        var size = startStr2.Length / divisions;

        var startParts = new List<long>();
        var endParts = new List<long>();
        for (int strt = 0; strt < startStr2.Length; strt += size)
        {
            startParts.Add(long.Parse(startStr2.Substring(strt, size)));
            endParts.Add(long.Parse(endStr2.Substring(strt, size)));
        }

        var lowerBound = startParts[0] >= startParts[1] ? startParts[0] : startParts[0] + 1;
        var upperBound = endParts[0] <= endParts[1] ? endParts[0] : endParts[0] - 1;
        if (lowerBound <= upperBound)
        {
            for (long i = lowerBound; i <= upperBound; i++)
            {
                var ansString = "";
                for (int part = 0; part < divisions; part++)
                {
                    ansString += i.ToString();
                }
                var ansNum = long.Parse(ansString);
                if (ansNum < long.Parse(startStr) || ansNum > long.Parse(endStr))
                {
                    continue;
                }
                answer = answer.Append(long.Parse(ansString)).ToArray();
            }
        }
        return answer;
    }

    [Example(expected: 4174379265, input: "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124")]
    [Puzzle(expected: 49046150754)]
    public static long Part2(string input)
    {
        var ranges = ReadLines(input).Single().Split(',').Select(x => (x.Split('-')[0], x.Split('-')[1]));
        var answer = Array.Empty<long>();
        foreach (var (startStr, endStr) in ranges)
        {
            Console.WriteLine($"\nProcessing range: {startStr}-{endStr}");
            for (int divisions = 2; divisions <= endStr.Length; divisions++)
            {
                var subans = MyFunct(startStr, endStr, divisions);
                Console.WriteLine($"Divisions: {divisions}, Found: {string.Join(", ", subans)}");
                answer = answer.Concat(subans).ToArray();
            }
        }

        return answer.Distinct().Sum();
    }
}