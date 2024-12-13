namespace AoC2024;

class Day13 : BaseDay
{
    [Example(expected: 480, input: "Button A: X+94, Y+34\nButton B: X+22, Y+67\nPrize: X=8400, Y=5400\n\nButton A: X+26, Y+66\nButton B: X+67, Y+21\nPrize: X=12748, Y=12176\n\nButton A: X+17, Y+86\nButton B: X+84, Y+37\nPrize: X=7870, Y=6450\n\nButton A: X+69, Y+23\nButton B: X+27, Y+71\nPrize: X=18641, Y=10279")]
    [Puzzle(expected: 29517)]
    public static int Part1(string input)
    {
        var configs = ReadLinesDouble(input);
        var ans = 0;
        foreach (var config in configs)
        {
            var a = config[0].Split("X+")[1].Split(", Y+").Select(int.Parse).ToArray();
            var b = config[1].Split("X+")[1].Split(", Y+").Select(int.Parse).ToArray();
            var prize = config[2].Split("X=")[1].Split(", Y=").Select(int.Parse).ToArray();
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    var x = a[0] * i + b[0] * j;
                    var y = a[1] * i + b[1] * j;
                    if (x == prize[0] && y == prize[1])
                    {
                        ans += i * 3 + j;
                    }
                }
            }
        }
        return ans;
    }

    // [Puzzle(expected: 103570327981381)]
    // public static long Part2Original(string input)
    // {
    //     var configs = ReadLinesDouble(input);
    //     long ans = 0;
    //     var extra = 10000000000000;
    //     foreach (var config in configs)
    //     {
    //         var a = config[0].Split("X+")[1].Split(", Y+").Select(long.Parse).ToArray();
    //         var b = config[1].Split("X+")[1].Split(", Y+").Select(long.Parse).ToArray();
    //         var bsteps = Math.Abs(a[0] - a[1]);
    //         var asteps = Math.Abs(b[0] - b[1]);
    //         var travelled = bsteps * b[0] + asteps * a[0];
    //         var bigStepsCount = (extra - (100 * travelled)) / travelled;

    //         var prize = config[2].Split("X=")[1].Split(", Y=").Select(x => long.Parse(x) + extra - (bigStepsCount * travelled)).ToArray();
    //         for (int i = 0; i < 10000; i++)
    //         {
    //             var dx = a[0] * i;
    //             var dy = a[1] * i;
    //             if (dx > prize[0] || dy > prize[1]) break;
    //             for (int j = 0; j < 10000; j++)
    //             {
    //                 var x = dx + b[0] * j;
    //                 var y = dy + b[1] * j;
    //                 if (x > prize[0] || y > prize[1]) break;
    //                 if (x == prize[0] && y == prize[1])
    //                 {
    //                     ans += bigStepsCount * (bsteps + (asteps * 3));
    //                     ans += i * 3 + j;
    //                 }
    //             }
    //         }
    //     }
    //     return ans;
    // }

    [Puzzle(expected: 103570327981381)]
    public static long Part2(string input)
    {
        var configs = ReadLinesDouble(input);
        long ans = 0;
        var extra = 10000000000000;
        foreach (var config in configs)
        {
            var first = config[0].Split("X+")[1].Split(", Y+").Select(long.Parse).ToArray();
            var second = config[1].Split("X+")[1].Split(", Y+").Select(long.Parse).ToArray();
            var prize = config[2].Split("X=")[1].Split(", Y=").Select(x => decimal.Parse(x) + extra).ToArray();
            var x1 = first[0];
            var y1 = first[1];
            var x2 = second[0];
            var y2 = second[1];
            var p0 = prize[0];
            var p1 = prize[1];
            var a = (p0 * y2 - p1 * x2) / (x1 * y2 - y1 * x2);
            var b = (p1 * x1 - p0 * y1) / (x1 * y2 - y1 * x2);
            if (a == Math.Floor(a) && b == Math.Floor(b))
            {
                ans += (long)(3 * a + b);
            }
        }
        return ans;
    }
}