namespace AoC2024;

class Day15 : BaseDay
{
    [Example(expected: 10092, input: "##########\n#..O..O.O#\n#......O.#\n#.OO..O.O#\n#..O@..O.#\n#O#..O...#\n#O..O..O.#\n#.OO.O.OO#\n#....O...#\n##########\n\n<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^\nvvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v\n><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<\n<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^\n^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><\n^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^\n>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^\n<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>\n^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>\nv^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^")]
    [Example(expected: 2028, input: "########\n#..O.O.#\n##@.O..#\n#...O..#\n#.#.O..#\n#...O..#\n#......#\n########\n\n<^^>>>vv<v>>v<<")]
    [Puzzle(expected: 1476771)]
    public static int Part1(string input)
    {
        var lines = ReadLinesDouble(input);
        var maze = BuildMaze(lines[0]);
        var moves = string.Join("", lines[1]);

        var botPos = maze.First(x => x.Value == '@').Key;
        maze[botPos] = '.';

        foreach (var move in moves)
        {
            var (dX, dY) = GetDelta(move);
            var nextPos = (botPos.X + dX, botPos.Y + dY);
            while (maze[nextPos] is 'O')
            {
                nextPos = (nextPos.Item1 + dX, nextPos.Item2 + dY);
            }
            if (maze[nextPos] is '#') continue;

            botPos = (botPos.X + dX, botPos.Y + dY);
            if (maze[botPos] is 'O')
            {
                maze[nextPos] = 'O';
                maze[botPos] = '.';
            }
        }

        return maze.Where(x => x.Value == 'O').Sum(x => x.Key.X + x.Key.Y * 100);
    }

    private static (int dX, int dY) GetDelta(char move)
    {
        return move switch
        {
            '^' => (0, -1),
            'v' => (0, 1),
            '<' => (-1, 0),
            '>' => (1, 0),
            _ => throw new InvalidOperationException()
        };
    }

    private static Dictionary<(int X, int Y), char> BuildMaze(List<string> list)
    {
        var maze = new Dictionary<(int X, int Y), char>();
        for (var y = 0; y < list.Count; y++)
        {
            for (var x = 0; x < list[y].Length; x++)
            {
                maze[(x, y)] = list[y][x];
            }
        }

        return maze;
    }

    [Example(expected: 9021, input: "##########\n#..O..O.O#\n#......O.#\n#.OO..O.O#\n#..O@..O.#\n#O#..O...#\n#O..O..O.#\n#.OO.O.OO#\n#....O...#\n##########\n\n<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^\nvvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v\n><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<\n<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^\n^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><\n^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^\n>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^\n<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>\n^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>\nv^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^")]
    [Puzzle(expected: 1468005)]
    public static int Part2(string input)
    {
        var upsized = input.Replace(".", "..").Replace("#", "##").Replace("O", "[]").Replace("@", "@.");
        var lines = ReadLinesDouble(upsized);
        var maze = BuildMaze(lines[0]);
        var moves = string.Join("", lines[1]);

        var botPos = maze.First(x => x.Value == '@').Key;
        maze[botPos] = '.';

        foreach (var move in moves)
        {
            var (dX, dY) = GetDelta(move);
            var toEval = new HashSet<(int X, int Y)> { (botPos.X + dX, botPos.Y + dY) };
            if (dY != 0)
            {
                if (maze[(botPos.X, botPos.Y + dY)] is '[') toEval.Add((botPos.X + 1, botPos.Y + dY));
                if (maze[(botPos.X, botPos.Y + dY)] is ']') toEval.Add((botPos.X - 1, botPos.Y + dY));
            }

            var boxesToMove = new HashSet<(int X, int Y)>();
            while (toEval.Any(pos => maze[pos] is '[' or ']'))
            {
                var nextToEval = new HashSet<(int X, int Y)>();
                foreach (var item in toEval.Where(pos => maze[pos] is '[' or ']'))
                {
                    boxesToMove.Add(item);
                    var next = (item.X + dX, item.Y + dY);
                    nextToEval.Add(next);
                    if (dY != 0)
                    {
                        if (maze[next] is '[') nextToEval.Add((item.X + 1, item.Y + dY));
                        if (maze[next] is ']') nextToEval.Add((item.X - 1, item.Y + dY));
                    }
                }

                toEval = nextToEval;
                if (toEval.Any(pos => maze[pos] is '#')) break;
            }

            if (toEval.Any(nextPos => maze[nextPos] is '#')) continue;

            botPos = (botPos.X + dX, botPos.Y + dY);
            foreach (var box in boxesToMove.Reverse())
            {
                maze[(box.X + dX, box.Y + dY)] = maze[(box.X, box.Y)];
                maze[box] = '.';
            }
        }

        return maze.Where(x => x.Value == '[').Sum(x => x.Key.X + x.Key.Y * 100);
    }
}