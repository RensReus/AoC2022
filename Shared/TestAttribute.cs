using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace AoC.Shared;

[AttributeUsage(AttributeTargets.Method)]
public class PuzzleAttribute : AocTestAttribute
{
    public PuzzleAttribute(object expected, bool part2 = false) : base(expected, "Puzzle")
        => Filename = part2 ? "_2" : "";
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ExampleAttribute : AocTestAttribute
{
    public ExampleAttribute(object expected, string input) : base(expected, "Example")
        => Input = input;

    public ExampleAttribute(object expected, int input) : base(expected, $"Example {input}")
        => Filename = $"_Example_{input}";
}

public abstract class AocTestAttribute : Attribute, ITestBuilder, IImplyFixture
{
    internal string? Input;
    internal string? Filename;
    internal string NamePrefix;
    internal object Expected;

    public AocTestAttribute(object expected, string name)
    {
        Expected = expected;
        NamePrefix = name;
    }

    public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test? suite)
    {
        var input = Input ?? ReadInput(method.MethodInfo.DeclaringType!);
        var parameters = new TestCaseParameters([input])
        {
            ExpectedResult = Expected,
        };
        var test = new NUnitTestCaseBuilder().BuildTestMethod(method, suite, parameters);
        test.Name = TestName(method, input);
        yield return test;
    }

    private string ReadInput(Type declaringType)
    {
        var filename = declaringType.ToString()[8..] + Filename;
        var year = declaringType.Namespace![3..];

        var path = Path.Combine(Directory.GetCurrentDirectory(), $"{year}/Inputs/{filename}.txt");
        return File.ReadAllText(path).ReplaceLineEndings("\n").TrimEnd();
    }

    protected virtual string TestName(IMethodInfo method, string input)
    {
        input = input.Length <= 50 ? input : input[..50];
        return $"{NamePrefix} Expected: {Expected} for {input}";
    }
}