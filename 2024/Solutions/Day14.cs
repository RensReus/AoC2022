
namespace AoC2024;

class Day14 : BaseDay
{
    [Example(expected: 12, input: "11,7\np=0,4 v=3,-3\np=6,3 v=-1,-3\np=10,3 v=-1,2\np=2,0 v=2,-1\np=0,0 v=1,3\np=3,0 v=-2,-2\np=7,6 v=-1,-3\np=3,0 v=-1,-2\np=9,3 v=2,3\np=7,3 v=-1,2\np=2,4 v=2,-3\np=9,5 v=-3,-3")]
    [Puzzle(expected: 230900224)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);
        var sizes = lines[0].Split(",").Select(int.Parse).ToArray();
        var halfX = sizes[0] / 2;
        var halfY = sizes[1] / 2;
        var counts = new int[4];
        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(" ");
            var pos = parts[0][2..].Split(",").Select(int.Parse).ToArray();
            var vel = parts[1][2..].Split(",").Select(int.Parse).ToArray();
            var endX = ((pos[0] + vel[0] * 100) % sizes[0] + sizes[0]) % sizes[0];
            var endY = ((pos[1] + vel[1] * 100) % sizes[1] + sizes[1]) % sizes[1];
            if (endX < halfX && endY < halfY) counts[0]++;
            if (endX > halfX && endY < halfY) counts[1]++;
            if (endX < halfX && endY > halfY) counts[2]++;
            if (endX > halfX && endY > halfY) counts[3]++;
        }
        return counts[0] * counts[1] * counts[2] * counts[3];
    }

    [Puzzle(expected: 6532)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input);
        var sizes = lines[0].Split(",").Select(int.Parse).ToArray();
        var halfX = sizes[0] / 2;
        var halfY = sizes[1] / 2;
        var counts = new int[4];
        var bots = new List<Bot>();
        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(" ");
            var pos = parts[0][2..].Split(",").Select(int.Parse).ToArray();
            var vel = parts[1][2..].Split(",").Select(int.Parse).ToArray();
            bots.Add(new Bot(pos[0], pos[1], vel[0], vel[1]));
        }
        var seconds = 0;
        var maxNeighbours = 0;
        var best = 0;
        while (seconds < sizes[0] * sizes[1])
        {
            foreach (var bot in bots)
            {
                bot.PosX = (bot.PosX + bot.VelX + sizes[0]) % sizes[0];
                bot.PosY = (bot.PosY + bot.VelY + sizes[1]) % sizes[1];
            }
            seconds++;
            var neighbourCount = NeighbourCount(bots);
            if (neighbourCount > maxNeighbours)
            {
                best = seconds;
                maxNeighbours = neighbourCount;
                Console.WriteLine($"{seconds}: {neighbourCount}");

                for (int i = 0; i < sizes[1]; i++)
                {
                    for (int j = 0; j < sizes[0]; j++)
                    {
                        var found = false;
                        foreach (var bot in bots)
                        {
                            if (bot.PosX == j && bot.PosY == i)
                            {
                                found = true;
                                break;
                            }
                        }
                        Console.Write(found ? "#" : ".");
                    }
                    Console.WriteLine();
                }
            }
        }
        return best;
    }

    private static int NeighbourCount(List<Bot> bots)
    {
        var quickLookUp = new HashSet<(int, int)>(bots.Select(b => (b.PosX, b.PosY)));
        var count = 0;
        foreach (var pos in quickLookUp)
        {
            List<(int, int)> neighbours = [(pos.Item1 - 1, pos.Item2-1), (pos.Item1, pos.Item2-1), (pos.Item1 + 1, pos.Item2-1),
                                           (pos.Item1 - 1, pos.Item2), (pos.Item1 + 1, pos.Item2),
                                           (pos.Item1 - 1, pos.Item2+1), (pos.Item1, pos.Item2+1), (pos.Item1 + 1, pos.Item2+1)];
            if (neighbours.Count(n => quickLookUp.Contains(n)) >= 2) count++;
        }
        return count;
    }

    private class Bot(int posX, int posY, int velX, int velY)
    {
        public int PosX { get; set; } = posX;
        public int PosY { get; set; } = posY;
        public int VelX { get; set; } = velX;
        public int VelY { get; set; } = velY;
    }
}