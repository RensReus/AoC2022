



namespace AoC2023;

class Day10 : BaseDay
{
    [Example(expected: 4, input: "-L|F7\n7S-7|\nL|7||\n-L-J|\nL|-JF")]
    [Example(expected: 8, input: "7-F7-\n.FJ|7\nSJLL7\n|F--J\nLJ.LJ")]
    [Example(expected: 8, input: "..F7.\n.FJ|.\nSJ.L7\n|F--J\nLJ...")]
    [Puzzle(expected: 6842)]
    public static int Part1(string input)
    {
        var lines = ReadLines(input);
        var startLine = lines.FindIndex(x => x.Contains('S'));
        var startCol = lines[startLine].IndexOf('S');
        var steps = 0;
        var evaluated = new Dictionary<string, int>();
        var toEval = new List<(int, int)> { (startLine, startCol) };
        while (toEval.Count > 0)
        {
            foreach (var x in toEval)
            {
                evaluated.Add($"{x.Item1},{x.Item2}", steps);
            }
            steps++;
            var newConnections = new List<(int, int)>();
            foreach (var option in toEval)
            {
                newConnections.AddRange(Connections(option.Item1, option.Item2, lines).Where(x => !evaluated.ContainsKey($"{x.Item1},{x.Item2}")));
            }
            if (newConnections.Distinct().Count() != newConnections.Count) return steps;
            toEval = newConnections;
        }
        return 123456789;
    }

    private static List<(int, int)> Connections(int curLine, int curCol, List<string> lines)
    {
        var directions = lines[curLine][curCol] switch
        {
            'S' => [(-1, 0), (1, 0), (0, 1), (0, -1)],
            '|' => [(-1, 0), (1, 0)],
            '-' => [(0, 1), (0, -1)],
            'F' => [(1, 0), (0, 1)],
            'J' => [(-1, 0), (0, -1)],
            'L' => [(-1, 0), (0, 1)],
            '7' => [(1, 0), (0, -1)],
            _ => new List<(int, int)>()
        };

        return directions
            .Where(x => InsideGrid(x, lines, curCol, curLine))
            .Where(x => ValidTiles(x, lines, curCol, curLine))
            .Select(x => (curLine + x.Item1, curCol + x.Item2)).ToList();
    }


    private static bool InsideGrid((int, int) x, List<string> lines, int curCol, int curLine)
        => curLine + x.Item1 >= 0 && curCol + x.Item2 >= 0 && curLine + x.Item1 < lines.Count && curCol + x.Item2 < lines[0].Length;

    private static bool ValidTiles((int, int) x, List<string> lines, int curCol, int curLine)
    {
        var validTiles = new Dictionary<(int, int), string>
        {
            {(-1,0), "|F7S"},
            {(1,0), "|JLS"},
            {(0,-1), "-FLS"},
            {(0,1), "-J7S"},
        };
        return validTiles[x].Contains(lines[curLine + x.Item1][curCol + x.Item2]);
    }

