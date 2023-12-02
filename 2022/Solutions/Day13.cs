namespace AoC2022;

class Day13 : BaseDay
{
    [Example(expected: 13, input: 1)]
    [Puzzle(expected: 6240)]
    public int Part1(string input)
        => input.Split("\n\n").Select((pair, index) => (Compare(pair.Split("\n")[0], pair.Split("\n")[1]), index))
            .Sum(x => x.Item1 != Outcome.Larger ? x.index + 1 : 0);

    public Outcome Compare(string left, string right)
    {
        var leftItems = SplitAtTopLevel(left);
        var rightItems = SplitAtTopLevel(right);
        for (int i = 0; i < leftItems.Count(); i++)
        {
            if (rightItems.Count() <= i)
            {
                return Outcome.Larger;
            }
            var outcome = PairIsCorrect(leftItems[i], rightItems[i]);
            if (outcome is Outcome.Equal) continue;

            return outcome;
        }
        return leftItems.Count() == rightItems.Count() ? Outcome.Equal : Outcome.Smaller;
    }

    public List<string> SplitAtTopLevel(string v)
    {
        if (v[1..^1] == "") { return new(); }
        var depth = 0;
        var output = new List<string>();
        var currBlock = "";
        foreach (var c in v[1..^1])
        {
            if (c == ',' && depth == 0)
            {
                output.Add(currBlock);
                currBlock = "";
            }
            else
            {
                currBlock += c;
            }
            if (c == '[') { depth++; }
            if (c == ']') { depth--; }
        }
        output.Add(currBlock);
        return output;
    }

    public Outcome PairIsCorrect(string left, string right)
        => (left[0] == '[', right[0] == '[') switch
        {
            (true, false) => Compare(left, $"[{right}]"),
            (false, true) => Compare($"[{left}]", right),
            (true, true) => Compare(left, right),
            _ => LeftIsSmaller(int.Parse(left), int.Parse(right)),
        };

    public Outcome LeftIsSmaller(int left, int right)
    {
        if (left > right)
        {
            return Outcome.Larger;
        }
        if (left < right) return Outcome.Smaller;
        return Outcome.Equal;
    }

    [Example(expected: 140, input: 1)]
    [Puzzle(expected: 23142)]
    public int Part2(string input)
    {
        MyComparer comparer = new MyComparer();
        var extraPackets = new string[2] { "[[2]]", "[[6]]" };
        var lines = input.Replace("\n\n", "\n").Split("\n").ToList();
        lines.AddRange(extraPackets);
        var lines2 = lines.OrderBy(x => x, comparer).ToList();
        return (lines2.IndexOf(extraPackets[0]) + 1) * (lines2.IndexOf(extraPackets[1]) + 1);
    }
}

public class MyComparer : IComparer<String>
{
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    public int Compare(string stringA, string stringB)
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    {
        var day = new Day13();
        var response = (int)day.Compare(stringA, stringB);
        return response;
    }
}

public enum Outcome
{
    Smaller = -1,
    Equal = 0,
    Larger = 1
}