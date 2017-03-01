using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading.Tasks;
using Testura.Code.UnitTestGenerator.Util;
using Testura.Code.Util.AppDomains;

namespace Testura.Code.UnitTestGenerator
{
    public class UnitTestGenerator
    {
        public UnitTestGenerator()
        {
            ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            Permissions = new PermissionSet(PermissionState.Unrestricted);
        }

        /// <summary>
        /// Gets or sets the application base for the app domain
        /// </summary>
        public string ApplicationBase { get; set; }

        /// <summary>
        /// Gets or sets the app domain evidence
        /// </summary>
        public Evidence Evidence { get; set; }

        /// <summary>
        /// Gets or sets the app domain permissions
        /// </summary>
        public PermissionSet Permissions { get; set; }

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

            var appDomainCodeGenerator = new AppDomainCodeGenerator { ApplicationBase = ApplicationBase, Evidence = Evidence, Permissions = Permissions};
            appDomainCodeGenerator.GenerateCode(assemblyPath, new UnitTestGeneratorProxy(), extraData);
        }

        /// <summary>
        /// Generate unit tests from an assembly in a different app domain async
        /// </summary>
        /// <param name="assemblyPath">Path to the assembly</param>
        /// <param name="testFramework">Test framework to use when generating code</param>
        /// <param name="mockFramework">Mock framework to use when generating code</param>
        /// <param name="outputDirectory">The output path for the generated dll file</param>
        public Task GenerateUnitTestsAsync(string assemblyPath, TestFrameworks testFramework, MockFrameworks mockFramework, string outputDirectory)
        {
            return Task.Run(() => GenerateUnitTests(assemblyPath, testFramework, mockFramework, outputDirectory));
        }

    }
}
