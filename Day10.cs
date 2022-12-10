using AoC2022.Days;

namespace AoC2022.Days10;

class Day : BaseDay
{
    static IList<String> ProcessInput(string filename)
        => ReadFile("10/" + filename);

    public override int Part1(string filename)
    {
        var input = ProcessInput(filename);
        var x = 1;
        var cycle = 1;
        int answer = 0;
        foreach (var line in input)
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

    public override int Part2(string filename)
    {
        var x = 1;
        var input = ProcessInput(filename);
        var cycle = 1;
        var currline = "";
        var output = new List<string>();
        foreach (var line in input)
        {
            var instr = line.Split();
            (currline, cycle, output) = UpdateLineCycleOutput(currline, cycle, output, x);
            if (instr[0] == "addx")
            {
                (currline, cycle, output) = UpdateLineCycleOutput(currline, cycle, output, x);
                x += int.Parse(instr[1]);
            }
        }

        return 1;
    }

    private (string currline, int cycle, List<string> output) UpdateLineCycleOutput(string currline, int cycle, List<string> output, int x)
    {
        var horizontalPos = (cycle - 1) % 40;
        currline += x >= horizontalPos - 1 && x <= horizontalPos + 1 ? "█" : " ";
        if (cycle % 40 == 0)
        {
            output.Add(currline);
            Console.WriteLine(currline);
            currline = "";
        }
        return (currline, cycle + 1, output);
    }

    public override List<Case> Part1Cases() => new() { new("1a", 13140), new("p1", 11780) };

    public override List<Case> Part2Cases() => new() { new("1a", 1) };
}