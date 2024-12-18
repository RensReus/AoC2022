
namespace AoC2024;

class Day17 : BaseDay
{
    [Example(expected: "4,6,3,5,6,3,5,2,1,0", input: "Register A: 729\nRegister B: 0\nRegister C: 0\n\nProgram: 0,1,5,4,3,0")]
    [Example(expected: "4,2,5,6,7,7,7,7,3,1,0", input: "Register A: 2024\nRegister B: 0\nRegister C: 0\n\nProgram: 0,1,5,4,3,0")]
    [Example(expected: "2,4,1,1,7,5,0,3,1,4,4,4,5,5,3,0", input: "Register A: 202975183645226\nRegister B: 0\nRegister C: 0\n\nProgram: 2,4,1,1,7,5,0,3,1,4,4,4,5,5,3,0")]
    [Puzzle(expected: "6,1,6,4,2,4,7,3,5")]
    public static string Part1(string input)
    {
        var lines = ReadLinesDouble(input);

        var registers = new Dictionary<long, long> { [4] = 0, [5] = 0, [6] = 0 };
        for (int i = 0; i < 3; i++)
        {
            var line = lines[0][i];
            var value = long.Parse(line.Split(": ")[1]);
            registers[i + 4] = value;
        }
        var program = lines[1][0].Split(": ")[1].Split(',').Select(long.Parse).ToArray();
        return RunProgram(program, registers);
    }

    private static long GetInput(Dictionary<long, long> registers, long v)
    {
        if (v <= 3) return v;
        return registers[v];
    }

    private static long BitwiseXor(long v, long literal)
    {
        return v ^ literal;
    }

    private static long Divide(long numerator, long divisor)
    {
        return (long)(numerator / Math.Pow(2, divisor));
    }

    [Example(expected: 117440, input: "Register A: 2024\nRegister B: 0\nRegister C: 0\n\nProgram: 0,3,5,4,3,0")]
    [Puzzle(expected: 202975183645226)]
    public static long Part2(string input)
    {
        var lines = ReadLinesDouble(input);

        var registers = new Dictionary<long, long> { [4] = 0, [5] = 0, [6] = 0 };
        var programString = lines[1][0].Split(": ")[1];
        var program = programString.Split(',').Select(long.Parse).ToArray();
        var targets = program.Reverse().ToList();
        var options = new List<long> { 0 };
        for (int loop = 0; loop < targets.Count; loop++)
        {
            var target = targets[loop];
            var newOptions = new List<long>();
            foreach (var option in options)
            {
                for (long i = option * 8; i < (option * 8) + 8; i++)
                {
                    registers[4] = i;
                    var output = RunProgram(program, registers, true);
                    if (long.Parse(output) == target)
                    {
                        Console.WriteLine($"Found {i} for {target}");
                        newOptions.Add(i);
                    }
                }
            }
            options = newOptions;
        }

        return options.First();
    }

    private static string RunProgram(long[] program, Dictionary<long, long> registers, bool exitEarly = false)
    {
        var pos = 0L;
        var output = new List<long>();
        var allsteps = new List<string>();
        while (pos < program.Length)
        {
            var opcode = program[pos];
            var literal = program[pos + 1];
            var combo = GetInput(registers, literal);
            if (opcode is 3)
            {
                if (registers[4] == 0) pos += 2;
                else pos = literal;
                continue;
            }
            if (opcode is 5)
            {
                output.Add(combo % 8);
                if (exitEarly) return string.Join(',', output);
                pos += 2;
                continue;
            }
            var target = opcode switch
            {
                0 => 4,
                1 => 5,
                2 => 5,
                4 => 5,
                6 => 5,
                7 => 6
            };
            registers[target] = opcode switch
            {
                0 => Divide(registers[4], combo),
                1 => BitwiseXor(registers[5], literal),
                2 => combo % 8,
                4 => BitwiseXor(registers[5], registers[6]),
                6 => Divide(registers[4], combo),
                7 => Divide(registers[4], combo)
            };
            if (pos == 0)
            {
                allsteps.Add("Restart");
            }
            allsteps.Add($"{opcode},{literal} {registers[4]},{registers[5]},{registers[6]}");
            pos += 2;
        }
        return string.Join(',', output);
    }
}