namespace AoC2022;

static class Day24
{
    static (HashSet<Blizzard>, Pos, int, int) ProcessInput(string input)
    {
        var lines = input.Split(";").ToList();
        var target = new Pos(lines.Count - 2, lines.Last().IndexOf('.') - 1);
        var blizzards = new HashSet<Blizzard>();
        for (int row = 1; row < lines.Count; row++)
        {
            var line = lines[row];
            for (int col = 1; col < line.Length; col++)
            {
                if ("<>v^".Contains(line[col])) blizzards.Add(new Blizzard(row - 1, col - 1, line[col]));
            }
        }
        return (blizzards, target, lines.Count - 2, lines[0].Length - 2);
    }

    [Example(expected: 18, input: "#.######;#>>.<^<#;#.<..<<#;#>v.><>#;#<^v^^>#;######.#")]
    [Puzzle(expected: 232)]
    public static int Part1(string input)
    {
        (var blizzards, var target, var maxRow, var maxCol) = ProcessInput(input);
        var start = new Pos(-1, 0);
        return TravelTime(start, target, blizzards, maxRow, maxCol).Item1;
    }

    public static (int, HashSet<Blizzard>) TravelTime(Pos start, Pos target, HashSet<Blizzard> blizzards, int maxRow, int maxCol)
    {
        var currentPositions = new HashSet<Pos> { start };
        var minute = 0;
        while (!currentPositions.Contains(target))
        {
            minute++;
            blizzards = UpdateBlizzards(blizzards, maxRow, maxCol);
            var blizzardPositions = blizzards.Select(bl => bl.Position).ToHashSet();
            var newPositions = new HashSet<Pos>();
            foreach (var pos in currentPositions)
            {
                newPositions.UnionWith(pos.GetNextPositions(blizzardPositions, maxRow, maxCol, target, start));
            }
            currentPositions = newPositions;
        }
        return (minute, blizzards);
    }

    private static HashSet<Blizzard> UpdateBlizzards(HashSet<Blizzard> blizzards, int maxRow, int maxCol)
        => blizzards.Select(bl => bl.Move(maxRow, maxCol)).ToHashSet();

    [Example(expected: 54, input: "#.######;#>>.<^<#;#.<..<<#;#>v.><>#;#<^v^^>#;######.#")]
    [Puzzle(expected: 715)]
    public static int Part2(string input)
    {
        (var blizzards, var target, var maxRow, var maxCol) = ProcessInput(input);
        var start = new Pos(-1, 0);
        (var time, blizzards) = TravelTime(start, target, blizzards, maxRow, maxCol);
        (var time2, blizzards) = TravelTime(target, start, blizzards, maxRow, maxCol);
        (var time3, _) = TravelTime(start, target, blizzards, maxRow, maxCol);

        return time + time2 + time3;
    }
}

public class Pos
{
    public int Row;
    public int Col;
    public Pos(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public override bool Equals(object? other)
    => other is Pos c && Row == c.Row && Col == c.Col;

    public override int GetHashCode()
        => HashCode.Combine(Row, Col);

    internal IEnumerable<Pos> GetNextPositions(IEnumerable<Pos> blizzardPositions, int maxRow, int maxCol, Pos target, Pos start)
        => new List<Pos> { new(Row, Col), new(Row + 1, Col), new(Row - 1, Col), new(Row, Col + 1), new(Row, Col - 1) }
            .Where(pos => pos.InsideGrid(maxRow, maxCol, target, start))
            .Except(blizzardPositions);

    private bool InsideGrid(int maxRow, int maxCol, Pos target, Pos start)
        => (Row >= 0 && Row < maxRow && Col >= 0 && Col < maxCol) || this.Equals(target) || this.Equals(start);
}

internal class Blizzard
{
    public Pos Position;
    public char Dir;
    public Blizzard(int row, int col, char dir)
    {
        Position = new(row, col);
        Dir = dir;
    }

    internal Blizzard Move(int maxRow, int maxCol)
        => Dir switch
        {
            '>' => new Blizzard(Position.Row, (Position.Col + 1) % maxCol, Dir),
            '<' => new Blizzard(Position.Row, (Position.Col + maxCol - 1) % maxCol, Dir),
            'v' => new Blizzard((Position.Row + 1) % maxRow, Position.Col, Dir),
            '^' => new Blizzard((Position.Row + maxRow - 1) % maxRow, Position.Col, Dir),
            _ => throw new ArgumentOutOfRangeException()
        };
}