using System.Text.RegularExpressions;

namespace AoC2022;

static class Day16
{
    static IDictionary<string, Valve> ProcessInput(string input)
    {
        var valves = input.Split(";").ToList().Select(x => new Valve(x)).ToDictionary(x => x.Name, x => x);
        return valves.ToDictionary(valve => valve.Key, valve => GetDistances(valve.Value, valves));
    }

    [Example(expected: 1651, input: 1)]
    [Puzzle(expected: 1871)]
    public static int Part1(string input)
        => GetFinishedRoutes(input, 30).Max(x => x.Value);

    [Example(expected: 1707, input: 1)]
    [Puzzle(expected: 2416)]
    public static int Part2(string input)
    {
        var routes = GetFinishedRoutes(input, 26).ToList();
        var maxPressure = 0;
        for (int i = 0; i < routes.Count; i++)
        {
            var key1 = routes[i].Key;
            var val1 = routes[i].Value;
            for (int j = i + 1; j < routes.Count; j++)
            {
                var total = val1 + routes[j].Value;
                if ((key1 & routes[j].Key) > 0 || total <= maxPressure) continue;
                maxPressure = total;
            }
        }
        return maxPressure;
    }

    private static Dictionary<int, int> GetFinishedRoutes(string input, int maxMinutes)
    {
        var valves = ProcessInput(input);
        var valvesNumeric = valves["AA"].DestinationsDetails.Select((name, index) => (name.Key, index)).ToDictionary(x => x.Key, x => (int)Math.Pow(2, x.index));
        var maxOpenValves = valvesNumeric.Count();
        var toEvalDict = new Dictionary<int, List<State>> { { 0, new List<State> { new State("AA", 0) } } };
        var possibleRoutes = new Dictionary<int, int>();
        for (int i = 0; i < maxOpenValves; i++)
        {
            toEvalDict[i + 1] = new List<State>();
            foreach (var state in toEvalDict[i])
            {
                foreach (var dest in valves[state.Current].DestinationsDetails)
                {
                    if (state.Minute + dest.Value >= maxMinutes || (state.Visited & valvesNumeric[dest.Key]) == valvesNumeric[dest.Key]) continue;
                    var newState = state.Move(dest, valves, valvesNumeric);
                    var newPress = newState.PressureReleased + newState.FlowRate * (maxMinutes - newState.Minute);
                    toEvalDict[i + 1].Add(newState);
                    if (possibleRoutes.TryGetValue(newState.Visited, out int oldPress) && oldPress > newPress) continue;
                    possibleRoutes[newState.Visited] = newPress;
                }
            }
        }
        return possibleRoutes;
    }

    private static Valve GetDistances(Valve valve, IDictionary<string, Valve> valves)
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
    public int Visited;
    public int FlowRate;
    public int PressureReleased;

    public State(string current, int minute, int flowrate = 0, int pressureReleased = 0, int visited = 0)
    {
        Current = current;
        Minute = minute;
        Visited = visited;
        FlowRate = flowrate;
        PressureReleased = pressureReleased;
    }

    internal State Move(KeyValuePair<string, int> dest, IDictionary<string, Valve> valves, IDictionary<string, int> valvesNumeric)
        => new State(dest.Key, Minute + dest.Value, FlowRate + valves[dest.Key].FlowRate, PressureReleased + FlowRate * dest.Value, Visited + valvesNumeric[dest.Key]);
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