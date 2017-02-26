using System.Collections.Generic;
using Testura.Code.UnitTestGenerator.Util;
using Testura.Code.Util.AppDomains;

namespace Testura.Code.UnitTestGenerator
{
    public class UnitTestGenerator
    {
        /// <summary>
        /// Gets or sets the application base for the app domain
        /// </summary>
        public string ApplicationBase { get; set; }

        /// <summary>
        /// Generate unit tests from an assembly in a different app domain
        /// </summary>
        /// <param name="assemblyPath">Path to the assembly</param>
        /// <param name="testFramework">Test framework to use when generating code</param>
        /// <param name="mockFramework">Mock framework to use when generating code</param>
        /// <param name="outputDirectory">The output path for the generated dll file</param>
        public void GenerateUnitTests(string assemblyPath, TestFrameworks testFramework, MockFrameworks mockFramework, string outputDirectory)
        {
            var extraData = new Dictionary<string, object>
            {
                ["mockFramework"] = mockFramework,
                ["testFramework"] = testFramework,
                ["outputPath"] = outputDirectory
            };

            var appDomainCodeGenerator = new AppDomainCodeGenerator();
            appDomainCodeGenerator.GenerateCode(assemblyPath, new UnitTestGeneratorProxy(), extraData);
        }
    }
}
