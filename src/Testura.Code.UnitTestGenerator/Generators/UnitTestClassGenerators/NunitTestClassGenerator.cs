using Testura.Code.UnitTestGenerator.Generators.MockGenerators;

namespace Testura.Code.UnitTestGenerator.Generators.UnitTestClassGenerators
{
    public class NunitTestClassGenerator : UnitTestClassGenerator
    {
        public NunitTestClassGenerator(IMockGenerator mockGenerator)
            : base(mockGenerator)
        {
        }

        /// <summary>
        /// Gets the name of the class attribute
        /// </summary>
        protected override string ClassAttribute => "TestFixture";

        /// <summary>
        /// Gets he name of the set up method attribute
        /// </summary>
        protected override string SetUpAttribute => "SetUp";

        /// <summary>
        /// Gets the required namespaces
        /// </summary>
        protected override string[] RequiredNamespaces => new[] { "NUnit.Framework" };
    }
}