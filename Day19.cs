using System.Text.RegularExpressions;

namespace AoC2022;

static class Day19
{
    static IList<BluePrint> ProcessInput(string input)
    {
        return input.Split(";").Select(x => new BluePrint(x)).ToList();
    }

    [Example(expected: 33, input: "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.;Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.")]
    [Puzzle(expected: 222222)]
    public static int Part1(string input)
    {
        var bluePrints = ProcessInput(input);
        var mostGeode = bluePrints.Select(bluePrint => MostGeodes(bluePrint)).ToArray();
        return mostGeode.Select((x, i) => x * (i+1)).Sum();
    }

    private static int MostGeodes(BluePrint bluePrint)
    {
        var currStates = new HashSet<BPState> { new BPState() };
        var nextStates = new HashSet<BPState>();
        for (int minute = 0; minute < 24; minute++)
        {
            foreach (var state in currStates)
            {
                nextStates.UnionWith(GetNextStates(state, bluePrint));
            }
            if (nextStates.Count > 1_000_000)
            {
                Console.WriteLine("iets");
            }
            currStates = nextStates;
            nextStates = new HashSet<BPState>();
        }

        return currStates.Max(state => state.Resources["geode"]);
    }

    private static HashSet<BPState> GetNextStates(BPState state, BluePrint bluePrint)
    {
        var possibleRobots = bluePrint.RobotCosts.Where(robot => state.EnoughResources(robot.Value) && state.NeedsRobot(robot.Key, bluePrint));
        var newStates = possibleRobots.Select(robot => state.BuildRobot(robot)).ToHashSet();
        newStates.Add(state.IncrementResources());
        return newStates;
    }

    [Example(expected: 1111111, input: "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.;Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.")]
    [Puzzle(expected: 222222)]
    public static int Part2(string input)
    {
        var processedInput = ProcessInput(input);
        return 1111111;
    }
}

internal class BPState
{
    public Dictionary<string, int> Resources = new Dictionary<string, int> { { "ore", 0 }, { "clay", 0 }, { "obsidian", 0 }, { "geode", 0 } };
    public Dictionary<string, int> Robots = new Dictionary<string, int> { { "ore", 1 }, { "clay", 0 }, { "obsidian", 0 }, { "geode", 0 } };

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
        var newState = new BPState();
        newState.Resources = Resources.ToDictionary(entry => entry.Key, entry => entry.Value);
        newState.Robots = Robots.ToDictionary(entry => entry.Key, entry => entry.Value);
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
        return Robots["ore"] < 2;
    }

    private bool NeedsClayBot(BluePrint bluePrint)
    {
        // hebben we het punt bereikt dat we alleen nog maar obsidian/geode bots gaan maken
        // gaat ie sneller obsidian bot halen
        return Robots["clay"] < bluePrint.MaxClayBots;
    }

    private bool NeedsObsidianBot(BluePrint bluePrint)
    {
        // maximaal geoderecipe.obsidian/ geoderecipe.ore
        // gaat ie sneller geode bot halen
        // hebben we het punt bereikt dat we alleen nog maar geode bots gaan maken
        return Robots["clay"] < bluePrint.MaxClayBots;
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
        MaxClayBots = Int32.Parse(groups[4].Value) / Int32.Parse(groups[3].Value); // round up
        MaxObsidianBots = Int32.Parse(groups[6].Value) / Int32.Parse(groups[5].Value);  // round up
    }

}