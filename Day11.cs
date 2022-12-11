using System.Data;
using AoC2022.Days;

namespace AoC2022.Days11;

class Day : BaseDay
{
    static IList<Monkey> ProcessInput(string filename)
        => ReadFile("11/" + filename, "\r\n\r\n").Select(x => new Monkey(x)).ToList();

    public override int Part1(string filename)
    {
        var monkeys = ProcessInput(filename);
        for (int i = 0; i < 20; i++)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Any())
                {
                    (int destination, int item) = monkey.GetDestinationItem();
                    monkeys[destination].Items.Enqueue(item);
                }
            }
        }
        monkeys = monkeys.OrderByDescending(x => x.EvaluatedItems).ToList();
        return monkeys[0].EvaluatedItems * monkeys[1].EvaluatedItems;
    }

    public override int Part2(string filename)
    {
        var monkeys = ProcessInput(filename);
        var extraModulo = 1;
        foreach (var monkey in monkeys)
        {
            extraModulo *= monkey.TestValue;
        }
        for (int i = 0; i < 10000; i++)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Any())
                {
                    (int destination, int item) = monkey.GetDestinationItem2(extraModulo);
                    monkeys[destination].Items.Enqueue(item);
                }
            }
        }
        monkeys = monkeys.OrderByDescending(x => x.EvaluatedItems).ToList();
        Console.WriteLine(Convert.ToInt64(monkeys[0].EvaluatedItems) * Convert.ToInt64(monkeys[1].EvaluatedItems));
        return monkeys[0].EvaluatedItems * monkeys[1].EvaluatedItems;
    }

    public override List<Case> Part1Cases() => new() { new("1a", 10605) };

    public override List<Case> Part2Cases() => new() { new("1a", 10605) };
}

internal class Monkey
{
    public Queue<int> Items = new Queue<int>();
    public string Operation;
    public int TestValue;
    public int TargetIfTrue;
    public int TargetIfFalse;
    public int EvaluatedItems;
    public Monkey(string x)
    {
        var lines = x.Split("\r\n");
        var items = lines[1].Split(":")[1].Split(",").Select(x => int.Parse(x));
        foreach (var item in items)
        {
            Items.Enqueue(item);
        }
        Operation = lines[2].Split("= ")[1];
        TestValue = int.Parse(lines[3].Split().Last());
        TargetIfTrue = int.Parse(lines[4].Split().Last());
        TargetIfFalse = int.Parse(lines[5].Split().Last());
    }
    public (int, int) GetDestinationItem()
    {
        EvaluatedItems++;
        var worry = Items.Dequeue();
        var op = Operation.Replace("old", worry.ToString());
        DataTable dt = new DataTable();
        worry = (int)dt.Compute(op, "") / 3;
        return (worry % TestValue == 0 ? TargetIfTrue : TargetIfFalse, worry);
    }

    public (int, int) GetDestinationItem2(int extraModulo)
    {
        EvaluatedItems++;
        var worry = Items.Dequeue();
        if (Operation == "old * old") // is net te groot voor int omdat extraModulo^2 net te groot was
        {
            long old = Convert.ToInt64(worry);
            long tempWorry = old * old;
            worry = (int)(tempWorry % extraModulo);
        }
        else
        {
            var op = Operation.Replace("old", worry.ToString());
            DataTable dt = new DataTable();
            worry = ((int)dt.Compute(op, "")) % extraModulo;
        }
        return (worry % TestValue == 0 ? TargetIfTrue : TargetIfFalse, worry);
    }
}