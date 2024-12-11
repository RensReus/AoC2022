namespace AoC2024;

class Day11 : BaseDay
{
    [Example(expected: 55312, input: "125 17")]
    [Puzzle(expected: 218956)]
    public static long Part1(string input)
        => Blink(input, 25);

    private static long Blink(string input, int blinks)
    {
        var stones = new Dictionary<long, long>();
        foreach (var stone in input.Split().Select(long.Parse))
        {
            stones[stone] = 1;
        }
        for (int i = 0; i < blinks; i++)
        {
            var newStones = new Dictionary<long, long>();
            foreach (var stone in stones)
            {
                var next = GetNextStones(stone.Key);
                foreach (var nextStone in next)
                {
                    if (!newStones.ContainsKey(nextStone))
                    {
                        newStones[nextStone] = 0;
                    }
                    newStones[nextStone] += stone.Value;
                }
            }
            stones = newStones;
        }
        return stones.Values.Sum();
    }

    private static IEnumerable<long> GetNextStones(long key)
    {
        if (key == 0) return [1];

        var stringStone = key.ToString();
        if (stringStone.Length % 2 == 0)
        {
            var half = stringStone.Length / 2;
            var firstHalf = stringStone[..half];
            var secondHalf = stringStone[half..];
            return [long.Parse(firstHalf), long.Parse(secondHalf)];
        }

        return [key * 2024];
    }

    [Puzzle(expected: 259593838049805)]
    public static long Part2(string input)
        => Blink(input, 75);
}