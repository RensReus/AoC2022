
using System.Data;

namespace AoC2023;

class Day19 : BaseDay
{
    [Example(expected: 19114, input: "px{a<2006:qkq,m>2090:A,rfg}\npv{a>1716:R,A}\nlnx{m>1548:A,A}\nrfg{s<537:gd,x>2440:R,A}\nqs{s>3448:A,lnx}\nqkq{x<1416:A,crn}\ncrn{x>2662:A,R}\nin{s<1351:px,qqz}\nqqz{s>2770:qs,m<1801:hdj,R}\ngd{a>3333:R,R}\nhdj{m>838:A,pv}\n\n{x=787,m=2655,a=1222,s=2876}\n{x=1679,m=44,a=2067,s=496}\n{x=2036,m=264,a=79,s=2244}\n{x=2461,m=1339,a=466,s=291}\n{x=2127,m=1623,a=2188,s=1013}")]
    [Puzzle(expected: 446935)]
    public static int Part1(string input)
    {
        var workflows = BuildWorkFlows(input.Split("\n\n")[0]);
        var parts = ReadLines(input.Split("\n\n")[1]).Select(line => new Part(line));
        var answer = 0;
        foreach (var part in parts)
        {
            var nextWorkflow = "in";
            while (nextWorkflow != "R" && nextWorkflow != "A")
            {
                nextWorkflow = part.ApplyWorkflow(workflows[nextWorkflow]);
            }
            if (nextWorkflow == "A") answer += part.TotalValue();
        }
        return answer;
    }

    private static Dictionary<string, List<Rule>> BuildWorkFlows(string v)
    {
        var workflows = new Dictionary<string, List<Rule>>();
        foreach (var line in ReadLines(v))
        {
            var (name, rules) = BuildRules(line);
            workflows[name] = rules;
        }
        return workflows;
    }

    private static (string, List<Rule>) BuildRules(string v)
    {
        var name = v.Split('{')[0];
        var rules = v.Split('{')[1].Trim('}').Split(',').Select(line => new Rule(line)).ToList();
        return (name, rules);
    }

    [Example(expected: 167409079868000, input: "px{a<2006:qkq,m>2090:A,rfg}\npv{a>1716:R,A}\nlnx{m>1548:A,A}\nrfg{s<537:gd,x>2440:R,A}\nqs{s>3448:A,lnx}\nqkq{x<1416:A,crn}\ncrn{x>2662:A,R}\nin{s<1351:px,qqz}\nqqz{s>2770:qs,m<1801:hdj,R}\ngd{a>3333:R,R}\nhdj{m>838:A,pv}\n\n{x=787,m=2655,a=1222,s=2876}\n{x=1679,m=44,a=2067,s=496}\n{x=2036,m=264,a=79,s=2244}\n{x=2461,m=1339,a=466,s=291}\n{x=2127,m=1623,a=2188,s=1013}")]
    [Puzzle(expected: 141882534122898)]
    public static long Part2(string input)
    {
        var initial = new Dictionary<char, (long, long)> { { 'x', (1, 4000) }, { 'm', (1, 4000) }, { 'a', (1, 4000) }, { 's', (1, 4000) } };
        var toEval = new List<(Dictionary<char, (long, long)>, string)> { (initial, "in") };
        var workflows = BuildWorkFlows(input.Split("\n\n")[0]);
        var answer = 0L;
        while (toEval.Count > 0)
        {
            var nextToEval = new List<(Dictionary<char, (long, long)>, string)>();
            foreach (var (attributes, workflow) in toEval)
            {
                nextToEval.AddRange(ApplyRules(attributes, workflows[workflow]));
            }
            toEval = nextToEval.Where(x => !"RA".Contains(x.Item2)).ToList();
            answer += nextToEval.Where(x => x.Item2 == "A" && AllRangesPositive(x.Item1)).Sum(x => CountOptions(x.Item1));
        }
        return answer;
    }

    private static bool AllRangesPositive(Dictionary<char, (long, long)> attributes)
        => attributes.Values.All(x => x.Item2 >= x.Item1);

    private static long CountOptions(Dictionary<char, (long, long)> attributes)
        => attributes.Select(x => (x.Value.Item2 - x.Value.Item1 + 1L)).Aggregate((a, x) => a * x);

    private static List<(Dictionary<char, (long, long)>, string)> ApplyRules(Dictionary<char, (long, long)> attributes, List<Rule> rules)
    {
        var response = new List<(Dictionary<char, (long, long)>, string)>();
        for (int i = 0; i < rules.Count - 1; i++)
        {
            var (attribute, value, op) = (rules[i].Attribute, rules[i].Value, rules[i].Operator);
            if (attributes.Values.Any(x => x.Item1 > x.Item2))
            {
                return response;
            }
            var startUpper = op == '>' ? rules[i].Value + 1 : rules[i].Value;
            var lower = attributes[attribute];
            lower.Item2 = long.Min(startUpper - 1, lower.Item2);
            var higher = attributes[attribute];
            higher.Item1 = long.Max(startUpper, higher.Item1);
            attributes[attribute] = op == '>' ? lower : higher;
            response.Add((AttributesWithUpdate(attributes, op == '>' ? higher : lower, attribute), rules[i].Destination));
        }
        response.Add((attributes, rules[^1].Destination));
        return response;
    }

    private static Dictionary<char, (long, long)> AttributesWithUpdate(Dictionary<char, (long, long)> attributes, (long, long) value, char attribute)
    {
        var response = new Dictionary<char, (long, long)>();
        foreach (var item in attributes)
        {
            response[item.Key] = item.Key == attribute ? value : attributes[item.Key];
        }
        return response;
    }

    private record Part
    {
        public Dictionary<char, int> Attributes = [];
        public Part(string line)
        {
            var groups = Regex.Match(line.Trim('}'), @"x=(.*),m=(.*),a=(.*),s=(.*)").Groups;
            Attributes['x'] = int.Parse(groups[1].Value);
            Attributes['m'] = int.Parse(groups[2].Value);
            Attributes['a'] = int.Parse(groups[3].Value);
            Attributes['s'] = int.Parse(groups[4].Value);
        }

        internal string ApplyWorkflow(List<Rule> rules)
        {
            for (int i = 0; i < rules.Count - 1; i++)
            {
                if (SatisfyRule(rules[i])) return rules[i].Destination;
            }
            return rules[^1].Destination;
        }

        internal int TotalValue()
            => Attributes.Values.Sum();

        private bool SatisfyRule(Rule rule)
            => rule.Operator switch
            {
                '>' => Attributes[rule.Attribute] > rule.Value,
                _ => Attributes[rule.Attribute] < rule.Value,
            };
    }

    private record Rule
    {
        public char Attribute;
        public char Operator;
        public int Value;
        public string Destination;

        public Rule(string line)
        {
            if (!line.Contains(':'))
            {
                Destination = line;
                return;
            }
            var groups = Regex.Match(line, @"([xmas])(.)(\d+):(.+)").Groups;
            Attribute = groups[1].Value[0];
            Operator = groups[2].Value[0];
            Value = int.Parse(groups[3].Value);
            Destination = groups[4].Value;
        }
    }
}