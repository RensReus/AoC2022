

namespace AoC2023;

class Day22 : BaseDay
{
    [Example(expected: 5, input: "1,0,1~1,2,1\n0,0,2~2,0,2\n0,2,3~2,2,3\n0,0,4~0,2,4\n2,0,5~2,2,5\n0,1,6~2,1,6\n1,1,8~1,1,9")]
    [Puzzle(expected: 473)]
    public static int Part1(string input)
    {
        var blocks = ReadLines(input).Select(x => new Block(x)).OrderBy(x => x.Start[2]).ToList();
        var occupiedCoords = new Dictionary<(int, int, int), int>();
        var blocksNotDestroy = new Dictionary<int, bool>();
        for (int i = 0; i < blocks.Count; i++)
        {
            var block = blocks[i];
            while (true)
            {
                if (block.Start[2] == 1) break;

                var blockDirectlyBelow = block.FindBlocksBelow(occupiedCoords);
                if (blockDirectlyBelow.Count > 0)
                {
                    if (blockDirectlyBelow.Count == 1)
                    {
                        blocksNotDestroy[blockDirectlyBelow.Single()] = true;
                    }
                    break;
                }
                block = block.MoveDown();
            }
            for (int x = block.Start[0]; x <= block.End[0]; x++)
            {
                for (int y = block.Start[1]; y <= block.End[1]; y++)
                {
                    occupiedCoords.Add((x, y, block.End[2]), i);
                }
            }
        }
        return blocks.Count - blocksNotDestroy.Count;
    }

    [Example(expected: 7, input: "1,0,1~1,2,1\n0,0,2~2,0,2\n0,2,3~2,2,3\n0,0,4~0,2,4\n2,0,5~2,2,5\n0,1,6~2,1,6\n1,1,8~1,1,9")]
    [Puzzle(expected: 61045)]
    public static int Part2(string input)
    {
        var blocks = ReadLines(input).Select(x => new Block(x)).OrderBy(x => x.Start[2]).ToList();
        var occupiedCoords = new Dictionary<(int, int, int), int>();
        var blockSupports = new Dictionary<int, List<int>>();
        var blockSupports2 = new Dictionary<int, List<int>>();
        for (int i = 0; i < blocks.Count; i++)
        {
            blockSupports2[i] = [];
            var block = blocks[i];
            while (true)
            {
                if (block.Start[2] == 1)
                {
                    blockSupports[i] = [-1];
                    break;
                }

                var blockDirectlyBelow = block.FindBlocksBelow(occupiedCoords);
                if (blockDirectlyBelow.Count > 0)
                {
                    blockSupports[i] = blockDirectlyBelow;
                    break;
                }
                block = block.MoveDown();
            }
            for (int x = block.Start[0]; x <= block.End[0]; x++)
            {
                for (int y = block.Start[1]; y <= block.End[1]; y++)
                {
                    occupiedCoords.Add((x, y, block.End[2]), i);
                }
            }
        }

        foreach (var blockSupport in blockSupports)
        {
            foreach (var support in blockSupport.Value)
            {
                if (support == -1) continue; 
                blockSupports2[support].Add(blockSupport.Key);
            }
        }

        var answer = 0;
        foreach (var blockSupport in blockSupports2)
        {
            answer += TotalCrashingDown(blockSupport, blockSupports);
        }
        return answer;
    }

    private static int TotalCrashingDown(KeyValuePair<int, List<int>> blockSupport, Dictionary<int, List<int>> blockSupports)
    {
        var supportsRemoved = new List<int> { blockSupport.Key };
        var shouldContinue = true;
        while (shouldContinue)
        {
            shouldContinue = false;
            foreach (var resting in blockSupports)
            {
                if (resting.Value.Any(x => !supportsRemoved.Contains(x)) || supportsRemoved.Contains(resting.Key)) continue;
                shouldContinue = true;
                supportsRemoved.Add(resting.Key);
            }
        }
        return blockSupports.Count(resting => resting.Value.All(supportsRemoved.Contains));
    }

    private record Block
    {
        public List<int> Start;
        public List<int> End;
        public Block(string line)
        {
            var (start, end) = (line.Split('~')[0].Split(','), line.Split('~')[1].Split(','));
            Start = start.Select(int.Parse).ToList();
            End = end.Select(int.Parse).ToList();
        }

        internal Block MoveDown()
        {
            var copy = (Block)MemberwiseClone();
            copy.Start[2]--;
            copy.End[2]--;
            if (copy.Start[2] < 1)
            {
                var a = 1;
            }
            return copy;
        }

        internal List<int> FindBlocksBelow(Dictionary<(int, int, int), int> occupiedCoords)
        {
            var blocksBelow = new List<int>();
            for (int x = Start[0]; x <= End[0]; x++)
            {
                for (int y = Start[1]; y <= End[1]; y++)
                {
                    if (occupiedCoords.TryGetValue((x, y, Start[2] - 1), out int i)) blocksBelow.Add(i);
                }
            }
            return blocksBelow.Distinct().ToList();
        }
    }
}