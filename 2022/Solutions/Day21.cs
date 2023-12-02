namespace AoC2022;

class Day21 : BaseDay
{
    static Dictionary<string, Monkey21> ProcessInput(string input)
        => input.Split("\n").ToDictionary(x => x.Split(":")[0], x => new Monkey21(x));

    [Example(expected: 152, input: "root: pppw + sjmn\ndbpl: 5\ncczh: sllz + lgvd\nzczc: 2\nptdq: humn - dvpt\ndvpt: 3\nlfqf: 4\nhumn: 5\nljgn: 2\nsjmn: drzm * dbpl\nsllz: 4\npppw: cczh / lfqf\nlgvd: ljgn * ptdq\ndrzm: hmdt - zczc\nhmdt: 32")]
    [Puzzle(expected: 22382838633806)]
    public static long Part1(string input)
    {
        var monkeys = ProcessInput(input);
        return monkeys["root"].GetValue(monkeys);
    }

    [Example(expected: 301, input: "root: pppw + sjmn\ndbpl: 5\ncczh: sllz + lgvd\nzczc: 2\nptdq: humn - dvpt\ndvpt: 3\nlfqf: 4\nhumn: 5\nljgn: 2\nsjmn: drzm * dbpl\nsllz: 4\npppw: cczh / lfqf\nlgvd: ljgn * ptdq\ndrzm: hmdt - zczc\nhmdt: 32")]
    [Puzzle(expected: 3099532691300)]
    public static long Part2(string input)
    {
        var monkeys = ProcessInput(input);
        monkeys["root"].Operation = "=";
        return monkeys["root"].GetHumanValue(monkeys, 0);
    }
}

internal class Monkey21
{
    public string Arg1;
    public string Arg2;
    public string Operation;
    private long? Value;
    public string Name;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Monkey21(string line)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        var strings = line.Split(": ")[1].Split(" ");
        Name = line.Split(": ")[0];
        if (strings.Length == 1)
        {
            Value = long.Parse(strings[0]);
        }
        else
        {
            Arg1 = strings[0];
            Operation = strings[1];
            Arg2 = strings[2];
        }
    }

    public long GetValue(Dictionary<string, Monkey21> monkeys)
        => Value is long v2 ? v2 : EvaluateOperation(monkeys);

    private long EvaluateOperation(Dictionary<string, Monkey21> monkeys)
    {
        var val1 = monkeys[Arg1].GetValue(monkeys);
        var val2 = monkeys[Arg2].GetValue(monkeys);
        Value = Operation switch
        {
            "+" => val1 + val2,
            "-" => val1 - val2,
            "*" => val1 * val2,
            "/" => val1 / val2,
            _ => throw new NotImplementedException()
        };
        return (long)Value;
    }

    public long? GetValue2(Dictionary<string, Monkey21> monkeys)
    {
        if (Name is "humn") return null;
        if (Value is long v2) return v2;
        if (monkeys[Arg1].GetValue2(monkeys) is null || monkeys[Arg2].GetValue2(monkeys) is null) return null;
        return EvaluateOperation(monkeys);
    }

    internal long GetHumanValue(Dictionary<string, Monkey21> monkeys, long requiredValue)
    {
        var m1 = monkeys[Arg1].GetValue2(monkeys);
        var firstUnknown = m1 is null;
        var knownValue = m1 ?? monkeys[Arg2].GetValue2(monkeys) ?? throw new ArgumentNullException("only one can be null");
        var newRequired = Operation switch
        {
            "+" => requiredValue - knownValue,
            "-" => firstUnknown ? requiredValue + knownValue : knownValue - requiredValue,
            "*" => requiredValue / knownValue,
            "/" => firstUnknown ? requiredValue * knownValue : knownValue / requiredValue, // De tweede komt niet voor
            "=" => knownValue,
            _ => throw new NotImplementedException()
        };
        if (Arg1 == "humn" || Arg2 == "humn") return newRequired;
        return monkeys[firstUnknown ? Arg1 : Arg2].GetHumanValue(monkeys, newRequired);
    }
}