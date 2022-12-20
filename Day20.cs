namespace AoC2022;

static class Day20
{
    [Example(expected: 3, input: "1;2;-3;3;-2;0;4")]
    [Puzzle(expected: 15297)]
    public static long Part1(string input)
        => CalculateAnswer(input, 1, 1);

    [Example(expected: 1623178306, input: "1;2;-3;3;-2;0;4")]
    [Puzzle(expected: 2897373276210)]
    public static long Part2(string input)
        => CalculateAnswer(input, 811589153, 10);

    private static long CalculateAnswer(string input, int key, int loops)
    {
        var items = ProcessInput(input, key);
        for (int i = 0; i < loops; i++)
        {
            var clean = items.Select(x => x.Value).ToList();
            items = Shuffle(items);
        }
        var zeroIndex = items.FindIndex(x => x.Value == 0);
        var length = items.Count;
        return items[(zeroIndex + 1000) % length].Value + items[(zeroIndex + 2000) % length].Value + items[(zeroIndex + 3000) % length].Value;
    }

    static List<MovedItem> ProcessInput(string input, int key)
        => input.Split(";").Select((x, i) => new MovedItem(x, i, key)).ToList();

    private static List<MovedItem> Shuffle(List<MovedItem> items)
    {
        var length = items.Count;
        for (int i = 0; i < length; i++)
        {
            var index = items.FindIndex(x => x.InitialPos == i);
            var toMove = items[index];
            items.RemoveAt(index);
            var newIndex = (index + toMove.Value) % (length - 1);
            if (newIndex < 0) newIndex += length - 1;
            items.Insert((int)newIndex, toMove);
        }
        return items;
    }
}

internal class MovedItem
{
    public int InitialPos;
    public long Value;
    public MovedItem(string value, int pos, long key)
    {
        InitialPos = pos;
        Value = int.Parse(value) * key;
    }
}