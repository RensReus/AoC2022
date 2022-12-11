using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace AoC2022;

[AttributeUsage(AttributeTargets.Method)]
public class PuzzleAttribute : TestAttribute, ITestBuilder, IImplyFixture
{
    public PuzzleAttribute(object expected, bool part2 = false) : base(expected)
        => Filename = part2 ? "_2" : "";
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ExampleAttribute : TestAttribute, ITestBuilder, IImplyFixture
{
    public ExampleAttribute(object expected, string input) : base(expected)
        => Input = input;

    public ExampleAttribute(object expected, int input) : base(expected)
        => Filename = $"_Example_{input}";
}

public abstract class TestAttribute : Attribute, ITestBuilder, IImplyFixture
{
    internal string? Input;
    internal string? Filename;
    internal object Expected;

    public TestAttribute(object expected)
        => Expected = expected;

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
        => $"{method.Name.Replace("_", " ")} Expected: {Expected} for {input}";
}