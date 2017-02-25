using System.Collections.Generic;
using Testura.Code.Saver;
using Testura.Code.UnitTestGenerator.Services;
using Testura.Code.UnitTestGenerator.Util;
using Testura.Code.Util.AppDomains;

namespace Testura.Code.UnitTestGenerator
{
    public class UnitTestGenerator
    {
        private readonly IFileService _fileService;
        private readonly ICodeSaver _codeSaver;

        public UnitTestGenerator()
        {
            _fileService = new FileService();
            _codeSaver = new CodeSaver();
        }

        /// <summary>
        // Generate unit tests from an assembly
        /// </summary>
        /// <param name="assemblyPath">Path to the assembly</param>
        /// <param name="testFramework">Test framework that we should generate</param>
        /// <param name="mockFramework">Mock framework that we should generate></param>
        /// <param name="outputDirectory">Path to the output directory where we should save the files</param>
        public void GenerateUnitTests(string assemblyPath, TestFrameworks testFramework, MockFrameworks mockFramework, string outputDirectory)
        {
            var appDomainCodeGenerator = new AppDomainCodeGenerator();
            appDomainCodeGenerator.GenerateCode(assemblyPath, new AppDomainUnitTestGenerator(testFramework, mockFramework, outputDirectory), new Dictionary<string, object>
            {
                ["mockFramework"] = mockFramework,
                ["testFramework"] = testFramework,
                ["outputPath"] = outputDirectory
            });
        }
    }
}
