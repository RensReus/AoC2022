namespace AoC2024;

class Day25 : BaseDay
{
    [Example(expected: 3, input: "#####\n.####\n.####\n.####\n.#.#.\n.#...\n.....\n\n#####\n##.##\n.#.##\n...##\n...#.\n...#.\n.....\n\n.....\n#....\n#....\n#...#\n#.#.#\n#.###\n#####\n\n.....\n.....\n#.#..\n###..\n###.#\n###.#\n#####\n\n.....\n.....\n.....\n#....\n#.#..\n#.#.#\n#####")]
    [Puzzle(expected: 2840)]
    public static int Part1(string input)
    {
        var groups = ReadLinesDouble(input);
        var keys = new List<int[]>();
        var locks = new List<int[]>();
        foreach (var group in groups)
        {
            var output = new int[5];
            var isKey = group[0][0] == '#';

            for (int col = 0; col < 5; col++)
            {
                for (int row = 0; row < 7; row++)
                {
                    if (group[row][col] == '#')
                    {
                        output[col]++;
                    }
                }
            }
            if (isKey)
            {
                keys.Add(output);
            }
            else
            {
                locks.Add(output);
            }
        }
        var answer = 0;
        foreach (var key in keys)
        {
            foreach (var @lock in locks)
            {
                var valid = true;
                for (int i = 0; i < 5; i++)
                {
                    if (key[i] + @lock[i] > 7)
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid) answer++;
            }
        }
        return answer;
    }
}