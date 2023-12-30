namespace AoC2022;

class Day22 : BaseDay
{
    static (char[,], List<Move>, int) ProcessInput(string input)
    {
        var forestlines = input.Split("\n\n")[0].Split("\n").ToList();
        var steps = input.Split("\n\n")[1];
        var width = forestlines.Max(x => x.Length);
        var height = forestlines.Count;
        var forest = new char[width, height];
        for (int row = 0; row < height; row++)
        {
            var rowFaceSize = 0;
            var line = forestlines[row];
            var lineLength = line.Length;
            for (int col = 0; col < lineLength; col++)
            {
                forest[col, row] = line[col];
                if (line[col] == '.' || line[col] == '#') rowFaceSize++;
            }
        }
        var moves = SplitMoveString(steps);
        return (forest, moves, forestlines[0].IndexOf('.'));
    }

    private static List<Move> SplitMoveString(string steps)
    {
        var toAdd = "";
        var moves = new List<Move>();
        foreach (var c in steps)
        {
            if (c is 'R' or 'L')
            {
                moves.Add(new Move(toAdd));
                moves.Add(new Move(c));
                toAdd = "";
            }
            else
            {
                toAdd += c;
            }
        }
        moves.Add(new Move(toAdd));
        return moves;
    }

    [Example(expected: 6032, input: "        ...#\n        .#..\n        #...\n        ....\n...#.......#\n........#...\n..#....#....\n..........#.\n        ...#....\n        .....#..\n        .#......\n        ......#.\n\n10R5L5R10L4R5L5")]
    [Puzzle(expected: 97356)]
    public static int Part1(string input)
    {
        (var forest, var moves, var startingCol) = ProcessInput(input);
        var maxCol = forest.GetLength(0);
        var maxRow = forest.GetLength(1);
        var position = new Position(startingCol);
        foreach (var move in moves)
        {
            if (move.RotationMove)
            {
                position.DirectionAttr.Rotate(move.RotationDirection);
            }
            else
            {
                // dit kan allemaal in een functie
                var stepsTaken = 0;
                while (stepsTaken < move.MoveLength)
                {
                    (var nextCol, var nextRow, var canMove) = position.NexPos(maxCol, maxRow, forest);
                    if (canMove)
                    {
                        stepsTaken++;
                        position.UpdatePosition(nextCol, nextRow);
                    }
                    else break;
                }
            }
        }
        return position.Answer();
    }

    [Puzzle(expected: 120175)]
    public static int Part2(string input)
    {
        (var forest, var moves, var startingCol) = ProcessInput(input);
        var position = new Position2(startingCol);
        foreach (var move in moves)
        {
            if (move.RotationMove)
            {
                position.DirectionAttr.Rotate(move.RotationDirection);
            }
            else
            {
                // dit kan allemaal in een functie
                var stepsTaken = 0;
                while (stepsTaken < move.MoveLength)
                {
                    (var nextCol, var nextRow, var newDir, var canMove) = position.NexPos(forest);
                    if (canMove)
                    {
                        stepsTaken++;
                        position.UpdatePosition(nextCol, nextRow, newDir);
                    }
                    else break;
                }
            }
        }
        return position.Answer();
    }
}

internal class Position2
{
    public int Col;
    public int Row;
    public Direction DirectionAttr;

    public Position2(int startingCol, int startRow = 0)
    {
        Col = startingCol;
        Row = 0;
        DirectionAttr = Direction.East;
    }

    internal int Answer()
        => (Row + 1) * 1000 + (Col + 1) * 4 + DirectionAttr.Answer();

    internal void UpdatePosition(int newCol, int newRow, Direction newDir)
    {
        Col = newCol;
        Row = newRow;
        DirectionAttr = newDir;
    }

    internal (int, int, Direction, bool) NexPos(char[,] forest)
    {
        var newCol = Col + DirectionAttr.DeltaCol;
        var newRow = Row + DirectionAttr.DeltaRow;
        var newDir = DirectionAttr;
        if (OutOfBounds(newRow, newCol, forest) || forest[newCol, newRow] == ' ' || forest[newCol, newRow] == 0) (newRow, newCol, newDir) = FaceJump(newRow, newCol);
        return (newCol, newRow, newDir, forest[newCol, newRow] == '.');
    }

