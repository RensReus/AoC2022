namespace AoC2024;

class Day08 : BaseDay
{
    [Example(expected: 14, input: "............\n........0...\n.....0......\n.......0....\n....0.......\n......A.....\n............\n............\n........A...\n.........A..\n............\n............")]
    [Puzzle(expected: 318)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);
        var antennaGroups = new Dictionary<char, List<(int, int)>>();
        var iBound = lines.Count;
        var jBound = lines[0].Length;
        for (int i = 0; i < iBound; i++)
        {
            for (int j = 0; j < jBound; j++)
            {
                var c = lines[i][j];
                if (c != '.')
                {
                    if (!antennaGroups.TryGetValue(c, out var positions))
                    {
                        positions = [];
                        antennaGroups[c] = positions;
                    }
                    positions.Add((i, j));
                }
            }
        }
        var antinodes = new HashSet<(int, int)>();

        foreach (var (_, antennas) in antennaGroups)
        {
            for (int i = 0; i < antennas.Count; i++)
            {
                var a = antennas[i];
                for (int j = i + 1; j < antennas.Count; j++)
                {
                    var b = antennas[j];
                    var diff = (b.Item1 - a.Item1, b.Item2 - a.Item2);
                    var antinode1 = (a.Item1 - diff.Item1, a.Item2 - diff.Item2);
                    var antinode2 = (b.Item1 + diff.Item1, b.Item2 + diff.Item2);
                    if (antinode1.Item1 >= 0 && antinode1.Item1 < iBound && antinode1.Item2 >= 0 && antinode1.Item2 < jBound)
                    {
                        antinodes.Add(antinode1);
                    }
                    if (antinode2.Item1 >= 0 && antinode2.Item1 < iBound && antinode2.Item2 >= 0 && antinode2.Item2 < jBound)
                    {
                        antinodes.Add(antinode2);
                    }
                }
            }
        }

        return antinodes.Count;
    }

    [Example(expected: 34, input: "............\n........0...\n.....0......\n.......0....\n....0.......\n......A.....\n............\n............\n........A...\n.........A..\n............\n............")]
    [Puzzle(expected: 1126)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input);
        var antennaGroups = new Dictionary<char, List<(int, int)>>();
        var iBound = lines.Count;
        var jBound = lines[0].Length;
        for (int i = 0; i < iBound; i++)
        {
            for (int j = 0; j < jBound; j++)
            {
                var c = lines[i][j];
                if (c != '.')
                {
                    if (!antennaGroups.TryGetValue(c, out var positions))
                    {
                        positions = [];
                        antennaGroups[c] = positions;
                    }
                    positions.Add((i, j));
                }
            }
        }
        var antinodes = new HashSet<(int, int)>();

        foreach (var (_, antennas) in antennaGroups)
        {
            for (int i = 0; i < antennas.Count; i++)
            {
                var a = antennas[i];
                for (int j = i + 1; j < antennas.Count; j++)
                {
                    var b = antennas[j];
                    var diff = (b.Item1 - a.Item1, b.Item2 - a.Item2);
                    var toAdd = b;
                    while (toAdd.Item1 >= 0 && toAdd.Item1 < iBound && toAdd.Item2 >= 0 && toAdd.Item2 < jBound)
                    {
                        antinodes.Add(toAdd);
                        toAdd = (toAdd.Item1 + diff.Item1, toAdd.Item2 + diff.Item2);
                    }
                    toAdd = b;
                    while (toAdd.Item1 >= 0 && toAdd.Item1 < iBound && toAdd.Item2 >= 0 && toAdd.Item2 < jBound)
                    {
                        antinodes.Add(toAdd);
                        toAdd = (toAdd.Item1 - diff.Item1, toAdd.Item2 - diff.Item2);
                    }
                }
            }
        }

        return antinodes.Count;
    }
}