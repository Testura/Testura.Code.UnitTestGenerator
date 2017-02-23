using System;
using Testura.Code.UnitTestGenerator.Generators.MockGenerators;
using Testura.Code.UnitTestGenerator.Generators.UnitTestGenerators;
using Testura.Code.UnitTestGenerator.Util;

namespace Testura.Code.UnitTestGenerator.Factories
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
