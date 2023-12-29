namespace AoC2022;

class Day19 : BaseDay
{
    private static List<BluePrint> ProcessInput(string input)
    {
        return input.Split("\n").Select(x => new BluePrint(x)).ToList();
    }

    [Puzzle(expected: 1389)]
    [Example(expected: 33, input: "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.\nBlueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.")]
    public static int Part1(string input)
    {
        var bluePrints = ProcessInput(input);
        var mostGeode = bluePrints.Select(x => MostGeodes(x, 24));
        return mostGeode.Select((x, i) => x * (i + 1)).Sum();
    }

    private static int MostGeodes(BluePrint bluePrint, int maxMinutes)
    {
        var currStates = new HashSet<BPState> { new() };
        var nextStates = new HashSet<BPState>();
        var start = DateTime.Now;
        for (int minute = 1; minute < maxMinutes; minute++)
        {
            if ((DateTime.Now - start).TotalSeconds > 30)
            {
                Console.WriteLine("TIME TIME TIME");
                break;
            }
            if (currStates.Count > 1_000_000 && minute != maxMinutes - 1)
            {
                Console.WriteLine($"{minute}, {currStates.Count}");
                Console.WriteLine("COUNT COUNT COUNT");
                break;
            }

            //if (currStates.Count > 10)
            //{
            //    Console.WriteLine($"{minute}, {nextStates.Count}");

            //}
            foreach (var state in currStates)
            {
                var n = GetNextStates(state, bluePrint);
                nextStates.UnionWith(n);
            }

            currStates = Filter(nextStates);
            nextStates = [];
        }
        currStates = currStates.Select(state => state.IncrementResources()).ToHashSet();
        var max = currStates.Max(state => state.Resources["geode"]);
        Console.WriteLine($"\nDone, {currStates.Count}");
        Console.WriteLine("max " + max);

        var bestStates = currStates.Where(state => state.Resources["geode"] == max);
        return max;
    }

    private static HashSet<BPState> Filter(HashSet<BPState> nextStates)
    {
        // dit maar efficienter
        var filtered = new HashSet<BPState>();
        foreach (var it in nextStates)
        {
            if (!filtered.Any(x => XBetter(x, it))) filtered.Add(it);
        }
        return filtered;
    }

    private static bool XBetter(BPState x, BPState value)
    {
        foreach (var item in x.Resources)
        {
            if (item.Value < value.Resources[item.Key]) return false;
        }
        foreach (var item in x.Robots)
        {
            if (item.Value < value.Robots[item.Key]) return false;
        }
        return true;
    }

    private static HashSet<BPState> GetNextStates(BPState state, BluePrint bluePrint)
    {
        var possibleRobots = bluePrint.RobotCosts.Where(robot => state.EnoughResources(robot.Value) && state.NeedsRobot(robot.Key, bluePrint));
        var newStates = possibleRobots.Select(state.BuildRobot).ToHashSet();
        if (state.DoNothingIsUsefull(bluePrint)) newStates.Add(state.IncrementResources());
        return newStates;
    }

    [Example(expected: 3472, input: "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.\nBlueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.")]
    [Puzzle(expected: 222222)]
    public static int Part2(string input)
    {
        var bluePrints = ProcessInput(input);
        var mostGeode = bluePrints.Take(3).Select(x => MostGeodes(x, 32));
        return mostGeode.Aggregate((x, y) => x * y);
    }
}

internal class BPState
{
    public Dictionary<string, int> Resources = new() { { "ore", 0 }, { "clay", 0 }, { "obsidian", 0 }, { "geode", 0 } };
    public Dictionary<string, int> Robots = new() { { "ore", 1 }, { "clay", 0 }, { "obsidian", 0 }, { "geode", 0 } };

    public override int GetHashCode()
    {
        var hash = 0;
        foreach (var item in Resources)
        {
            hash = HashCode.Combine(hash, item.Key);
            hash = HashCode.Combine(hash, item.Value);
        }
        foreach (var item in Robots)
        {
            hash = HashCode.Combine(hash, item.Key);
            hash = HashCode.Combine(hash, item.Value);
        }
        return hash;
    }

