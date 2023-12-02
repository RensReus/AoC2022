namespace AoC2022;

class Day23 : BaseDay
{
    static HashSet<Elf> ProcessInput(string input)
    {
        var elves = new HashSet<Elf>();
        var lines = ReadLines(input);
        for (int y = 0; y < lines.Count; y++)
        {
            var line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == '#') elves.Add(new(x, y));
            }
        }
        return elves;
    }

    [Example(expected: 110, input: "....#..\n..###.#\n#...#.#\n.#...##\n#.###..\n##.#.##\n.#..#..")]
    [Puzzle(expected: 3766)]
    public static int Part1(string input)
    {
        var elves = ProcessInput(input);
        for (int round = 0; round < 10; round++)
        {
            (elves, _) = MoveElvesRound(elves, round);
        }
        return CalculateOpenSpaces(elves);
    }

    [Example(expected: 20, input: "....#..\n..###.#\n#...#.#\n.#...##\n#.###..\n##.#.##\n.#..#..")]
    [Puzzle(expected: 954)]
    public static int Part2(string input)
    {
        var elves = ProcessInput(input);
        var needToMoveElves = true;
        var round = 0;
        while (needToMoveElves)
        {
            (elves, needToMoveElves) = MoveElvesRound(elves, round);
            round++;
        }
        return round;
    }

    private static (HashSet<Elf>, bool) MoveElvesRound(HashSet<Elf> elves, int round)
    {
        var needToMoveElves = false;
        var newElves = new HashSet<Elf>();
        var proposedMoves = new Dictionary<Elf, int>();
        foreach (var elf in elves)
        {
            elf.ProposeMove(elves, round);
            if (elf.NeedsToMove && elf.ProposedMove is not null)
            {
                if (proposedMoves.ContainsKey(elf.ProposedMove)) proposedMoves[elf.ProposedMove]++;
                else proposedMoves[elf.ProposedMove] = 1;
                needToMoveElves = true;
            }
        }
        foreach (var elf in elves)
        {
            if (!elf.NeedsToMove || elf.ProposedMove is null || proposedMoves[elf.ProposedMove] > 1)
            {
                newElves.Add(elf);
            }
            else
            {
                newElves.Add(elf.ProposedMove);
            }
        }
        return (newElves, needToMoveElves);
    }

    private static int CalculateOpenSpaces(HashSet<Elf> elves)
    {
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;
        // hier ergens een off by one error, of misschien in het stappen zetten
        foreach (var elf in elves)
        {
            minX = int.Min(elf.X, minX);
            minY = int.Min(elf.Y, minY);
            maxX = int.Max(elf.X, maxX);
            maxY = int.Max(elf.Y, maxY);
        }
        return (maxX - minX + 1) * (maxY - minY + 1) - elves.Count;
    }
}


internal class Elf
{
    public int X;
    public int Y;
    public bool NeedsToMove;
    public Elf? ProposedMove;

    public Elf(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }
    public override bool Equals(object? other)
        => other is Elf c && X == c.X && Y == c.Y;

    public override int GetHashCode()
        => HashCode.Combine(X, Y);

    internal void ProposeMove(HashSet<Elf> elves, int round)
    {
        var directions = new List<string> { "N", "S", "W", "E" };

        NeedsToMove = HasNeighbours(elves);
        if (NeedsToMove)
        {
            for (int i = round; i < round + 4; i++)
            {
                var dir = directions[i % 4];
                if (SpotsFree(dir, elves))
                {
                    ProposedMove = ProposeMove(dir);
                    return;
                }
            }
            ProposedMove = null;
        }
    }

    private bool SpotsFree(string dir, HashSet<Elf> elves)
        => dir switch
        {
            "N" => !(elves.Contains(new Elf(X - 1, Y - 1)) || elves.Contains(new Elf(X, Y - 1)) || elves.Contains(new Elf(X + 1, Y - 1))),
            "S" => !(elves.Contains(new Elf(X - 1, Y + 1)) || elves.Contains(new Elf(X, Y + 1)) || elves.Contains(new Elf(X + 1, Y + 1))),
            "W" => !(elves.Contains(new Elf(X - 1, Y + 1)) || elves.Contains(new Elf(X - 1, Y)) || elves.Contains(new Elf(X - 1, Y - 1))),
            "E" => !(elves.Contains(new Elf(X + 1, Y + 1)) || elves.Contains(new Elf(X + 1, Y)) || elves.Contains(new Elf(X + 1, Y - 1))),
            _ => throw new ArgumentOutOfRangeException()
        };

    private Elf ProposeMove(string dir)
        => dir switch
        {
            "E" => new Elf(X + 1, Y),
            "W" => new Elf(X - 1, Y),
            "S" => new Elf(X, Y + 1),
            "N" => new Elf(X, Y - 1),
            _ => throw new ArgumentOutOfRangeException()
        };

    private bool HasNeighbours(HashSet<Elf> elves)
    {
        for (int dx = -1; dx < 2; dx++)
        {
            for (int dy = -1; dy < 2; dy++)
            {
                if ((dx != 0 || dy != 0) && elves.Contains(new Elf(X + dx, Y + dy)))
                {
                    return true;
                }
            }
        }
        return false;
    }
}