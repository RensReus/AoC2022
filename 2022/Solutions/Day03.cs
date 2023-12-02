namespace AoC2022;

class Day03 : BaseDay
{
    [Example(expected: 157, input: "vJrwpWtwJgWrhcsFMMfFFhFp\njqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL\nPmmdzqPrVvPwwTWBwg\nwMqvLMZHhHMvwLHjbvcjnnSBnvTQFn\nttgJtRGJQctTZtZT\nCrZsJsPPZsGzwwsLwLmpwMDw")]
    [Puzzle(expected: 8298)]
    public int Part1(string input)
        => ReadLines(input)
            .Select(line => line.Substring(0, line.Length / 2)
                .Intersect(line.Substring(line.Length / 2))
                .First())
                .Sum(x => x.PriorityValue());

    [Example(expected: 70, input: "vJrwpWtwJgWrhcsFMMfFFhFp\njqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL\nPmmdzqPrVvPwwTWBwg\nwMqvLMZHhHMvwLHjbvcjnnSBnvTQFn\nttgJtRGJQctTZtZT\nCrZsJsPPZsGzwwsLwLmpwMDw")]
    [Puzzle(expected: 2708)]
    public int Part2(string input)
    {
        var processedInput = ReadLines(input);
        var answer = 0;
        for (int i = 0; i < processedInput.Count; i += 3)
        {
            answer += processedInput[i].Intersect(processedInput[i + 1]).Intersect(processedInput[i + 2]).First().PriorityValue();
        }
        return answer;
    }
}

public static class CharExtensions
{
    public static int PriorityValue(this char letter)
        => letter switch
        {
            <= 'Z' => letter - 'A' + 27,
            _ => letter - 'a' + 1,
        };
}
