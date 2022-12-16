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
        var simplifiedValves = RemoveZeroValves(valves);
        var nonZeroValves = simplifiedValves.Count(x => x.Value.FlowRate > 0);
        var state = new State(0, 0, 0, nonZeroValves, new HashSet<string>(), simplifiedValves["AA"]);
        var evaluatedStates = new Dictionary<string, int>();
        (var answer, evaluatedStates) = EvaluateBranches(state, simplifiedValves, evaluatedStates);
        Console.WriteLine("evaluated states:" + evaluatedStates.Count);
        return answer;
    }

    private static IDictionary<string, Valve> RemoveZeroValves(IDictionary<string, Valve> valves)
    {
        var toRemove = valves.Where(valve => valve.Value.FlowRate == 0 && valve.Value.DestinationCosts.Count == 2).Select(x => x.Value);
        foreach (var simplifyValve in toRemove)
        {
            var simplifyName = simplifyValve.Name;
            var n0 = simplifyValve.DestinationCosts[0];
            var n1 = simplifyValve.DestinationCosts[1];
            var newStepCost = n0.Item2 + n1.Item2;
            var index0 = valves[n0.Item1].DestinationCosts.FindIndex(x => x.Item1 == simplifyName);
            valves[n0.Item1].DestinationCosts[index0] = (n1.Item1, newStepCost);
            var index1 = valves[n1.Item1].DestinationCosts.FindIndex(x => x.Item1 == simplifyName);
            valves[n1.Item1].DestinationCosts[index1] = (n0.Item1, newStepCost);
            valves.Remove(simplifyName);
        }
        return valves;
    }

    private static (int, Dictionary<string, int>) EvaluateBranches(State state, IDictionary<string, Valve> valves, Dictionary<string, int> evaluatedStates)
    {
        evaluatedStates[state.ToEvaluatedString()] = state.PressureReleased;
        if (state.Minute == 30)
        {
            return (state.PressureReleased, evaluatedStates);
        }

        var newStates = state.NextPossibleStates(valves, evaluatedStates);

        if (newStates.Count() == 0 || state.OpenedValves.Count() == state.NonZeroValves)
        {
            if (state.FlowRate >= 81)
            {
                return (state.PressureReleased + (30 - state.Minute) * state.FlowRate, evaluatedStates);
            }
            return (state.PressureReleased + (30 - state.Minute) * state.FlowRate, evaluatedStates);
        }
        var best = 0;
        foreach (var nextState in newStates)
        {
            var outcome = EvaluateBranches(nextState, valves, evaluatedStates);
            evaluatedStates = outcome.Item2;
            best = int.Max(outcome.Item1, best);
        }
        return (best, evaluatedStates);
    }

    [Example(expected: 1707, input: 1)]
    [Puzzle(expected: 222222)]
    public static int Part2(string input)
    {
        var valves = ProcessInput(input);
        var nonZeroValves = valves.Count(x => x.Value.FlowRate > 0);
        var state = new State2(0, 0, 0, nonZeroValves, new HashSet<string>(), valves["AA"], valves["AA"]);
        var evaluatedStates = new Dictionary<string, int>();
        (var answer, evaluatedStates) = EvaluateBranches2(state, valves, evaluatedStates);
        Console.WriteLine("evaluated states:" + evaluatedStates.Count);
        return answer;
    }

    private static (int, Dictionary<string, int>) EvaluateBranches2(State2 state, IDictionary<string, Valve> valves, Dictionary<string, int> evaluatedStates)
    {
        if (state.Minute == 26)
        {
            return (state.PressureReleased, evaluatedStates);
        }
        state.PressureReleased += state.FlowRate;
        var newStates = state.NextPossibleStates(valves, evaluatedStates);

        if (newStates.Count() == 0 || state.OpenedValves.Count() == state.NonZeroValves)
        {
            return (state.PressureReleased + (25 - state.Minute) * state.FlowRate, evaluatedStates);
        }
        var best = 0;
        foreach (var nextState in newStates)
        {
            var outcome = EvaluateBranches2(nextState.Copy(), valves, evaluatedStates);
            evaluatedStates = outcome.Item2;
            best = int.Max(outcome.Item1, best);
        }
        return (best, evaluatedStates);
    }
}

internal class State2
{
    public Valve[] Explorers;
    public int Minute;
    public int FlowRate;
    public int PressureReleased;
    public int NonZeroValves;
    public HashSet<string> OpenedValves;

    public State2(int minute, int flowRate, int pressureReleased, int nonZeroValves, HashSet<string> openedValves, Valve currValve, Valve elephantValve)
    {
        Minute = minute;
        FlowRate = flowRate;
        PressureReleased = pressureReleased;
        OpenedValves = new(openedValves);
        NonZeroValves = nonZeroValves;
        Explorers = new Valve[] { currValve, elephantValve };
    }

    internal State2 Copy()
        => new State2(Minute, FlowRate, PressureReleased, NonZeroValves, OpenedValves, Explorers[0], Explorers[1]);

