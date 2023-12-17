using System.Numerics;
using NUnit.Framework;

[TestFixture]
public static class SpeedTest
{
    [Test]
    public static void ReadOnlyStructTest()
    {
        var a = new ReadOnlyStruct(1, 1);
        var b = new ReadOnlyStruct(1, 1);
        for (int i = 0; i < 1e8; i++)
        {
            a += b;
        }
    }

    [Test]
    public static void StructTest()
    {
        var a = new Struct(1, 1);
        var b = new Struct(1, 1);
        for (int i = 0; i < 1e8; i++)
        {
            a += b;
        }
    }

    [Test]
    public static void RecordTest()
    {
        var a = new Record(1, 1);
        var b = new Record(1, 1);
        for (int i = 0; i < 1e8; i++)
        {
            a += b;
        }
    }

    [Test]
    public static void ClassTest()
    {
        var a = new Class(1, 1);
        var b = new Class(1, 1);
        for (int i = 0; i < 1e8; i++)
        {
            a += b;
        }
    }

    [Test]
    public static void VectorTest()
    {
        var a = new Vector2(1, 1);
        var b = new Vector2(1, 1);
        for (int i = 0; i < 1e8; i++)
        {
            a += b;
        }
    }

    [TestCase(1)]
    [TestCase(2)]
    public static void TupleSpeed(int diff)
    {
        var a = (1, 1);
        var b = (diff, diff);
        for (int i = 0; i < 1e8; i++)
        {
            a = (a.Item1 + b.Item1, a.Item2 + b.Item2);
        }
    }

    [Test]
    public static void TwoNumbersSpeed()
    {
        var a = 1;
        var b = 1;
        for (int i = 0; i < 1e8; i++)
        {
            a = a + 1;
            b = b + 1;
        }
    }

    private readonly struct ReadOnlyStruct(int x, int y)
    {
        public int Row { get; } = x;
        public int Col { get; } = y;

        public static ReadOnlyStruct operator +(ReadOnlyStruct left, ReadOnlyStruct right)
            => new(left.Row + right.Row, left.Col + right.Col);

        public static ReadOnlyStruct operator -(ReadOnlyStruct left, ReadOnlyStruct right)
            => new(left.Row - right.Row, left.Col - right.Col);
    }

    private readonly struct Struct(int x, int y)
    {
        public int Row { get; } = x;
        public int Col { get; } = y;

        public static Struct operator +(Struct left, Struct right)
            => new(left.Row + right.Row, left.Col + right.Col);

        public static Struct operator -(Struct left, Struct right)
            => new(left.Row - right.Row, left.Col - right.Col);
    }

    private readonly struct Record(int x, int y)
    {
        public int Row { get; } = x;
        public int Col { get; } = y;

        public static Record operator +(Record left, Record right)
            => new(left.Row + right.Row, left.Col + right.Col);

        public static Record operator -(Record left, Record right)
            => new(left.Row - right.Row, left.Col - right.Col);
    }

    private readonly struct Class(int x, int y)
    {
        public int Row { get; } = x;
        public int Col { get; } = y;

        public static Class operator +(Class left, Class right)
            => new(left.Row + right.Row, left.Col + right.Col);

        public static Class operator -(Class left, Class right)
            => new(left.Row - right.Row, left.Col - right.Col);
    }
}