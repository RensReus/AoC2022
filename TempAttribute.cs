using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace AoC2022
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class TempAttribute : Attribute, ITestBuilder, IImplyFixture
    {
        public TempAttribute()
        {

        }
        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            var parameters = new TestCaseParameters(new object[] { "1000;2000;3000;;4000;;5000;6000;;7000;8000;9000;;10000" })
            {
                ExpectedResult = 10,
            };
            var test = new NUnitTestCaseBuilder().BuildTestMethod(method, suite, new());
            test.Name = "Temp";
            yield return test;
        }
    }
}