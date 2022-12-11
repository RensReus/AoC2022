using System.Text.RegularExpressions;

namespace AoC2022;

class Day05
{
    static (List<List<char>>, IEnumerable<Instruction>) ProcessInput(string input)
    {
        var inputparts = input.Split(";;");
        var initialstate = inputparts[0].Split(";");
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

        var moves = inputparts[1].Split(";").Select(line => new Instruction(Regex.Match(line, @"move (\d+) from (\d+) to (\d+)").Groups));
        return (stacks, moves);
    }

    [Example(expected: "CMZ", input: 1)]
    [Puzzle(expected: "SVFDLGLWV")]
    public string Part1(string input)
    {
        (var stacks, var moves) = ProcessInput(input);
        foreach (var move in moves)
        {
            var start = stacks[move.Source].Count() - move.Amount;
            stacks[move.Destination].AddRange(stacks[move.Source].Skip(start).Reverse());
            stacks[move.Source].RemoveRange(start, move.Amount);
        }
        return new string(stacks.Select(x => x.Last()).ToArray());
    }

    [Example(expected: "MCD", input: 1)]
    [Puzzle(expected: "DCVTCVPCL")]
    public string Part2(string input)
    {
        (var stacks, var moves) = ProcessInput(input);
        foreach (var move in moves)
        {
            var start = stacks[move.Source].Count() - move.Amount;
            stacks[move.Destination].AddRange(stacks[move.Source].Skip(start));
            stacks[move.Source].RemoveRange(start, move.Amount);
        }
        return new string(stacks.Select(x => x.Last()).ToArray());
    }
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
