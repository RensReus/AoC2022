using AoC2022.Days;

namespace AoC2022.Days07;

class Day : BaseDay
{
    static Dir ProcessInput(string filename)
    {
        var lines = ReadFile("07/" + filename);
        Dir currDir = new("default", null);
        var topDir = new Dir("/", null);
        currDir.Children.Add(topDir);
        var directories = new List<Dir> { topDir };
        foreach (var line in lines)
        {
            if (line == "$ cd ..")
            {
                currDir = currDir.Parent;
                continue;
            }
            if (line.StartsWith("$ cd"))
            {
                currDir = (Dir)currDir.Children.First(x => x.Name == line.Split()[2]);
                continue;
            }
            if (line == "$ ls")
            {
                continue;
            }
            if (line.StartsWith("dir "))
            {
                var newdir = new Dir(line.Split()[1], currDir);
                directories.Add(newdir);
                currDir.Children.Add(newdir);
                continue;
            }
            currDir.Children.Add(new File(line, currDir));
        }
        return topDir;
    }

    public override int Part1(string filename)
    {
        var input = ProcessInput(filename);
        return input.SumOfSizesAllDirectoriesSmallerThan(100000);
    }

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

public class Entity
{
    public virtual int Size => 0;
    public string Name;
    public Dir Parent;
}

public class Dir : Entity
{
    public List<Entity> Children = new();
    public override int Size => Children.Sum(x => x.Size); // TODO store calculated size
    public Dir(string name, Dir parent)
    {
        Name = name;
        Parent = parent;
    }

    internal int SumOfSizesAllDirectoriesSmallerThan(int maxsize)
        => Children.Where(x => x.GetType() == typeof(Dir)).Sum(x => ((Dir)x).SumOfSizesAllDirectoriesSmallerThan(maxsize))
            + (Size <= maxsize ? Size : 0);

    internal int SmallestSubdirectoryLargerThan(int toFreeUp)
    {
        var childrenFreeUpSizes = Children.Where(x => x.GetType() == typeof(Dir)).Select(x => ((Dir)x).SmallestSubdirectoryLargerThan(toFreeUp));
        var smallestValidChild = childrenFreeUpSizes.Count() > 0 ? childrenFreeUpSizes.Min() : int.MaxValue;
        var thisSize = Size <= toFreeUp ? int.MaxValue : Size;
        return Int32.Min(smallestValidChild, thisSize);
    }
}

public class File : Entity
{
    public int FileSize;
    public override int Size => FileSize;
    public File(string input, Dir parent)
    {
        FileSize = Int32.Parse(input.Split()[0]);
        Name = input.Split()[1];
        Parent = parent;
    }
}