    private bool OutOfBounds(int newRow, int newCol, char[,] forest)
    {
        return newRow < 0 || newRow >= 200 || newCol < 0 || newCol >= 150;
    }

    private static (int newRow, int newCol, Direction newDir) FaceJump(int newRow, int newCol)
    {
        return (newRow, newCol) switch
        {
            (-1, >= 50 and < 100) => (newCol + 100, 0, Direction.East),
            (-1, >= 100 and < 150) => (199, newCol - 100, Direction.North),
            ( >= 0 and < 50, 150) => (149 - newRow, 99, Direction.West),
            (50, >= 100 and < 150) => (newCol - 50, 99, Direction.West),
            ( >= 50 and < 100, 100) => (49, newRow + 50, Direction.North),
            ( >= 100 and < 150, 100) => (49 - newRow + 100, 149, Direction.West),
            (150, >= 50 and < 100) => (newCol + 100, 49, Direction.West),
            ( >= 150 and < 200, 50) => (149, newRow - 100, Direction.North),
            (200, >= 0 and < 50) => (0, newCol + 100, Direction.South),
            ( >= 150 and < 200, -1) => (0, newRow - 100, Direction.South),
            ( >= 100 and < 150, -1) => (49 - newRow + 100, 50, Direction.East),
            (99, >= 0 and < 50) => (newCol + 50, 50, Direction.East),
            ( >= 50 and < 100, 49) => (100, newRow - 50, Direction.South),
            ( >= 0 and < 50, 49) => (149 - newRow, 0, Direction.East),
            _ => throw new Exception()
        };
    }
}

internal class Position
{
    public int Col;
    public int Row;
    public Direction DirectionAttr;

    public Position(int startingCol)
    {
        Col = startingCol;
        DirectionAttr = new Direction(1, 0);
    }

    internal int Answer()
        => (Row + 1) * 1000 + (Col + 1) * 4 + DirectionAttr.Answer();

    internal void UpdatePosition(int newCol, int newRow)
    {
        Col = newCol;
        Row = newRow;
    }

    internal (int, int, bool) NexPos(int maxCol, int maxRow, char[,] forest)
    {
        var newCol = (Col + DirectionAttr.DeltaCol + maxCol) % maxCol;
        var newRow = (Row + DirectionAttr.DeltaRow + maxRow) % maxRow;
        while (forest[newCol, newRow] is not '#' and not '.')
        {
            newCol = (newCol + DirectionAttr.DeltaCol + maxCol) % maxCol;
            newRow = (newRow + DirectionAttr.DeltaRow + maxRow) % maxRow;
        }
        return (newCol, newRow, forest[newCol, newRow] is '.');
    }
}

public class Direction
{
    public int DeltaCol;
    public int DeltaRow;

    public Direction(int dCol, int dRow)
    {
        DeltaCol = dCol;
        DeltaRow = dRow;
    }

    public void Rotate(char c)
    {
        var oldCol = DeltaCol;
        if (c == 'R')
        {
            DeltaCol = -DeltaRow;
            DeltaRow = oldCol;
        }
        else
        {
            DeltaCol = DeltaRow;
            DeltaRow = -oldCol;
        }
    }
    public int Answer()
        => (DeltaCol, DeltaRow) switch
        {
            (1, 0) => 0,
            (0, 1) => 1,
            (-1, 0) => 2,
            (0, -1) => 3,
            _ => throw new InvalidDataException()
        };

    public static Direction North => new(0, -1);
    public static Direction South => new(0, 1);
    public static Direction West => new(-1, 0);
    public static Direction East => new(1, 0);
}

internal class Move
{
    public bool RotationMove;
    public char RotationDirection;
    public int MoveLength;
    public Move(char c)
    {
        RotationDirection = c;
        RotationMove = true;
    }

    public Move(string toAdd)
    {
        MoveLength = int.Parse(toAdd);
    }
}