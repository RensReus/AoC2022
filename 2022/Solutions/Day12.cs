namespace AoC2022;

class Day12 : BaseDay
{
    [Example(expected: 31, input: "Sabqponm\nabcryxxl\naccszExk\nacctuvwj\nabdefghi")]
    [Puzzle(expected: 468)]
    public int Part1(string input)
    {
        var heightMap = ReadLines(input);
        var start = GetPoint('S', heightMap);
        return RouteLength(heightMap, start, false);
    }

    private int RouteLength(IList<string> heightMap, Point start, bool goingDown)
    {
        var visited = new HashSet<Point>();
        var steps = 0;
        var height = heightMap.Count;
        var width = heightMap[0].Length;
        var nextPoints = new List<Point>();
        var target = goingDown ? 'a' : 'E';
        nextPoints.Add(start);
        visited.Add(start);
        while (true)
        {
            steps++;
            var currPoints = nextPoints.ToList();
            nextPoints = [];
            foreach (var point in currPoints)
            {
                foreach (var neighbour in point.ValidNeighbours(heightMap, height, width, goingDown))
                {
                    if (!visited.Contains(neighbour))
                    {
                        if (heightMap[neighbour.Row][neighbour.Col] == target)
                        {
                            if (point.Elevation >= 'y' || goingDown) return steps;
                        }
                        else
                        {
                            visited.Add(neighbour);
                            nextPoints.Add(neighbour);
                        }
                    }
                }
            }
        }
    }

    private Point GetPoint(char v, IList<string> heightMap)
    {
        for (int row = 0; row < heightMap.Count; row++)
        {
            for (int col = 0; col < heightMap[0].Length; col++)
            {
                if (heightMap[row][col] == v) return new(row, col, 'a');
            }
        }
        return new(-1, -1, '0');
    }

    [Example(expected: 29, input: "Sabqponm\nabcryxxl\naccszExk\nacctuvwj\nabdefghi")]
    [Puzzle(expected: 459)]
    public int Part2(string input)
    {
        var heightMap = ReadLines(input);
        var start = GetPoint('E', heightMap);
        start.Elevation = 'z';
        return RouteLength(heightMap, start, true);
    }
}

public class Point
{
    public int Row;
    public int Col;
    public char Elevation;
    public Point(int row, int col, char elevation)
    {
        Row = row;
        Col = col;
        Elevation = elevation;
    }
    public override bool Equals(object? other)
        => other is Point c && Row == c.Row && Col == c.Col;

    public override int GetHashCode()
        => HashCode.Combine(Row, Col);

    internal IEnumerable<Point> ValidNeighbours(IList<string> heightMap, int height, int width, bool goingDown)
    {
        var neighbours = new List<Point>();
        if (Row - 1 >= 0) neighbours.Add(new(Row - 1, Col, heightMap[Row - 1][Col]));
        if (Row + 1 < height) neighbours.Add(new(Row + 1, Col, heightMap[Row + 1][Col]));
        if (Col - 1 >= 0) neighbours.Add(new(Row, Col - 1, heightMap[Row][Col - 1]));
        if (Col + 1 < width) neighbours.Add(new(Row, Col + 1, heightMap[Row][Col + 1]));

        return goingDown ? neighbours.Where(point => point.Elevation - Elevation >= -1) : neighbours.Where(point => point.Elevation - Elevation <= 1);
    }

}