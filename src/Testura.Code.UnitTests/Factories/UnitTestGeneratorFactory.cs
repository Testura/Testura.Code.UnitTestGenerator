using System;
using Testura.Code.UnitTests.MockGenerators;
using Testura.Code.UnitTests.UnitTestGenerators;
using Testura.Code.UnitTests.Util;

namespace Testura.Code.UnitTests.Factories
{
    public static class UnitTestGeneratorFactory
    {
        public static IUnitTestGenerator GetUnitTestGenerator(TestFrameworks testFramework, IMockGenerator mockGenerator)
        {
            switch (testFramework)
            {
                case TestFrameworks.MsTest:
                    return null;
                case TestFrameworks.NUnit:
                    return new NunitTestGenerator(mockGenerator);
                default:
                    throw new ArgumentOutOfRangeException(nameof(testFramework), testFramework, null);
            }
        }
    }
}