    internal IEnumerable<State2> NextPossibleStates(IDictionary<string, Valve> valves, Dictionary<string, int> evaluatedStates)
    {
        var allNewStates = new List<State2>();

        var newStatesActionOne = Explorers[0].DestinationCosts.Select(x => NextState(valves[x.Item1], 0)).ToList();
        if (Explorers[0].FlowRate > 0 && !OpenedValves.Contains(Explorers[0].Name))
        {
            newStatesActionOne = newStatesActionOne.Prepend(TurnOnValve(0)).ToList();
        }

        foreach (var newstate in newStatesActionOne)
        {
            var newStatesActionBoth = Explorers[1].DestinationCosts.Select(x => newstate.NextState(valves[x.Item1], 1)).ToList();
            if (Explorers[1].FlowRate > 0 && !newstate.OpenedValves.Contains(Explorers[1].Name))
            {
                newStatesActionBoth = newStatesActionBoth.Prepend(TurnOnValve(1)).ToList();
            }
            allNewStates.AddRange(newStatesActionBoth);
        }

        var b = new List<State2>();
        foreach (var s in allNewStates)
        {
            var evalString = s.ToEvaluatedString();
            var c = evaluatedStates.ContainsKey(evalString);
            if (s.Minute <= 26 && (!evaluatedStates.ContainsKey(evalString) || evaluatedStates[evalString] < s.PressureReleased))
            {
                b.Add(s);
                evaluatedStates[evalString] = s.PressureReleased;
            }
        }
        var a = allNewStates.Where(x => !evaluatedStates.ContainsKey(x.ToEvaluatedString()) || evaluatedStates[x.ToEvaluatedString()] < x.PressureReleased);
        return b;
    }

    internal State2 NextState(Valve nextValve, int i)
    {
        if (i == 0)
        {
            return new State2(Minute, FlowRate, PressureReleased, NonZeroValves, OpenedValves, nextValve, Explorers[1]);
        }
        return new State2(Minute + 1, FlowRate, PressureReleased, NonZeroValves, OpenedValves, Explorers[0], nextValve);
    }

    internal string ToEvaluatedString()
    {
        var openedValvesString = string.Join(",", OpenedValves.ToArray().Order());
        var explorers = string.Join(",", Explorers.Select(x => x.Name).Order());
        return $"Curr: {explorers};Time: {Minute};Opened: {openedValvesString}";
    }

    internal State2 TurnOnValve(int i)
    {
        var openedValves = new HashSet<string>(OpenedValves);
        openedValves.Add(Explorers[i].Name);
        return new State2(Minute + 1, FlowRate + Explorers[i].FlowRate, PressureReleased + FlowRate, NonZeroValves, openedValves, Explorers[0], Explorers[0]);
    }
}

internal class State
{
    public int Minute;
    public int FlowRate;
    public int PressureReleased;
    public int NonZeroValves;
    public HashSet<string> OpenedValves;
    public Valve CurrValve;

    public State(int minute, int flowRate, int pressureReleased, int nonZeroValves, HashSet<string> openedValves, Valve currValve)
    {
        Minute = minute;
        FlowRate = flowRate;
        PressureReleased = pressureReleased;
        OpenedValves = new(openedValves);
        CurrValve = currValve;
        NonZeroValves = nonZeroValves;
    }

    internal State NextState(Valve nextValve, int steps)
        => new State(Minute + steps, FlowRate, PressureReleased + (FlowRate * steps), NonZeroValves, OpenedValves, nextValve);

    internal IEnumerable<State> NextPossibleStates(IDictionary<string, Valve> valves, Dictionary<string, int> evaluatedStates)
    {
        var newStates = CurrValve.DestinationCosts.Select(x => NextState(valves[x.Item1], x.Item2)).ToList();
        if (CurrValve.FlowRate > 0 && !OpenedValves.Contains(CurrValve.Name))
        {
            newStates = newStates.Prepend(TurnOnValve()).ToList();
        }
        var b = new List<State>();
        foreach (var s in newStates)
        {
            var ss = s.ToEvaluatedString();
            var c = evaluatedStates.ContainsKey(ss);
            //var d = ;
            if (s.Minute <= 30 && (!evaluatedStates.ContainsKey(ss) || evaluatedStates[ss] < s.PressureReleased))
            {
                b.Add(s);
            }
        }
        var a = newStates.Where(x => !evaluatedStates.ContainsKey(x.ToEvaluatedString()) || evaluatedStates[x.ToEvaluatedString()] < x.PressureReleased);
        return b;
    }

    internal string ToEvaluatedString()
    {
        var openedValvesString = string.Join(",", OpenedValves.ToArray().Order());
        return $"Curr: {CurrValve.Name};Time: {Minute};Opened: {openedValvesString}";
    }
    // => $"Curr: {CurrValve.Name};Time: {Minute};Press: {PressureReleased};Opened: {string.Join(",", OpenedValves.ToArray().Order())}";

    internal State TurnOnValve()
    {
        var openedValves = new HashSet<string>(OpenedValves);
        openedValves.Add(CurrValve.Name);
        return new State(Minute + 1, FlowRate + CurrValve.FlowRate, PressureReleased + FlowRate, NonZeroValves, openedValves, CurrValve);
    }
}

public class Valve
{
    public string Name;
    public int FlowRate;
    public List<(string, int)> DestinationCosts;
    public Valve(string line)
    {
        var groups = Regex.Match(line, @"Valve (.+) has flow rate=(.+)@ tunnels? leads? to valves? (.+)").Groups;
        Name = groups[1].Value;
        FlowRate = int.Parse(groups[2].Value);
        DestinationCosts = groups[3].Value.Split(", ").Select(x => (x, 1)).ToList();
    }
}