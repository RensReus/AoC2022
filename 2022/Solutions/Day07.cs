namespace AoC2022;

class Day07 : BaseDay
{
    static Dir ProcessInput(string input)
    {
        var lines = ReadLines(input).Where(x => !x.Contains("$ ls"));
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var topDir = new Dir("/", null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        var currDir = topDir;
        foreach (var line in lines.Skip(1))
        {
            if (line.Contains("$ cd"))
            {
                currDir = line.Equals("$ cd ..") ? currDir.Parent : currDir.SubDirectories.First(x => x.Name == line.Split()[2]);
            }
            else if (line.StartsWith("dir "))
            {
                currDir.SubDirectories.Add(new Dir(line.Split()[1], currDir));
            }
            else
            {
                currDir.Files.Add(Int32.Parse(line.Split()[0]));
            }
        }
        return topDir;
    }

    [Example(expected: 95437, input: 1)]
    [Puzzle(expected: 1449447)]
    public int Part1(string input)
        => ProcessInput(input).SumOfSizesAllDirectoriesSmallerThan(100000);

    [Example(expected: 24933642, input: 1)]
    [Puzzle(expected: 8679207)]
    public int Part2(string input)
    {
        var processedInput = ProcessInput(input);
        var freeSpace = 70000000 - processedInput.Size;
        var toFreeUp = 30000000 - freeSpace;
        return processedInput.SmallestSubdirectoryLargerThan(toFreeUp);
    }
}

public class Dir
{
    public List<Dir> SubDirectories = new();
    public List<int> Files = new();
    public int Size => SubDirectories.Sum(x => x.Size) + Files.Sum();
    public Dir Parent;
    public string Name;
    public Dir(string name, Dir parent)
    {
        Name = name;
        Parent = parent;
    }

    internal int SumOfSizesAllDirectoriesSmallerThan(int maxsize)
        => SubDirectories.Sum(x => x.SumOfSizesAllDirectoriesSmallerThan(maxsize))
            + (Size <= maxsize ? Size : 0);

    internal int SmallestSubdirectoryLargerThan(int toFreeUp)
    {
        var smallestValidChild = SubDirectories.Select(x => ((Dir)x).SmallestSubdirectoryLargerThan(toFreeUp)).DefaultIfEmpty(int.MaxValue).Min();
        var thisSize = Size <= toFreeUp ? int.MaxValue : Size;
        return Int32.Min(smallestValidChild, thisSize);
    }
}
