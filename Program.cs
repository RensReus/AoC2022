using Day = AoC2022.Days11.Day;
namespace AoC2022
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = new Day();

            day.Test1();
            Console.WriteLine("Part 1: " + day.Part1("p1"));
            day.Test2();
            Console.WriteLine("Part 2: " + day.Part2("p1"));
        }
    }
}