    internal bool EnoughResources(Dictionary<string, int> costs)
        => costs.All(cost => Resources[cost.Key] >= cost.Value);

    internal BPState BuildRobot(KeyValuePair<string, Dictionary<string, int>> robot)
    {
        var newState = Copy();
        foreach (var cost in robot.Value)
        {
            newState.Resources[cost.Key] -= cost.Value;
        }
        newState = newState.IncrementResources();
        newState.Robots[robot.Key] += 1;
        return newState;
    }

    private BPState Copy()
    {
        var newState = new BPState
        {
            Resources = Resources.ToDictionary(entry => entry.Key, entry => entry.Value),
            Robots = Robots.ToDictionary(entry => entry.Key, entry => entry.Value)
        };
        return newState;
    }

    internal BPState IncrementResources()
    {
        var newState = Copy();
        foreach (var item in Robots)
        {
            newState.Resources[item.Key] += item.Value;
        }
        return newState;
    }

    internal bool NeedsRobot(string key, BluePrint bluePrint)
        => false || key switch
        {
            "ore" => NeedsOreBot(bluePrint),
            "clay" => NeedsClayBot(bluePrint),
            "obsidian" => NeedsObsidianBot(bluePrint),
            _ => true
        };

    // huidige productie / recipe -> x bots per turn
    private bool NeedsOreBot(BluePrint bluePrint)
    {
        return Robots["ore"] < bluePrint.MaxOreBots;
    }

    private bool NeedsClayBot(BluePrint bluePrint)
    {
        // hebben we het punt bereikt dat we alleen nog maar obsidian/geode bots gaan maken
        // gaat ie sneller obsidian bot halen
        return Robots["clay"] < bluePrint.MaxClayBots;
    }

    private bool NeedsObsidianBot(BluePrint bluePrint)
    {
        // gaat ie sneller geode bot halen
        // hebben we het punt bereikt dat we alleen nog maar geode bots gaan maken
        return Robots["obsidian"] < bluePrint.MaxObsidianBots;
    }

    internal bool ShouldBuildNothing(BluePrint bluePrint)
    {
        return !EnoughResources(bluePrint.RobotCosts["geode"]);
    }

    public override string ToString()
    {
        return string.Join(",", Resources.Values) + " " + string.Join(",", Robots.Values);
    }

    internal bool DoNothingIsUsefull(BluePrint bp)
    {
        foreach (var cost in bp.RobotCosts.Values)
        {
            return cost.Any(cost2 => Resources[cost2.Key] < cost2.Value);
        }
        return true;
    }
}

internal class BluePrint
{
    public Dictionary<string, Dictionary<string, int>> RobotCosts = new Dictionary<string, Dictionary<string, int>>();
    public int MaxOreBots;
    public int MaxClayBots;
    public int MaxObsidianBots;

    public BluePrint(string line)
    {
        var groups = Regex.Match(line, @"Blueprint \d+: Each ore robot costs (\d+) ore\. Each clay robot costs (\d+) ore\. Each obsidian robot costs (\d+) ore and (\d+) clay\. Each geode robot costs (\d+) ore and (\d+) obsidian\.").Groups;
        RobotCosts["ore"] = new Dictionary<string, int> { { "ore", Int32.Parse(groups[1].Value) } };
        RobotCosts["clay"] = new Dictionary<string, int> { { "ore", Int32.Parse(groups[2].Value) } };
        RobotCosts["obsidian"] = new Dictionary<string, int> { { "ore", Int32.Parse(groups[3].Value) }, { "clay", Int32.Parse(groups[4].Value) } };
        RobotCosts["geode"] = new Dictionary<string, int> { { "ore", Int32.Parse(groups[5].Value) }, { "obsidian", Int32.Parse(groups[6].Value) } };
        MaxOreBots = RobotCosts.Max(x => x.Value["ore"]);
        MaxClayBots = Int32.Parse(groups[4].Value);
        MaxObsidianBots = Int32.Parse(groups[6].Value);
    }

}