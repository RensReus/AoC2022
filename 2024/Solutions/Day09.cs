
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
        var ans = 0L;
        while (i <= endI)
        {
            if (memory[i] == -1)
            {
                while (memory[endI] == -1)
                {
                    endI--;
                }
                if (i > endI)
                {
                    break;
                }
                ans += memory[endI] * i;
                endI--;
            }
            else
            {
                ans += memory[i] * i;
            }
            i++;
        }

        return ans;
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

            // find a block of -1 that is big enough
            for (var j = 0; j < i + 1; j++)
            {
                if (memory[j] == -1)
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
        }

        var ans = 0L;
        for (int j = 0; j < memory.Count; j++)
        {
            if (memory[j] == -1) continue;
            ans += memory[j] * j;

        }
        return ans;
    }
}