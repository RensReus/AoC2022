using System.Text.RegularExpressions;

namespace AoC2022;

static class Day16
{
    static IDictionary<string, Valve> ProcessInput(string input)
    {
        return input.Split(";").ToList().Select(x => new Valve(x)).ToDictionary(x => x.Name, x => x);
    }

    [Example(expected: 1651, input: 1)]
    [Puzzle(expected: 1871)]
    public static int Part1(string input)
    {
        var valves = ProcessInput(input);
        var evaluatedValves = valves.ToDictionary(valve => valve.Key, valve => GetAllRoutes(valve.Value, valves));
        return GetFinishedRoutes(evaluatedValves, 30).Max(x => x.Value);
    }

    [Example(expected: 1707, input: 1)]
    [Puzzle(expected: 2416)]
    public static int Part2(string input)
    {
        var valves = ProcessInput(input);
        var evaluatedValves = valves.ToDictionary(valve => valve.Key, valve => GetAllRoutes(valve.Value, valves));
        var routes = GetFinishedRoutes(evaluatedValves, 26).ToList();
        var maxPressure = 0;
        for (int i = 0; i < routes.Count; i++)
        {
            for (int j = i + 1; j < routes.Count; j++)
            {
                if (routes[i].Key.Visited.Overlaps(routes[j].Key.Visited)) continue;
                maxPressure = int.Max(maxPressure, routes[i].Value + routes[j].Value);
            }
        }
        return maxPressure;
    }

    private static Dictionary<State, int> GetFinishedRoutes(IDictionary<string, Valve> valves, int maxMinutes)
    {
        var total = valves.Count(valve => valve.Value.FlowRate > 0);
        var totalFlowRate = valves.Sum(valve => valve.Value.FlowRate);
        var toEvalDict = new Dictionary<int, List<State>> { { 0, new List<State> { new State("AA", 0) } } };
        var possibleRoutes = new Dictionary<State, int>();
        for (int i = 0; i < total; i++)
        {
            toEvalDict[i + 1] = new List<State>();
            foreach (var state in toEvalDict[i])
            {
                foreach (var dest in valves[state.Current].DestinationsDetails)
                {
                    if (state.Minute + dest.Value >= maxMinutes || state.Visited.Contains(dest.Key)) continue;
                    var newState = state.Move(dest, valves);
                    toEvalDict[i + 1].Add(newState);
                    possibleRoutes[newState] = newState.PressureReleased + newState.FlowRate * (maxMinutes - newState.Minute);
                }
            }
        }
        return possibleRoutes;
    }

    private static Valve GetAllRoutes(Valve valve, IDictionary<string, Valve> valves)
    {
        var toEval = valve.Destinations.Select(x => x);
        var evaluated = new Dictionary<string, int> { { valve.Name, 0 } };
        var steps = 0;
        while (toEval.Any())
        {
            steps += 1;
            var newToEval = new List<string>();
            foreach (var dest in toEval)
            {
                evaluated[dest] = steps;
                foreach (var dest2 in valves[dest].Destinations)
                {
                    if (evaluated.ContainsKey(dest2) || newToEval.Contains(dest2)) continue;
                    newToEval.Add(dest2);
                }
            }
            toEval = newToEval;
        }
        valve.DestinationsDetails = evaluated.Where(x => valves[x.Key].FlowRate > 0 && x.Key != valve.Name).ToDictionary(y => y.Key, y => y.Value + 1);
        return valve;
    }
}

internal class State
{
    public string Current;
    internal int Minute;
    public HashSet<string> Visited = new HashSet<string>();
    public int FlowRate;
    public int PressureReleased;

    public State(string current, int minute)
    {
        Current = current;
        Minute = minute;
    }

    public State(string current, int minute, int flowrate, int pressureReleased, HashSet<string> visited)
    {
        Current = current;
        Minute = minute;
        Visited = visited;
        FlowRate = flowrate;
        PressureReleased = pressureReleased;
    }

    internal State Move(KeyValuePair<string, int> dest, IDictionary<string, Valve> valves)
    {
        var newVisited = new HashSet<string> { dest.Key };
        newVisited.UnionWith(Visited);
        return new State(dest.Key, Minute + dest.Value, FlowRate + valves[dest.Key].FlowRate, PressureReleased + FlowRate * dest.Value, newVisited);
    }

    public override bool Equals(object? other)
        => other is State s && s.Current == Current && s.Visited.SetEquals(Visited);

    public override int GetHashCode()
    {
        var hashCode = Current.GetHashCode();
        foreach (var valve in Visited)
        {
            hashCode = HashCode.Combine(hashCode, valve);
        }
        return hashCode;
    }
}

public class Valve
{
    public string Name;
    public int FlowRate;
    public List<string> Destinations;
    public Dictionary<string, int> DestinationsDetails = new Dictionary<string, int>();

    public Valve(string line)
    {
        var groups = Regex.Match(line, @"Valve (.+) has flow rate=(.+)@ tunnels? leads? to valves? (.+)").Groups;
        Name = groups[1].Value;
        FlowRate = int.Parse(groups[2].Value);
        Destinations = groups[3].Value.Split(", ").ToList();
    }
}