namespace AoC2022;

static class Day21
{
    static Dictionary<string, Monkey21> ProcessInput(string input)
    {
        return input.Split(";").ToDictionary(x => x.Split(":")[0], x => new Monkey21(x));
    }

    [Example(expected: 152, input: "root: pppw + sjmn;dbpl: 5;cczh: sllz + lgvd;zczc: 2;ptdq: humn - dvpt;dvpt: 3;lfqf: 4;humn: 5;ljgn: 2;sjmn: drzm * dbpl;sllz: 4;pppw: cczh / lfqf;lgvd: ljgn * ptdq;drzm: hmdt - zczc;hmdt: 32")]
    [Puzzle(expected: 22382838633806)]
    public static long Part1(string input)
    {
        var monkeys = ProcessInput(input);
        return monkeys["root"].GetValue(monkeys);
    }

    [Example(expected: 301, input: "root: pppw + sjmn;dbpl: 5;cczh: sllz + lgvd;zczc: 2;ptdq: humn - dvpt;dvpt: 3;lfqf: 4;humn: 5;ljgn: 2;sjmn: drzm * dbpl;sllz: 4;pppw: cczh / lfqf;lgvd: ljgn * ptdq;drzm: hmdt - zczc;hmdt: 32")]
    [Puzzle(expected: 3099532691300)]
    public static long Part2(string input)
    {
        var monkeys = ProcessInput(input);
        var rootN1 = monkeys["root"].n1;
        var rootN2 = monkeys["root"].n2;
        var valN1 = monkeys[rootN1].GetValue2(monkeys);
        var valN2 = monkeys[rootN2].GetValue2(monkeys);
        var nullBranchName = valN1 is null ? rootN1 : rootN2;
        var requiredValue = valN1 ?? valN2 ?? 0;
        return monkeys[nullBranchName].GetHumanValue(monkeys, new HashSet<long> { requiredValue }).First();
    }
}

internal class Monkey21
{
    public string n1;
    public string n2;
    public string op;
    private long? value;
    public string Name;

    public Monkey21(string line)
    {
        var strings = line.Split(": ")[1].Split(" ");
        Name = line.Split(": ")[0];
        if (strings.Length == 1)
        {
            value = long.Parse(strings[0]);
        }
        else
        {
            n1 = strings[0];
            op = strings[1];
            n2 = strings[2];
        }
    }

    public long GetValue(Dictionary<string, Monkey21> monkeys)
    {
        if (value is long v2) return v2;
        var m1 = monkeys[n1].GetValue(monkeys);
        var m2 = monkeys[n2].GetValue(monkeys);
        value = op switch
        {
            "+" => m1 + m2,
            "-" => m1 - m2,
            "*" => m1 * m2,
            "/" => m1 / m2,
            _ => throw new NotImplementedException()
        };
        if (op == "/")
        {
            var a = 1;
        }
        return (long)value;
    }

    public long? GetValue2(Dictionary<string, Monkey21> monkeys)
    {
        if (Name is "humn") return null;
        if (value is long v2) return v2;
        var m1 = monkeys[n1].GetValue2(monkeys);
        var m2 = monkeys[n2].GetValue2(monkeys);
        if (m1 is null || m2 is null) return null;

        value = op switch
        {
            "+" => m1 + m2,
            "-" => m1 - m2,
            "*" => m1 * m2,
            "/" => m1 / m2,
            _ => throw new NotImplementedException()
        };
        return (long)value;
    }

    internal HashSet<long> GetHumanValue(Dictionary<string, Monkey21> monkeys, HashSet<long> requiredValues)
    {
        var m1 = monkeys[n1].GetValue2(monkeys);
        var m2 = monkeys[n2].GetValue2(monkeys);
        var firstUnknown = m1 is null;
        var knownValue = m1 ?? m2 ?? 0;
        var newRequireds = op switch
        {
            "+" => requiredValues.Select(x => x - knownValue),
            "-" => GetRequiredMinus(firstUnknown, knownValue, requiredValues),
            "*" => GetRequiredMulti(firstUnknown, knownValue, requiredValues),
            "/" => GetRequiredDivid(firstUnknown, knownValue, requiredValues),
            _ => throw new NotImplementedException()
        };
        if (n1 == "humn" || n2 == "humn") return newRequireds.ToHashSet();
        return monkeys[firstUnknown ? n1 : n2].GetHumanValue(monkeys, newRequireds.ToHashSet());
    }

    private IEnumerable<long> GetRequiredMinus(bool firstUnknown, long knownValue, HashSet<long> requiredValues)
    {
        return (firstUnknown ? requiredValues.Select(x => x + knownValue) : requiredValues.Select(x => -1 * (x - knownValue)));
    }

    private IEnumerable<long> GetRequiredDivid(bool firstUnknown, long knownValue, HashSet<long> requiredValues)
    {
        return firstUnknown ? requiredValues.SelectMany(x => GetPossibleDivide(x, knownValue)) : requiredValues.Select(x => x * knownValue);
    }

    private IEnumerable<long> GetPossibleDivide(long requiredValue, long knownValue)
    {
        // wss alleen de laagste nodig en dan is de hele list overbodig
        return Enumerable.Range(0, (int)knownValue).Select(x => (requiredValue * knownValue) + x);
    }

    private IEnumerable<long> GetRequiredMulti(bool firstUnknown, long knownValue, HashSet<long> requiredValues)
    {
        return requiredValues.Select(x => x / knownValue);
    }
}