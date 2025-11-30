using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace AoC.Shared;

[AttributeUsage(AttributeTargets.Method)]
public class PuzzleAttribute : TestAttribute
{
    public PuzzleAttribute(object expected, bool part2 = false) : base(expected, "Puzzle")
    {
        FilenameSuffix = part2 ? "_2" : "";
        Folder = "Puzzle Inputs";
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ExampleAttribute : TestAttribute
{
    public ExampleAttribute(object expected, string input) : base(expected, "Example")
        => Input = input;

    public ExampleAttribute(object expected, int input) : base(expected, $"Example {input}")
    {
        FilenameSuffix = $"_{input}";
        Folder = "Example Inputs";
    }
}

public abstract class TestAttribute : Attribute, ITestBuilder, IImplyFixture
{
    internal string? Input;
    internal string? FilenameSuffix;
    internal string? Folder;
    internal string NamePrefix;
    internal object Expected;

    public TestAttribute(object expected, string name)
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
        var filename = declaringType.ToString()[8..] + FilenameSuffix;
        var year = declaringType.Namespace![3..];

        var path = Path.Combine(Directory.GetCurrentDirectory(), $"{year}/{Folder}/{filename}.txt");
        try
        {
            return File.ReadAllText(path).ReplaceLineEndings("\n").TrimEnd();
        }
        catch (FileNotFoundException)
        {
            return "";
        }
    }

    protected virtual string TestName(IMethodInfo method, string input)
    {
        input = input.Length <= 50 ? input : input[..50];
        return $"{NamePrefix} Expected: {Expected} for {input}";
    }
}