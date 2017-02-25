using Testura.Code.UnitTestGenerator.Generators.MockGenerators;

namespace Testura.Code.UnitTestGenerator.Generators.UnitTestGenerators
{
    public class MsTestClassGenerator : UnitTestClassGenerator
    {
        public MsTestClassGenerator(IMockGenerator mockGenerator)
            : base(mockGenerator)
        {
        }

        protected override string ClassAttribute => "TestClass";

        protected override string SetUpAttribute => "TestInitialize";

        protected override string[] RequiredNamespaces => new[] { "Microsoft.VisualStudio.TestTools.UnitTesting" };
    }
}
