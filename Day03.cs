using AoC2022.Days;

namespace AoC2022.Days03;

class Day : BaseDay
{
    static List<string> ProcessInput(bool part2, string suffix)
    {
        var readFile = ReadFile("03/" + suffix);
        return readFile.Select(line => line).ToList();
    }

    public override int Part1(string suffix)
        => ProcessInput(false, suffix).Sum(x => PriorityValue(x));

    public override int Part2(string suffix)
    {
        var input = ProcessInput(true, suffix);
        var answer = 0;
        for (int i = 0; i < input.Count; i += 3)
        {
            foreach (var letter in input.ElementAt(i))
            {
                if (input.ElementAt(i + 1).Contains(letter) && input.ElementAt(i + 2).Contains(letter))
                {
                    answer += PriorityValue(letter);
                    break;
                }
            }
        }

        return answer;
    }

    private int PriorityValue(string line)
    {
        var A = line.Substring(0, line.Length / 2).ToCharArray();
        var B = line.Substring(line.Length / 2).ToCharArray();
        List<Char> Double = new List<Char>();
        foreach (var letter in A)
        {
            if (B.Contains(letter) && !Double.Contains(letter))
            {
                Double.Add(letter);
            }
        }
        return Double.Sum(letter => PriorityValue(letter));
    }

    private int PriorityValue(char letter)
        => letter switch
        {
            <= 'Z' => letter - 'A' + 27,
            >= 'a' => letter - 'a' + 1,
            _ => throw new ArgumentOutOfRangeException()
        };

    public override List<Case> Part1Cases() => new() { new("1a", 157), new("p1", 8298) };

    public override List<Case> Part2Cases() => new() { new("1a", 70), new("p1", 2708) };
}

