

namespace AoC2023;

class Day15 : BaseDay
{
    [Example(expected: 1320, input: "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7")]
    [Example(expected: 52, input: "HASH")]
    [Puzzle(expected: 514281)]
    public static int Part1(string input)
        => input.Split(',').Sum(CalculateHash);

    private static int CalculateHash(string arg)
    {
        var answer = 0;
        foreach (var c in arg)
        {
            answer += c;
            answer = answer * 17 % 256;
        }
        return answer;
    }

    [Example(expected: 145, input: "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7")]
    [Puzzle(expected: 244199)]
    public static int Part2(string input)
    {
        var boxes = Enumerable.Range(0, 256).ToDictionary(x => x, x => new List<(string, int)>());
        foreach (var block in input.Split(','))
        {
            var blockContents = block.Split(new char[] { '-', '=' }, StringSplitOptions.RemoveEmptyEntries);
            var label = blockContents[0];
            var id = CalculateHash(label);
            if (blockContents.Length > 1)
            {
                var strength = blockContents[1][0] - '0';
                var index = boxes[id].FindIndex(x => x.Item1 == label);
                if (index == -1) boxes[id].Add((label, strength));
                else boxes[id][index] = (boxes[id][index].Item1, strength);
            }
            else
            {
                var index = boxes[id].FindIndex(x => x.Item1 == label);
                if (index != -1) boxes[id].RemoveAt(index);
            }

        }
        return boxes.Sum(FocusPower);
    }

    private static int FocusPower(KeyValuePair<int, List<(string, int)>> pair)
        => pair.Value
            .Select((val, index) => (fLength: val.Item2, index: index + 1))
            .Sum(x => x.index * x.fLength)
            * (pair.Key + 1);
}