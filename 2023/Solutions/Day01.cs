namespace AoC2023;

class Day01 : BaseDay
{
    private static readonly string[] NumberStrings = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

    [Example(expected: 142, input: "1abc2\r\npqr3stu8vwx\r\na1b2c3d4e5f\r\ntreb7uchet")]
    [Puzzle(expected: 54338)]
    public static int Part1(string input)
        => ReadLines(input)
            .Sum(line => int.Parse(string.Join("", line.First(Is1To9), line.Last(Is1To9))));

    [Example(expected: 281, input: "two1nine\r\neightwothree\r\nabcone2threexyz\r\nxtwone3four\r\n4nineeightseven2\r\nzoneight234\r\n7pqrstsixteen")]
    [Puzzle(expected: 53389)]
    public static int Part2(string input)
    {
        var output = 0;
        foreach (var line in ReadLines(input))
        {
            var firstNum = line.FirstOrDefault(Is1To9);
            var firstNumIndex = line.Contains(firstNum) ? line.IndexOf(firstNum) : int.MaxValue;
            var firstStringNum = FirstStringNum(line);
            var firstStringNumIndex = line.Contains(firstStringNum) ? line.IndexOf(firstStringNum) : int.MaxValue;
            var firstChar = firstNumIndex < firstStringNumIndex ? firstNum : (char)(Array.IndexOf(NumberStrings, firstStringNum) + 49);

            var lastNum = line.LastOrDefault(Is1To9);
            var lastNumIndex = line.Contains(lastNum) ? line.LastIndexOf(lastNum) : int.MinValue;
            var lastStringNum = LastStringNum(line);
            var lastStringNumIndex = line.Contains(lastStringNum) ? line.LastIndexOf(lastStringNum) : int.MinValue;
            var lastChar = lastNumIndex > lastStringNumIndex ? lastNum : (char)(Array.IndexOf(NumberStrings, lastStringNum) + 49);

            output += int.Parse(string.Join("", firstChar, lastChar));
        }
        return output;
    }

    private static bool Is1To9(char x) => x >= 48 && x <= 57;

    private static string FirstStringNum(string line)
    {
        var first = int.MaxValue;
        var firstValue = "aaaaaaaaaaaa";
        foreach (var item in NumberStrings)
        {
            if (line.Contains(item))
            {
                if (line.IndexOf(item) < first)
                {
                    first = line.IndexOf(item);
                    firstValue = item;
                }
            }
        }
        return firstValue;
    }

    private static string LastStringNum(string line)
    {
        var last = int.MinValue;
        var lastValue = "aaaaaaaaaaa";
        foreach (var item in NumberStrings)
        {
            if (line.Contains(item))
            {
                if (line.LastIndexOf(item) > last)
                {
                    last = line.LastIndexOf(item);
                    lastValue = item;
                }
            }
        }
        return lastValue;
    }
}