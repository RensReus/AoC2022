using System.Text.RegularExpressions;
using AoC2022.Days;

namespace AoC2022.Days05;

class Day : BaseDay
{
    static Tuple<List<List<char>>, IEnumerable<Instruction>> ProcessInput(string filename)
    {
        var a = ReadFileNoTrim("05/" + filename, "\r\n\r\n");
        var initialstate = a[0].Split("\r\n");
        var stacks = initialstate.Last().Split("   ").Select(x => new List<char>()).ToList();
        foreach (var line in initialstate.Take(initialstate.Length - 1).Reverse())
        {
            for (int i = 0; i < stacks.Count(); i++)
            {
                var box = line[i * 4 + 1];
                if (box != ' ')
                {
                    stacks[i].Add(box);
                }
            }
        }

        var moves = a[1].Split("\r\n").Select(line => new Instruction(Regex.Match(line, @"move (\d+) from (\d+) to (\d+)").Groups));
        return new Tuple<List<List<char>>, IEnumerable<Instruction>>(stacks, moves);
    }

    public override int Part1(string filename)
    {
        var input = ProcessInput(filename);
        var stacks = input.Item1;
        var moves = input.Item2;
        foreach (var move in moves)
        {
            for (int i = 0; i < move.Amount; i++)
            {
                stacks[move.Destination].Add(stacks[move.Source].Last());
                stacks[move.Source].RemoveAt(stacks[move.Source].Count() - 1);
            }
        }
        Console.WriteLine(new string(stacks.Select(x => x.Last()).ToArray()));
        int answer = 0;
        return answer;
    }

    public override int Part2(string filename)
    {
        var input = ProcessInput(filename);
        var stacks = input.Item1;
        var moves = input.Item2;
        foreach (var move in moves)
        {
            var temp = new List<char>();
            for (int i = 0; i < move.Amount; i++)
            {
                temp.Add(stacks[move.Source].Last());
                stacks[move.Source].RemoveAt(stacks[move.Source].Count() - 1);
            }
            temp.Reverse();
            foreach (var item in temp)
            {
                stacks[move.Destination].Add(item);
            }
        }
        Console.WriteLine(new string(stacks.Select(x => x.Last()).ToArray()));
        int answer = 0;
        return answer;
    }

    public override List<Case> Part1Cases() => new() { new("1a", 1111111111) };

    public override List<Case> Part2Cases() => new() { };
}

internal class Instruction
{
    public int Amount;
    public int Source;
    public int Destination;

    public Instruction(GroupCollection groups)
    {
        Amount = Int32.Parse(groups[1].Value);
        Source = Int32.Parse(groups[2].Value) - 1;
        Destination = Int32.Parse(groups[3].Value) - 1;
    }
}
