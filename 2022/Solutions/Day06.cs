namespace AoC2022;

class Day06
{
    [Example(expected: 7, input: "mjqjpqmgbljsphdztnvjfqwrcgsmlb")]
    [Example(expected: 5, input: "bvwbjplbgvbhsrlpgdmjqwftvncz")]
    [Example(expected: 6, input: "nppdvjthqldpwncqszvftbrmjlhg")]
    [Example(expected: 10, input: "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg")]
    [Example(expected: 11, input: "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw")]
    [Puzzle(expected: 1566)]
    public int Part1(string input)
        => FirstUniqueLetterSegmentEnd(input, 4);

    [Example(expected: 19, input: "mjqjpqmgbljsphdztnvjfqwrcgsmlb")]
    [Example(expected: 23, input: "bvwbjplbgvbhsrlpgdmjqwftvncz")]
    [Example(expected: 23, input: "nppdvjthqldpwncqszvftbrmjlhg")]
    [Example(expected: 29, input: "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg")]
    [Example(expected: 26, input: "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw")]
    [Puzzle(expected: 2265)]
    public int Part2(string input)
        => FirstUniqueLetterSegmentEnd(input, 14);

    public int FirstUniqueLetterSegmentEnd(string input, int size)
    {
        for (int i = 0; i < input.Length - size + 1; i++)
        {
            if (AllLettersDifferent(input.Substring(i, size)))
            {
                return i + size;
            }
        }
        return 0;
    }

    private bool AllLettersDifferent(string v)
    {
        for (int i = 0; i < v.Length - 1; i++)
        {
            if (v.Substring(i + 1).Contains(v[i]))
            {
                return false;
            }
        }
        return true;
    }
}