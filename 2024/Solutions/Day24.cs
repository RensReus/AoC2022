namespace AoC2024;

class Day24 : BaseDay
{
    [Example(expected: 4, input: "x00: 1\nx01: 1\nx02: 1\ny00: 0\ny01: 1\ny02: 0\n\nx00 AND y00 -> z00\nx01 XOR y01 -> z01\nx02 OR y02 -> z02")]
    [Example(expected: 2024, input: "x00: 1\nx01: 0\nx02: 1\nx03: 1\nx04: 0\ny00: 1\ny01: 1\ny02: 1\ny03: 1\ny04: 1\n\nntg XOR fgs -> mjb\ny02 OR x01 -> tnw\nkwq OR kpj -> z05\nx00 OR x03 -> fst\ntgd XOR rvg -> z01\nvdt OR tnw -> bfw\nbfw AND frj -> z10\nffh OR nrd -> bqk\ny00 AND y03 -> djm\ny03 OR y00 -> psh\nbqk OR frj -> z08\ntnw OR fst -> frj\ngnj AND tgd -> z11\nbfw XOR mjb -> z00\nx03 OR x00 -> vdt\ngnj AND wpb -> z02\nx04 AND y00 -> kjc\ndjm OR pbm -> qhw\nnrd AND vdt -> hwm\nkjc AND fst -> rvg\ny04 OR y02 -> fgs\ny01 AND x02 -> pbm\nntg OR kjc -> kwq\npsh XOR fgs -> tgd\nqhw XOR tgd -> z09\npbm OR djm -> kpj\nx03 XOR y03 -> ffh\nx00 XOR y04 -> ntg\nbfw OR bqk -> z06\nnrd XOR fgs -> wpb\nfrj XOR qhw -> z04\nbqk OR frj -> z07\ny03 OR x01 -> nrd\nhwm AND bqk -> z03\ntgd XOR rvg -> z12\ntnw OR pbm -> gnj")]
    [Puzzle(expected: 52728619468518)]
    public static long Part1(string input)
    {
        var lines = ReadLinesDouble(input);
        var inputs = lines[0].Select(x => x.Split(": ")).ToDictionary(x => x[0], x => int.Parse(x[1]));
        var instructions = lines[1].Select(x => x.Split(" ")).Select(x => (A: x[0], Op: x[1], B: x[2], Out: x[4])).ToList();
        var inputs2 = Calculate(inputs, instructions);
        var z = inputs.Where(x => x.Key.StartsWith("z")).OrderByDescending(x => x.Key).Select(x => x.Value);
        return Convert.ToInt64(string.Join("", z), 2);
    }

    public static Dictionary<string, int> Calculate(Dictionary<string, int> inputs, List<(string A, string Op, string B, string Out)> instructions)
    {
        while (instructions.Count > 0)
        {
            // for each instruction
            foreach (var inst in instructions)
            {
                if (inputs.TryGetValue(inst.A, out var a) && inputs.TryGetValue(inst.B, out var b))
                {
                    inputs[inst.Out] = inst.Op switch
                    {
                        "AND" => a & b,
                        "OR" => a | b,
                        "XOR" => a ^ b,
                        _ => throw new InvalidOperationException($"Unknown operation {inst.Op}")
                    };
                    instructions.Remove(inst);
                    break;
                }
            }
        }
        return inputs;
    }

    [Example(expected: "z00,z01,z02,z05", input: "x00: 0\nx01: 1\nx02: 0\nx03: 1\nx04: 0\nx05: 1\ny00: 0\ny01: 0\ny02: 1\ny03: 1\ny04: 0\ny05: 1\n\nx00 AND y00 -> z05\nx01 AND y01 -> z02\nx02 AND y02 -> z01\nx03 AND y03 -> z03\nx04 AND y04 -> z04\nx05 AND y05 -> z00")]
    [Puzzle(expected: "1111111")]
    public static string Part2(string input)
    {
        var lines = ReadLinesDouble(input);
        var inputs = lines[0].Select(x => x.Split(": ")).ToDictionary(x => x[0], x => 0);
        var instructions = lines[1].Select(x => x.Split(" ")).Select(x => (A: x[0], Op: x[1], B: x[2], Out: x[4])).ToList();
        var max = int.Parse(inputs.Keys.OrderByDescending(x => x).First()[1..]);
        var invalidOutputs = new HashSet<string>();
        // x0 en y0 moeten een xor hebben die z0 oplevert
        // x0 en y0 moeten een and hebben die een overflow oplevert
        var overflow = "gct";

        for (int i = 1; i <= max; i++)
        {
            var x = $"x{i:D2}";
            var y = $"y{i:D2}";
            var z = $"z{i:D2}";
            // er moet een XOR zijn tussen x en y die de single digit geeft
            // er moet een AND zijn tussen x en y die een ioverflow oplevert
            // dan moet z een xor van singledigit en overflow zijn
            // en overflow moet een or zijn van ioverflow en overflow
            // maak het pad tussen x,y overflow en z
        }

        return "max";
    }
}