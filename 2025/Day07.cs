namespace AoC2025;

class Day07 : BaseDay
{
    [Example(expected: 21, input: ".......S.......\n...............\n.......^.......\n...............\n......^.^......\n...............\n.....^.^.^.....\n...............\n....^.^...^....\n...............\n...^.^...^.^...\n...............\n..^...^.....^..\n...............\n.^.^.^.^.^...^.\n...............")]
    [Puzzle(expected: 1490)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);
        var beams = new HashSet<int> { lines[0].IndexOf('S') };
        var answer = 0;
        foreach (var line in lines)
        {
            var newBeams = new HashSet<int>();
            foreach (var c in beams)
            {
                if (line[c] == '^')
                {
                    newBeams.Add(c - 1);
                    newBeams.Add(c + 1);
                    answer++;
                }
                else
                {
                    newBeams.Add(c);
                }
            }
            beams = newBeams;
        }
        return answer;
    }

    [Example(expected: 40, input: ".......S.......\n...............\n.......^.......\n...............\n......^.^......\n...............\n.....^.^.^.....\n...............\n....^.^...^....\n...............\n...^.^...^.^...\n...............\n..^...^.....^..\n...............\n.^.^.^.^.^...^.\n...............")]
    [Puzzle(expected: 3806264447357)]
    public static long Part2(string input)
    {
        var lines = ReadLines(input);
        var beams = new Dictionary<int, long> { { lines[0].IndexOf('S'), 1 } };
        foreach (var line in lines)
        {
            var newBeams = new Dictionary<int, long>();
            foreach (var (col, count) in beams)
            {
                if (line[col] == '^')
                {
                    AddToDict(newBeams, col - 1, count);
                    AddToDict(newBeams, col + 1, count);
                }
                else
                {
                    AddToDict(newBeams, col, count);
                }
            }
            beams = newBeams;
        }
        return beams.Values.Sum();
    }

    private static void AddToDict(Dictionary<int, long> dict, int key, long value)
    {
        if (dict.ContainsKey(key))
        {
            dict[key] += value;
        }
        else
        {
            dict[key] = value;
        }
    }
}