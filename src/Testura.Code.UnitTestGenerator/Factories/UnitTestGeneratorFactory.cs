using System;
using Testura.Code.UnitTestGenerator.Generators.MockGenerators;
using Testura.Code.UnitTestGenerator.Generators.UnitTestClassGenerators;
using Testura.Code.UnitTestGenerator.Util;

namespace Testura.Code.UnitTestGenerator.Factories
{
    public static class UnitTestGeneratorFactory
    {
        public static IUnitTestClassGenerator GetUnitTestGenerator(TestFrameworks testFramework, IMockGenerator mockGenerator)
        {
            switch (testFramework)
            {
                case TestFrameworks.MsTest:
                    return new MsTestClassGenerator(mockGenerator);
                case TestFrameworks.NUnit:
                    return new NunitTestClassGenerator(mockGenerator);
                default:
                    throw new ArgumentOutOfRangeException(nameof(testFramework), testFramework, null);
            }
        }
    }
}
