namespace AoC2022;

class Day14 : BaseDay
{
    static (char[,], int) ProcessInput(string input, bool part2 = false)
    {
        var inputCoords = input.Split("\n")
            .Select(x =>
                x.Split(" -> ")
                    .Select(y => (int.Parse(y.Split(",")[0]), int.Parse(y.Split(",")[1]))));
        var maxY = inputCoords.Max(line => line.Max(pair => pair.Item2));
        var maxX = inputCoords.Max(line => line.Max(pair => pair.Item1));
        if (part2)
        {
            maxY += 2;
            maxX += maxY + 1;
        }
        var state = new char[maxX + 1, maxY + 1];
        for (int i = 0; i < maxX + 1; i++)
        {
            for (int j = 0; j < maxY + 1; j++)
            {
                state[i, j] = '.';
            }
        }
        foreach (var line in inputCoords)
        {
            var l = line.ToArray();
            for (int i = 0; i < line.Count() - 1; i++)
            {
                var pair1 = l[i];
                var pair2 = l[i + 1];
                if (pair1.Item1 == pair2.Item1)
                {
                    for (int j = int.Min(pair1.Item2, pair2.Item2); j < int.Max(pair1.Item2, pair2.Item2) + 1; j++)
                    {
                        state[pair1.Item1, j] = '#';
                    }
                }
                if (pair1.Item2 == pair2.Item2)
                {
                    for (int j = int.Min(pair1.Item1, pair2.Item1); j < int.Max(pair1.Item1, pair2.Item1) + 1; j++)
                    {
                        state[j, pair1.Item2] = '#';
                    }
                }
            }
        }

        if (part2)
        {
            for (int i = 0; i < maxX + 1; i++)
            {
                state[i, maxY] = '#';
            }
        }
        return (state, maxY);
    }

    [Example(expected: 24, input: "498,4 -> 498,6 -> 496,6\n503,4 -> 502,4 -> 502,9 -> 494,9")]
    [Puzzle(expected: 1330)]
    public int Part1(string input)
    {
        (var state, var maxY) = ProcessInput(input);
        var answer = 0;

        while (true)
        {
            (state, bool cameToRest) = DropSand(state, maxY);
            if (!cameToRest) break;
            answer++;
        }
        return answer;
    }

    private static void PrintState(char[,] state, int minX)
    {
        var height = state.GetLength(1);
        var width = state.GetLength(0);
        for (int i = 0; i < height; i++)
        {
            for (int j = minX; j < width; j++)
            {
                Console.Write(state[j, i]);
            }
            Console.WriteLine();
        }
    }

    private (char[,] state, bool cameToRest) DropSand(char[,] state, int maxY)
    {
        var x = 500;
        var y = 0;
        while (y < maxY)
        {
            var down = state[x, y + 1];
            var left = state[x - 1, y + 1];
            var right = state[x + 1, y + 1];
            if (state[x, y + 1] == '.') { y++; continue; }
            if (state[x - 1, y + 1] == '.') { x--; y++; continue; }
            if (state[x + 1, y + 1] == '.') { x++; y++; continue; }
            state[x, y] = 'o';
            return (state, true);
        }
        return (state, false);
    }

    [Example(expected: 93, input: "498,4 -> 498,6 -> 496,6\n503,4 -> 502,4 -> 502,9 -> 494,9")]
    [Puzzle(expected: 26139)]
    public int Part2(string input)
    {
        (var state, var maxY) = ProcessInput(input, true);
        var answer = 0;
        while (true)
        {
            (state, bool cameToRest) = DropSand(state, maxY);
            answer++;
            if (state[500, 0] == 'o') break;
        }
        PrintState(state, 485);

        return answer;
    }
}

public class LineCoord
{
    int X;
    int Y;
    public LineCoord(string line)
    {
        X = int.Parse(line.Split(",")[0]);
        Y = int.Parse(line.Split(",")[1]);
    }
}