
namespace AoC2023;

class Day10 : BaseDay
{
    [Example(expected: 4, input: "-L|F7\n7S-7|\nL|7||\n-L-J|\nL|-JF")]
    [Example(expected: 8, input: "7-F7-\n.FJ|7\nSJLL7\n|F--J\nLJ.LJ")]
    [Example(expected: 8, input: "..F7.\n.FJ|.\nSJ.L7\n|F--J\nLJ...")]
    [Puzzle(expected: 6842)]
    public static int Part1(string input)
    {
        var loop = FindLoop(input);
        return loop.Count / 2;
    }

    public static List<(int, int)> FindLoop(string input)
    {
        var lines = ReadLines(input);
        var startLine = lines.FindIndex(x => x.Contains('S'));
        var start = (startLine, lines[startLine].IndexOf('S'));
        foreach (var loopOption in Connections(start, lines))
        {
            var loop = TryToFindLoop(start, loopOption, lines);
            if (loop.Count != 0) return loop;
        }
        return [];
    }

    private static List<(int, int)> TryToFindLoop((int startLine, int) start, (int, int) loopOption, List<string> lines)
    {
        var evaluated = new List<(int, int)> { start };
        var toEval = loopOption;
        while (true)
        {
            evaluated.Add(toEval);
            var nextOptions = Connections(toEval, lines);
            if (nextOptions.Contains(start) && toEval != loopOption) return evaluated;
            if (!nextOptions.Any(x => !evaluated.Contains(x))) return [];
            toEval = nextOptions.Single(x => !evaluated.Contains(x));
        };
    }

    private static List<(int, int)> Connections((int, int) cur, List<string> lines)
    {
        var directions = lines[cur.Item1][cur.Item2] switch
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
            .Where(x => InsideGrid(x, lines, cur))
            .Where(x => ValidTiles(x, lines, cur))
            .Select(x => (cur.Item1 + x.Item1, cur.Item2 + x.Item2)).ToList();
    }


    private static bool InsideGrid((int, int) x, List<string> lines, (int, int) cur)
        => cur.Item1 + x.Item1 >= 0 && cur.Item2 + x.Item2 >= 0 && cur.Item1 + x.Item1 < lines.Count && cur.Item2 + x.Item2 < lines[0].Length;

    private static bool ValidTiles((int, int) x, List<string> lines, (int, int) cur)
    {
        var validTiles = new Dictionary<(int, int), string>
        {
            {(-1,0), "|F7S"},
            {(1,0), "|JLS"},
            {(0,-1), "-FLS"},
            {(0,1), "-J7S"},
        };
        return validTiles[x].Contains(lines[cur.Item1 + x.Item1][cur.Item2 + x.Item2]);
    }

    [Example(expected: 4, input: "...........\n.S-------7.\n.|F-----7|.\n.||.....||.\n.||.....||.\n.|L-7.F-J|.\n.|..|.|..|.\n.L--J.L--J.\n...........")]
    [Example(expected: 8, input: ".F----7F7F7F7F-7....\n.|F--7||||||||FJ....\n.||.FJ||||||||L7....\nFJL7L7LJLJ||LJ.L-7..\nL--J.L7...LJS7F-7L7.\n....F-J..F7FJ|L7L7L7\n....L7.F7||L7|.L7L7|\n.....|FJLJ|FJ|F7|.LJ\n....FJL-7.||.||||...\n....L---J.LJ.LJLJ...")]
    [Example(expected: 10, input: "FF7FSF7F7F7F7F7F---7\nL|LJ||||||||||||F--J\nFL-7LJLJ||||||LJL-77\nF--JF--7||LJLJ7F7FJ-\nL---JF-JLJ.||-FJLJJ7\n|F|F-JF---7F7-L7L|7|\n|FFJF7L7F-JF7|JL---7\n7-L-JL7||F7|L7F-7F7|\nL.L7LFJ|||||FJL7||LJ\nL7JLJL-JLJLJL--JLJ.L")]
    [Puzzle(expected: 393)]
    public static int Part2(string input)
    {
        var loop = FindLoop(input);

        var updatedLines = UpdateS(loop, input);

        return CountInsideFields(loop, updatedLines);
    }

    private static List<string> UpdateS(List<(int, int)> loop, string input)
    {
        var lines = ReadLines(input);
        var startLine = lines.FindIndex(x => x.Contains('S'));
        var start = (startLine, lines[startLine].IndexOf('S'));

        var neighbors = Connections(start, lines).Where(loop.Contains).ToList();
        var sDiff = (neighbors[0].Item1 - neighbors[1].Item1, neighbors[0].Item2 - neighbors[1].Item2);

        char replace;
        if (sDiff.Item1 == 0) replace = '-';
        else if (sDiff.Item2 == 0) replace = '|';

        var eitherHigher = neighbors[0].Item1 < start.Item1 || neighbors[0].Item1 < start.Item1;
        var eitherRight = neighbors[0].Item2 > start.Item2 || neighbors[1].Item2 > start.Item2;

        replace = (eitherHigher, eitherRight) switch
        {
            (true, true) => 'L',
            (true, false) => 'J',
            (false, true) => 'F',
            _ => '7'
        };

        var newline = lines[start.Item1].ToArray();
        newline[start.Item2] = replace;
        lines[start.Item1] = new string(newline);
        return lines;
    }

    private static int CountInsideFields(List<(int, int)> loop, List<string> lines)
    {
        var insideVector = (1, 0);
        var checkedLoop = new List<(int, int)>();
        var insideFields = new List<(int, int)>();
        var nextElement = loop.Where(x => lines[x.Item1][x.Item2] == '-').OrderBy(x => x.Item1).First();
        while (true)
        {
            var loopChar = lines[nextElement.Item1][nextElement.Item2];
            insideFields.AddRange(FindInsideFields(nextElement, loop, insideVector, insideFields));
            if (!"-|".Contains(loopChar))
            {
                insideVector = TransformInside(insideVector, loopChar);
                insideFields.AddRange(FindInsideFields(nextElement, loop, insideVector, insideFields));
            }
            checkedLoop.Add(nextElement);
            var nextOptions = Connections(nextElement, lines).Where(x => !checkedLoop.Contains(x));
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

    private static List<(int, int)> FindInsideFields((int, int) nextElement, List<(int, int)> loop, (int, int) insideVector, List<(int, int)> insideFields)
    {
        var insideNeighbour = (nextElement.Item1 + insideVector.Item1, nextElement.Item2 + insideVector.Item2);
        if (insideFields.Contains(insideNeighbour) || loop.Contains(insideNeighbour))
        {
            return [];
        }
        var newInsideFields = new List<(int, int)> { insideNeighbour };
        var toEval = new List<(int, int)> { insideNeighbour };
        while (toEval.Count > 0)
        {
            var nextToEval = new List<(int, int)>();
            foreach (var field in toEval)
            {
                foreach (var dir in new List<(int, int)> { (-1, 0), (1, 0), (0, 1), (0, -1) })
                {
                    var newField = (field.Item1 + dir.Item1, field.Item2 + dir.Item2);
                    if (!newInsideFields.Contains(newField) && !loop.Contains(newField))
                    {
                        nextToEval.Add(newField);
                        newInsideFields.Add(newField);
                    }
                }
            }
            toEval = nextToEval;
        }
        return newInsideFields;
    }
}