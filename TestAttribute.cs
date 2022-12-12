using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace AoC2022;

[AttributeUsage(AttributeTargets.Method)]
public class PuzzleAttribute : TestAttribute
{
    public PuzzleAttribute(object expected, bool part2 = false) : base(expected, "Puzzle")
        => Filename = part2 ? "_2" : "";
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ExampleAttribute : TestAttribute
{
    public ExampleAttribute(object expected, string input) : base(expected, "Example")
        => Input = input;

    public ExampleAttribute(object expected, int input) : base(expected, $"Example {input}")
        => Filename = $"_Example_{input}";
}

public abstract class TestAttribute : Attribute, ITestBuilder, IImplyFixture
{
    internal string? Input;
    internal string? Filename;
    internal string? NamePrefix;
    internal object Expected;

    public TestAttribute(object expected, string name)
    {
        Expected = expected;
        NamePrefix = name;
    }

    public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test? suite)
    {
        var input = Input ?? ReadInput(method.MethodInfo.DeclaringType);
        var parameters = new TestCaseParameters(new object[] { input })
        {
            ExpectedResult = Expected,
        };
        var test = new NUnitTestCaseBuilder().BuildTestMethod(method, suite, parameters);
        test.Name = TestName(method, input);
        yield return test;
    }

    private string ReadInput(Type? declaringType)
    {
        var filename = declaringType?.ToString().Substring(8) + Filename;
        var path = Path.Combine(Directory.GetCurrentDirectory(), $"Inputs/{filename}.txt");
        return File.ReadAllText(path).TrimEnd().Replace("\r\n", ";");
    }

    protected virtual string TestName(IMethodInfo method, string input)
        => $"{NamePrefix} Expected: {Expected} for {input}";
}