    [Example(expected: 4, input: "...........\n.S-------7.\n.|F-----7|.\n.||.....||.\n.||.....||.\n.|L-7.F-J|.\n.|..|.|..|.\n.L--J.L--J.\n...........")]
    [Example(expected: 8, input: ".F----7F7F7F7F-7....\n.|F--7||||||||FJ....\n.||.FJ||||||||L7....\nFJL7L7LJLJ||LJ.L-7..\nL--J.L7...LJS7F-7L7.\n....F-J..F7FJ|L7L7L7\n....L7.F7||L7|.L7L7|\n.....|FJLJ|FJ|F7|.LJ\n....FJL-7.||.||||...\n....L---J.LJ.LJLJ...")]
    [Example(expected: 10, input: "FF7FSF7F7F7F7F7F---7\nL|LJ||||||||||||F--J\nFL-7LJLJ||||||LJL-77\nF--JF--7||LJLJ7F7FJ-\nL---JF-JLJ.||-FJLJJ7\n|F|F-JF---7F7-L7L|7|\n|FFJF7L7F-JF7|JL---7\n7-L-JL7||F7|L7F-7F7|\nL.L7LFJ|||||FJL7||LJ\nL7JLJL-JLJLJL--JLJ.L")]
    [Puzzle(expected: 393)]
    public static int Part2(string input)
    {
        var lines = ReadLines(input);
        var startLine = lines.FindIndex(x => x.Contains('S'));
        var startCol = lines[startLine].IndexOf('S');
        var steps = 0;
        var evaluated = new Dictionary<string, int>();
        var toEval = new List<(int, int)> { (startLine, startCol) };
        var duplicate = (0, 0);
        while (toEval.Count > 0)
        {
            foreach (var x in toEval)
            {
                evaluated.Add($"{x.Item1},{x.Item2}", steps);
            }
            steps++;
            var newConnections = new List<(int, int)>();
            foreach (var option in toEval)
            {
                newConnections.AddRange(Connections(option.Item1, option.Item2, lines).Where(x => !evaluated.ContainsKey($"{x.Item1},{x.Item2}")));
            }
            if (newConnections.Distinct().Count() != newConnections.Count)
            {
                HashSet<(int, int)> seenElements = new();

                foreach ((int, int) element in newConnections)
                {
                    if (!seenElements.Add(element))
                    {
                        duplicate = element;
                    }
                }
                break;
            }
            toEval = newConnections;
        }
        toEval = [duplicate];

        var loop = new List<(int, int)> { (startLine, startCol) };
        while (toEval.Count > 0)
        {
            foreach (var x in toEval)
            {
                loop.Add((x.Item1, x.Item2));
            }
            var newConnections = new List<(int, int)>();
            foreach (var option in toEval)
            {
                newConnections.AddRange(Connections(option.Item1, option.Item2, lines).Where(x => !loop.Contains((x.Item1, x.Item2))));
            }
            toEval = newConnections;
        }

        var sDiff = (loop[^1].Item1 - loop[^2].Item1, loop[^1].Item2 - loop[^2].Item2);
        var replace = Math.Sign(sDiff.Item1) == Math.Sign(sDiff.Item2) ? '7' : 'J'; // TODO check all 4 options
        var newline = lines[startLine].ToArray();
        newline[startCol] = replace;
        lines[startLine] = new string(newline);

        return CountInsideFields(loop, lines);
    }

    private static int CountInsideFields(List<(int, int)> loop, List<string> lines)
    {
        var inside = (1, 0);
        var checkedLoop = new List<(int, int)>();
        var insideFields = new List<(int, int)>();
        var nextElement = loop.Where(x => lines[x.Item1][x.Item2] == '-').OrderBy(x => x.Item1).First();
        while (true)
        {
            var loopChar = lines[nextElement.Item1][nextElement.Item2];
            var insideNeighbour = (nextElement.Item1 + inside.Item1, nextElement.Item2 + inside.Item2);
            if (!insideFields.Contains(insideNeighbour) && !loop.Contains(insideNeighbour))
            {
                insideFields.AddRange(FindInsideFields(insideNeighbour, loop));
            }
            if (!"-|".Contains(loopChar))
            {
                inside = TransformInside(inside, loopChar);
                // TODO dit in functie
                insideNeighbour = (nextElement.Item1 + inside.Item1, nextElement.Item2 + inside.Item2);
                if (!insideFields.Contains(insideNeighbour) && !loop.Contains(insideNeighbour))
                {
                    insideFields.AddRange(FindInsideFields(insideNeighbour, loop));
                }
            }
            checkedLoop.Add(nextElement);
            var nextOptions = Connections(nextElement.Item1, nextElement.Item2, lines).Where(x => !checkedLoop.Contains(x));
            if (!nextOptions.Any()) break;
            nextElement = nextOptions.First();
        }
        return insideFields.Count;
    }

    private static (int, int) TransformInside((int, int) inside, char loopChar)
        => loopChar switch
        {
            'F' or 'J' => (inside.Item2, inside.Item1),
            '7' or 'L' => (-inside.Item2, -inside.Item1),
            _ => inside
        };

    private static List<(int, int)> FindInsideFields((int, int) insideNeighbour, List<(int, int)> loop)
    {
        var insideFields = new List<(int, int)> { insideNeighbour };
        var toEval = new List<(int, int)> { insideNeighbour };
        while (toEval.Count > 0)
        {
            var nextToEval = new List<(int, int)>();
            foreach (var field in toEval)
            {
                foreach (var dir in new List<(int, int)> { (-1, 0), (1, 0), (0, 1), (0, -1) })
                {
                    var newField = (field.Item1 + dir.Item1, field.Item2 + dir.Item2);
                    if (!insideFields.Contains(newField) && !loop.Contains(newField))
                    {
                        nextToEval.Add(newField);
                        insideFields.Add(newField);
                    }
                }
            }
            toEval = nextToEval;
        }
        return insideFields;
    }
}