using System;
using Testura.Code.UnitTestGenerator.Generators.MockGenerators;
using Testura.Code.UnitTestGenerator.Util;

namespace Testura.Code.UnitTestGenerator.Factories
{
    public static class MockGeneratorFactory
    {
        public static IMockGenerator GetMockGenerator(MockFrameworks mockFramework)
        {
            switch (mockFramework)
            {
                case MockFrameworks.Moq:
                    return new MoqGenerator();
                default:
                    throw new ArgumentOutOfRangeException(nameof(mockFramework), mockFramework, null);
            }
        }
    }
}
