namespace AoC2025;

class Day01 : BaseDay
{
    private static int ParseLine(string line)
    {
        var (dir, dist) = (line[0], int.Parse(line[1..]));
        return dir == 'L' ? -dist : dist;
    }

    [Example(expected: 3, input: "L68\nL30\nR48\nL5\nR60\nL55\nL1\nL99\nR14\nL82")]
    [Puzzle(expected: 1071)]
    public static int Part1(string input)
    {
        var pos = 50;
        var moves = ReadLines(input).Select(ParseLine);
        var answer = 0;
        foreach (var move in moves)
        {
            pos = ((pos + move) % 100 + 100) % 100;
            if (pos is 0)
            {
                answer++;
            }
        }
        return answer;
    }

    [Example(expected: 6, input: "L68\nL30\nR48\nL5\nR60\nL55\nL1\nL99\nR14\nL82")]
    [Puzzle(expected: 6700)]
    public static int Part2(string input)
    {
        var pos = 50;
        var moves = ReadLines(input).Select(ParseLine);
        var answer = 0;
        foreach (var move in moves)
        {
            answer += Math.Abs(move / 100);
            var reducedMove = move % 100;
            if (reducedMove == 0)
            {
                continue;
            }

            var newpos = pos + reducedMove;

            if (newpos >= 100 || (newpos <= 0 && pos != 0))
            {
                answer++;
            }
            pos = (newpos + 100) % 100;
        }
        return answer;
    }
}