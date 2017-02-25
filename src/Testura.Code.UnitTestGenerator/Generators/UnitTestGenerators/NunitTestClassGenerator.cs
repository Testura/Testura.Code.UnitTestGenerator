using Testura.Code.UnitTestGenerator.Generators.MockGenerators;

namespace Testura.Code.UnitTestGenerator.Generators.UnitTestGenerators
{
    public class NunitTestClassGenerator : UnitTestClassGenerator
    {
        public NunitTestClassGenerator(IMockGenerator mockGenerator)
            : base(mockGenerator)
        {
        }

        protected override string ClassAttribute => "TestFixture";

        protected override string SetUpAttribute => "SetUp";

        protected override string[] RequiredNamespaces => new[] { "NUnit.Framework" };
    }
}