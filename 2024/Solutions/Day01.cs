namespace AoC2024;

class Day01 : BaseDay
{
    [Example(expected: 11, input: "3   4\n4   3\n2   5\n1   3\n3   9\n3   3")]
    [Puzzle(expected: 1879048)]
    public static int Part1(string input)
    {
        var (list1, list2) = ParseInput(input);
        list1.Sort();
        list2.Sort();
        return list1.Zip(list2, (x, y) => Math.Abs(x - y)).Sum();
    }

    [Example(expected: 31, input: "3   4\n4   3\n2   5\n1   3\n3   9\n3   3")]
    [Puzzle(expected: 21024792)]
    public static int Part2(string input)
    {
        var (list1, list2) = ParseInput(input);
        return list1.Sum(x => x * list2.Count(x2 => x2 == x));
    }

    private static (List<int> list1, List<int> list2) ParseInput(string input)
    {
        var lines = ReadLines(input);
        var list1 = new List<int>();
        var list2 = new List<int>();
        foreach (var line in lines)
        {
            var numbers = line.Split().Select(int.Parse);
            list1.Add(numbers.First());
            list2.Add(numbers.Last());
        }
        return (list1, list2);
    }
}