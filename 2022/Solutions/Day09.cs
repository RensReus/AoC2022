namespace AoC2022;

class Day09 : BaseDay
{
    [Example(expected: 13, input: "R 4\nU 4\nL 3\nD 1\nR 4\nD 1\nL 5\nR 2")]
    [Puzzle(expected: 6057)]
    public int Part1(string input)
        => UniqueVisitedFieldsCount(ReadLines(input), 2);

    [Example(expected: 1, input: "R 4\nU 4\nL 3\nD 1\nR 4\nD 1\nL 5\nR 2")]
    [Example(expected: 36, input: "R 5\nU 8\nL 8\nD 3\nR 17\nD 10\nL 25\nU 20")]
    [Puzzle(expected: 2514)]
    public int Part2(string input)
        => UniqueVisitedFieldsCount(ReadLines(input), 10);

    public int UniqueVisitedFieldsCount(IList<string> input, int ropeLength)
    {
        var rope = GenerateRope(ropeLength);
        var visited = new HashSet<Coord> { rope.Last() };
        foreach (var move in input)
        {
            var dir = move.Split()[0];
            var dist = int.Parse(move.Split()[1]);
            for (int i = 0; i < dist; i++)
            {
                rope[0].Move(dir);
                rope = UpdateTail(rope);
                visited.Add(rope.Last());
            }
        }
        return visited.Count();
    }

    private List<Coord> GenerateRope(int length)
    {
        var rope = new List<Coord>();
        for (int i = 0; i < length; i++)
        {
            rope.Add(new Coord(0, 0));
        }
        return rope;
    }

    private List<Coord> UpdateTail(List<Coord> rope)
    {
        for (int i = 1; i < rope.Count(); i++)
        {
            rope[i] = UpdateTail(rope[i - 1], rope[i]);
        }
        return rope;
    }

    private Coord UpdateTail(Coord headPos, Coord tailPos)
    {
        var deltaX = headPos.X - tailPos.X;
        var deltaY = headPos.Y - tailPos.Y;

        if (Math.Abs(deltaX) <= 1 && Math.Abs(deltaY) <= 1) return tailPos;

        return new Coord(tailPos.X + Math.Sign(deltaX), tailPos.Y + Math.Sign(deltaY));
    }
}

internal class Coord
{
    public int X;
    public int Y;
    public Coord(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }
    public override bool Equals(object? other)
        => other is Coord c && X == c.X && Y == c.Y;

    public override int GetHashCode()
        => HashCode.Combine(X, Y);

    internal void Move(string dir)
    {
        switch (dir)
        {
            case "R": X += 1; break;
            case "L": X -= 1; break;
            case "U": Y += 1; break;
            case "D": Y -= 1; break;
        }
    }
}