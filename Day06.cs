using AoC2022.Days;

namespace AoC2022.Days06;

class Day : BaseDay
{
    static String ProcessInput(string filename)
        => ReadFile("06/" + filename).First();

    public override int Part1(string filename)
        => FirstUniqueLetterSegmentEnd(ProcessInput(filename), 4);

    public override int Part2(string filename)
        => FirstUniqueLetterSegmentEnd(ProcessInput(filename), 14);

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

    public override List<Case> Part1Cases() => new() { new("1a", 5), new("1b", 6), new("1c", 10), new("1d", 11), new("1e", 7), new("p1", 1566) };

    public override List<Case> Part2Cases() => new() { new("1a", 23), new("1b", 23), new("1c", 29), new("1d", 26), new("1e", 19), new("p1", 2265) };
}