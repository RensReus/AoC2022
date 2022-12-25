namespace AoC2022;

static class Day22
{
    static (char[,], List<Move>, int, int) ProcessInput(string input)
    {
        var forestlines = input.Split(";;")[0].Split(";").ToList();
        var steps = input.Split(";;")[1];
        var width = forestlines.Max(x => x.Length);
        var height = forestlines.Count;
        var forest = new char[width, height];
        var faceSize = int.MaxValue;
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
            faceSize = int.Min(faceSize, rowFaceSize);
        }
        var moves = SplitMoveString(steps);
        return (forest, moves, forestlines[0].IndexOf('.'), faceSize);
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

    [Example(expected: 6032, input: "        ...#;        .#..;        #...;        ....;...#.......#;........#...;..#....#....;..........#.;        ...#....;        .....#..;        .#......;        ......#.;;10R5L5R10L4R5L5")]
    [Puzzle(expected: 97356)]
    public static int Part1(string input)
    {
        (var forest, var moves, var startingCol, _) = ProcessInput(input);
        var maxCol = forest.GetLength(0);
        var maxRow = forest.GetLength(1);
        var position = new Position(startingCol);
        foreach (var move in moves)
        {
            if (move.RotationMove)
            {
                position.Direction.Rotate(move.RotationDirection);
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

    [Example(expected: 5031, input: "        ...#;        .#..;        #...;        ....;...#.......#;........#...;..#....#....;..........#.;        ...#....;        .....#..;        .#......;        ......#.;;10R5L5R10L4R5L5")]
    [Puzzle(expected: 222222)]
    public static int Part2(string input)
    {
        (var forest, var moves, var startingCol, var faceSize) = ProcessInput(input);
        var exampleTransitions = new List<string>
        {
            "D4UL3UU2UR6R",
            "R3LU1UD5DL6D",
            "L2RU1LR4LD5L",
            "L3RU1DD5UR6U",
            "U4DR6LD2DL3D",
            "U4RR1RD2LL5R"
        };
        var actualTransitions = new List<string>
        {
            "U6LR2LL4LD3U",
            "L1RR5RD3RU6D",
            "U1DR2DD5UL4U",
            "U3LR5LD6UL1L",
            "L4RU3DR2RD6R",
            "U4DR5DD2UL1U"
        };
        var faces = ExtractFaces(forest, faceSize, faceSize > 4 ? actualTransitions : exampleTransitions);
        var position = new Position2(startingCol);
        foreach (var move in moves)
        {
            if (move.RotationMove)
            {
                position.Rotate(move.RotationDirection);
            }
            else
            {
                // dit kan allemaal in een functie
                var stepsTaken = 0;
                while (stepsTaken < move.MoveLength)
                {
                    (var nextCol, var nextRow, var newFace, var newDir, var canMove) = position.NexPos(forest, faces);
                    if (canMove)
                    {
                        stepsTaken++;
                        position.UpdatePosition(nextCol, nextRow, newFace, newDir);
                    }
                    else break;
                }
            }
        }
        return position.Answer();
    }

    private static List<Face> ExtractFaces(char[,] forest, int faceSize, List<string> transitions)
    {
        var faces = new List<Face>();
        for (int row = 0; row < forest.GetLength(1) / faceSize; row++)
        {
            for (int col = 0; col < forest.GetLength(0) / faceSize; col++)
            {
                if (".#".Contains(forest[col * faceSize, row * faceSize]))
                {
                    faces.Add(new(col, row, faceSize));
                }
            }
        }
        for (int i = 0; i < faces.Count; i++)
        {
            faces[i].AddNeighbours(transitions[i]);
        }
        return faces;
    }
}

internal class Face
{
    public int MinCol => Col * FaceSize;
    public int MaxCol => MinCol + FaceSize - 1;
    public int MinRow => Row * FaceSize;
    public int MaxRow => MinRow + FaceSize - 1;
    public int Row;
    public int Col;
    public int FaceSize;
    public Dictionary<char, Neighbour> Neighbours = new Dictionary<char, Neighbour>();

    public Face(int col, int row, int faceSize)
    {
        Row = row;
        Col = col;
        FaceSize = faceSize;
    }

    internal void AddNeighbours(string v)
    {
        for (int i = 0; i < 12; i += 3)
        {
            Neighbours[v[i]] = new Neighbour(v.Substring(i, 3));
        }
    }
}

public class Neighbour
{
    public int Face;
    public int Rotation; // 0, 1, 2, 3 clockwise
    public char DestSide;

    public Neighbour(string input)
    {
        Face = input[1] - '1';
        Rotation = GetRotation(input[0], input[2]);
        DestSide = input[2];
    }

    private int GetRotation(char sourceSide, char destinationSide)
    {
        var sides = new List<char> { 'U', 'R', 'D', 'L' };
        return (sides.IndexOf(sourceSide) - sides.IndexOf(destinationSide) + 4 + 2) % 4;
    }
}

internal class Position2
{
    public int Col;
    public int Row;
    public int Face;
    public int Direction;
    public Position2(int startingCol)
    {
        Col = startingCol;
    }

    internal int Answer()
        => (Row + 1) * 1000 + (Col + 1) * 4 + Direction;

    internal void UpdatePosition(int newCol, int newRow, int newFace, int newDir)
    {
        Col = newCol;
        Row = newRow;
        Face = newFace;
        Direction = newDir;
    }

    internal (int, int, int, int, bool) NexPos(char[,] forest, List<Face> faces)
    {
        var newCol = Col + Delta().Item1;
        var newRow = Row + Delta().Item2;
        var currentFace = faces[Face];
        (newCol, newRow, int newDir, int newFace) = (newCol, newRow) switch
        {
            (_, _) when newCol > currentFace.MaxCol => ApplyTranslation(currentFace, 'R', faces),
            (_, _) when newRow > currentFace.MaxRow => ApplyTranslation(currentFace, 'D', faces),
            (_, _) when newCol < currentFace.MinCol => ApplyTranslation(currentFace, 'L', faces),
            (_, _) when newRow < currentFace.MinRow => ApplyTranslation(currentFace, 'U', faces),
            (_, _) => (newCol, newRow, Direction, Face)
        };
        return (newCol, newRow, newFace, newDir, forest[newCol, newRow] is '.');
    }

    private (int, int) Delta()
        => (Direction) switch
        {
            0 => (1, 0),
            1 => (0, 1),
            2 => (-1, 0),
            3 => (0, -1),
            _ => throw new InvalidDataException()
        };

    private (int newCol, int newRow, int newDir, int newFace) ApplyTranslation(Face currentFace, char soure, List<Face> faces)
    {
        var neighbour = currentFace.Neighbours[soure];
        var newFaceint = neighbour.Face;
        var newDir = GetNewDir(neighbour.DestSide);
        var relCol = Col - currentFace.MinCol;
        var relRow = Row - currentFace.MinRow;
        var faceSizeOffset = currentFace.FaceSize - 1;
        (var newCol, var newRow) = neighbour.Rotation switch
        {
            1 => (faceSizeOffset - relRow, relCol),
            2 => (faceSizeOffset - relCol, faceSizeOffset - relRow),
            3 => (relRow, faceSizeOffset - relCol),
            _ => (relCol % faceSizeOffset, relRow % faceSizeOffset)
        };
        var newFace = faces[newFaceint];
        var absCol = newCol + newFace.MinCol;
        var absRow = newRow + newFace.MinRow;
        return (absCol, absRow, newDir, newFaceint);
    }

    private int GetNewDir(char destSide)
        => destSide switch
        {
            'R' => 2,
            'U' => 1,
            'D' => 3,
            _ => 0
        };

    internal void Rotate(char rotationDirection)
        => Direction = (Direction + 4 + (rotationDirection == 'R' ? 1 : -1)) % 4;
}

internal class Position
{
    public int Col;
    public int Row;
    public Direction Direction;
    public Position(int startingCol)
    {
        Col = startingCol;
        Direction = new Direction(1, 0);
    }

    internal int Answer()
        => (Row + 1) * 1000 + (Col + 1) * 4 + Direction.Answer();

    internal void UpdatePosition(int newCol, int newRow)
    {
        Col = newCol;
        Row = newRow;
    }

    internal (int, int, bool) NexPos(int maxCol, int maxRow, char[,] forest)
    {
        var newCol = (Col + Direction.DeltaCol + maxCol) % maxCol;
        var newRow = (Row + Direction.DeltaRow + maxRow) % maxRow;
        while (forest[newCol, newRow] is not '#' and not '.')
        {
            newCol = (newCol + Direction.DeltaCol + maxCol) % maxCol;
            newRow = (newRow + Direction.DeltaRow + maxRow) % maxRow;
        }
        return (newCol, newRow, forest[newCol, newRow] is '.');
    }
}

public class Direction
{
    public int DeltaCol;
    public int DeltaRow;

    public Direction(int d1, int d2)
    {
        DeltaCol = d1;
        DeltaRow = d2;
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