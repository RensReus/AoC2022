
namespace AoC2024;

class Day09 : BaseDay
{
    [Example(expected: 1928, input: "2333133121414131402")]
    [Puzzle(expected: 6341711060162)]
    public static long Part1(string input)
    {
        var line = ReadLines(input)[0];
        var memory = BuildMemory(line);
        var i = 0;
        var endI = memory.Count - 1;
        while (true)
        {
            if (memory[i] == -1)
            {
                while (memory[endI] == -1) endI--;
                if (i > endI) break;

                memory[i] = memory[endI];
                memory[endI] = -1;
                endI--;
            }
            i++;
        }

        return memory.Select((x, i) => x * i).Where(x => x > 0).Sum();
    }

    private static List<long> BuildMemory(string line)
    {
        var memory = new List<long>();
        bool open = true;
        var id = -1;
        foreach (var c in line)
        {
            open = !open;
            if (!open)
            {
                id++;
            }
            for (long i = 0; i < c - '0'; i++)
            {
                memory.Add(open ? -1 : id);
            }
        }
        return memory;
    }

    [Example(expected: 2858, input: "2333133121414131402")]
    [Puzzle(expected: 6377400869326)]
    public static long Part2(string input)
    {
        var line = ReadLines(input)[0];
        var memory = BuildMemory(line);
        var i = memory.Count - 1;
        while (i > 0)
        {
            // skip -1
            if (memory[i] == -1)
            {
                i--;
                continue;
            }

            // get the size of the current block
            var id = memory[i];

            var size = 0;
            while (memory[i] == id)
            {
                i--;
                size++;
                if (i < 0) break;
            }

            for (var j = 0; j < i + 1; j++)
            {
                var openSize = 0;
                while (memory[j] == -1)
                {
                    j++;
                    openSize++;
                }

                if (openSize >= size)
                {
                    for (int k = j - openSize; k < j - openSize + size; k++)
                    {
                        memory[k] = id;
                    }
                    for (int k = i + 1; k < i + 1 + size; k++)
                    {
                        memory[k] = -1;
                    }
                    break;
                }
            }
        }

        return memory.Select((x, i) => x * i).Where(x => x > 0).Sum();
    }
}