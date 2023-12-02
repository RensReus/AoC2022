namespace AoC2022;

class Day10 : BaseDay
{
    [Example(expected: 13140, input: 1)]
    [Puzzle(expected: 11780)]
    public int Part1(string input)
    {
        var processedInput = ReadLines(input);
        var x = 1;
        var cycle = 1;
        int answer = 0;
        foreach (var line in processedInput)
        {
            var instr = line.Split();
            (answer, cycle) = UpdateAnswerCycle(answer, cycle, x);
            if (instr[0] == "addx")
            {
                (answer, cycle) = UpdateAnswerCycle(answer, cycle, x);
                x += int.Parse(instr[1]);
            }
        }
        return answer;
    }

    private (int answer, int cycle) UpdateAnswerCycle(int answer, int cycle, int x)
    {
        if ((cycle + 20) % 40 == 0)
        {
            answer += x * cycle;
        }
        return (answer, cycle + 1);
    }

    [Example(expected: "██  ██  ██  ██  ██  ██  ██  ██  ██  ██  \n███   ███   ███   ███   ███   ███   ███ \n████    ████    ████    ████    ████    \n█████     █████     █████     █████     \n██████      ██████      ██████      ████\n███████       ███████       ███████     \n", input: 1)]
    [Puzzle(expected: "███  ████ █  █ █    ███   ██  █  █  ██  \n█  █    █ █  █ █    █  █ █  █ █  █ █  █ \n█  █   █  █  █ █    ███  █  █ █  █ █  █ \n███   █   █  █ █    █  █ ████ █  █ ████ \n█    █    █  █ █    █  █ █  █ █  █ █  █ \n█    ████  ██  ████ ███  █  █  ██  █  █ \n")]
    public string Part2(string input)
    {
        var x = 1;
        var processedInput = ReadLines(input);
        var cycle = 1;
        var answer = "";
        foreach (var line in processedInput)
        {
            var instr = line.Split();
            (cycle, answer) = UpdateLineCycleanswer(cycle, answer, x);
            if (instr[0] == "addx")
            {
                (cycle, answer) = UpdateLineCycleanswer(cycle, answer, x);
                x += int.Parse(instr[1]);
            }
        }
        Console.WriteLine(answer);
        return answer;
    }

    private (int cycle, string answer) UpdateLineCycleanswer(int cycle, string answer, int x)
    {
        var horizontalPos = (cycle - 1) % 40;
        answer += x >= horizontalPos - 1 && x <= horizontalPos + 1 ? "█" : " ";
        if (cycle % 40 == 0)
        {
            answer += "\n";
        }
        return (cycle + 1, answer);
    }
}