using AoC2022.Days;

namespace AoC2022.Days07;

class Day : BaseDay
{
    static Dir ProcessInput(string filename)
    {
        var lines = ReadFile("07/" + filename).Where(x => !x.Contains("$ ls"));
        var topDir = new Dir("/", null);
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

    public override int Part1(string filename)
        => ProcessInput(filename).SumOfSizesAllDirectoriesSmallerThan(100000);

    public override int Part2(string filename)
    {
        var input = ProcessInput(filename);
        var freeSpace = 70000000 - input.Size;
        var toFreeUp = 30000000 - freeSpace;
        return input.SmallestSubdirectoryLargerThan(toFreeUp);
    }

    public override List<Case> Part1Cases() => new() { new("1a", 95437), new("p1", 1449447) };

    public override List<Case> Part2Cases() => new() { new("1a", 24933642), new("p1", 8679207) };
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
