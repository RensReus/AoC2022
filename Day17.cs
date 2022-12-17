namespace AoC2022;

static class Day17
{
    static List<Block> Blocks => "####;;.#.;###;.#.;;..#;..#;###;;#;#;#;#;;##;##".Split(";;").Select(x => new Block(x)).ToList();

    [Example(expected: 3068, input: ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>")]
    [Puzzle(expected: 3141)]
    public static long Part1(string jets)
        => DropBlocks(jets, 2022);

    [Example(expected: 1514285714288, input: ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>")]
    [Puzzle(expected: 1561739130391)]
    public static long Part2(string jets)
        => DropBlocks(jets, 1_000_000_000_000);

    private static long DropBlocks(string jets, long totalBlocks)
    {
        int jetCounter = 0;
        long blockCounter = 0;
        long bonusLoopHeight = 0;
        long maxHeight = 0;

        var prevstates = new Dictionary<string, (long, long)>();
        var field = Enumerable.Range(0, 7).Select(x => new LongCoord(x, 0)).ToHashSet();

        while (blockCounter < totalBlocks)
        {
            int blockCounterMod = (int)(blockCounter % 5);
            var block = new Block(maxHeight, Blocks[blockCounterMod]);
            var cameToRest = false;
            while (!cameToRest)
            {
                block.JetMove(field, jets[jetCounter]);
                jetCounter = (jetCounter + 1) % jets.Length;

                cameToRest = block.TryDownMove(field, block);
            }

            field.UnionWith(block.Shape);
            maxHeight = long.Max(block.HighestPart(), maxHeight);
            blockCounter++;

            var stateString = StateString(field, maxHeight, blockCounterMod, jetCounter);
            if (prevstates.ContainsKey(stateString) && bonusLoopHeight == 0)
            {
                (var prevBlockCount, var prevHeight) = prevstates[stateString];
                var blocksPlaced = blockCounter - prevBlockCount;
                var heightAdded = maxHeight - prevHeight;

                var blocksLeft = totalBlocks - blockCounter;
                var loopsLeft = blocksLeft / blocksPlaced;

                bonusLoopHeight = loopsLeft * heightAdded;
                blockCounter += blocksPlaced * loopsLeft;
            }
            prevstates[stateString] = (blockCounter, maxHeight);
        }
        return maxHeight + bonusLoopHeight;
    }

    private static string StateString(HashSet<LongCoord> field, long maxHeight, long blockCounter, long jetCounter)
        => $"{blockCounter};{jetCounter};Surface {TopOfEachColumn(field, maxHeight)}";

    private static string TopOfEachColumn(HashSet<LongCoord> field, long maxHeight)
    {
        var output = "";
        // Could fail due to ignoring overhangs. But is fine for my inputs.
        for (int i = 0; i < 7; i++)
        {
            var searchHeight = maxHeight;
            while (!field.Contains(new(i, searchHeight)))
            {
                searchHeight--;
            }
            output += maxHeight - searchHeight + ";";
        }
        return output;
    }
}

internal class Block
{
    public HashSet<LongCoord> Shape;
    public Block(long maxHeight, Block block)
    {
        Shape = block.Shape.Select(c => c.Move(2, maxHeight + 4L)).ToHashSet();
    }

    public Block(string input)
    {
        Shape = new HashSet<LongCoord>();
        var inputLines = input.Split(";").Reverse().ToList();
        for (int y = 0; y < inputLines.Count; y++)
        {
            for (int x = 0; x < inputLines[y].Length; x++)
            {
                if (inputLines[y][x] == '#') Shape.Add(new LongCoord(x, y));
            }
        }
    }

    public long HighestPart()
        => Shape.Max(c => c.Y);

    internal void JetMove(HashSet<LongCoord> field, char v)
    {
        (int dx, int dy) = v == '<' ? (-1, 0) : (1, 0);
        var movedShape = Shape.Select(c => c.Move(dx, dy)).ToHashSet();
        if (movedShape.Any(c => c.X is < 0 or >= 7) || movedShape.Any(c => field.Contains(c))) return;
        Shape = movedShape;
    }

    internal bool TryDownMove(HashSet<LongCoord> field, Block block)
    {
        var movedShape = Shape.Select(c => c.Move(0, -1)).ToHashSet();
        var cameToRest = movedShape.Any(c => field.Contains(c));
        if (!cameToRest) Shape = movedShape;
        return cameToRest;
    }
}

internal class LongCoord
{
    public long X;
    public long Y;
    public LongCoord(long X, long Y)
    {
        this.X = X;
        this.Y = Y;
    }

    public override bool Equals(object? other)
        => other is LongCoord c && X == c.X && Y == c.Y;

    public override int GetHashCode()
        => HashCode.Combine(X, Y);

    public LongCoord Move(long dx, long dy)
        => new LongCoord(X + dx, Y + dy);
}
