using Testura.Code.UnitTestGenerator.Generators.MockGenerators;

namespace Testura.Code.UnitTestGenerator.Generators.UnitTestGenerators
{
    public class MsTestClassGenerator : UnitTestClassGenerator
    {
        public MsTestClassGenerator(IMockGenerator mockGenerator)
            : base(mockGenerator)
        {
        }

        /// <summary>
        /// Gets the name of the class attribute
        /// </summary>
        protected override string ClassAttribute => "TestClass";

        /// <summary>
        /// Gets he name of the set up method attribute
        /// </summary>
        protected override string SetUpAttribute => "TestInitialize";

        /// <summary>
        /// Gets the required namespaces
        /// </summary>
        protected override string[] RequiredNamespaces => new[] { "Microsoft.VisualStudio.TestTools.UnitTesting" };
    }
}
