using System;
using Testura.Code.UnitTests.MockGenerators;
using Testura.Code.UnitTests.Util;

namespace Testura.Code.UnitTests.Factories
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
