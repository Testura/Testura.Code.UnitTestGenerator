using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Testura.Code.Extensions.Naming;
using Testura.Code.Saver;
using Testura.Code.UnitTestGenerator.Factories;
using Testura.Code.UnitTestGenerator.Generators.UnitTestClassGenerators;
using Testura.Code.UnitTestGenerator.Services;
using Testura.Code.UnitTestGenerator.Util;

namespace Testura.Code.UnitTestGenerator
{
    public class UnitTestGenerator
    {
        private readonly ICodeSaver _codeSaver;
        private readonly IFileService _fileService;

        public UnitTestGenerator()
        {
            _codeSaver = new CodeSaver();
            _fileService = new FileService();
        }

        /// <summary>
        /// Generate unit tests from an assembly in a different app domain
        /// </summary>
        /// <param name="assemblyPath">Path to the assembly</param>
        /// <param name="testFramework">Test framework to use when generating code</param>
        /// <param name="mockFramework">Mock framework to use when generating code</param>
        /// <param name="outputDirectory">The output path for the generated dll file</param>
        public void GenerateUnitTests(string assemblyPath, TestFrameworks testFramework, MockFrameworks mockFramework, string outputDirectory)
        {
            var mockGenerator = MockGeneratorFactory.GetMockGenerator(mockFramework);
            var unitTestGenerator = UnitTestGeneratorFactory.GetUnitTestGenerator(testFramework, mockGenerator);

            var assembly = Assembly.LoadFrom(assemblyPath);
            var assemblyName = assembly.GetName().Name;
            var assemblyTestName = $"{assemblyName}.Tests";
            var exportedTypes = assembly.ExportedTypes.Where(t => t.IsPublic && !t.IsAbstract && !t.IsInterface);
            _fileService.CreateDirectory(Path.Combine(outputDirectory, assemblyTestName));
            foreach (var typeUnderTest in exportedTypes)
            {
                GenerateClass(typeUnderTest, assemblyName, assemblyTestName, unitTestGenerator, outputDirectory);
            }
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

        private void GenerateClass(Type typeUnderTest, string assemblyName, string assemblyTestName, IUnitTestClassGenerator unitTestGenerator, string outputPath)
        {
            var @namespace = typeUnderTest.Namespace.Replace($"{assemblyName}.", string.Empty).Split('.');
            var path = Path.Combine(outputPath, assemblyTestName, string.Join(@"/", @namespace));
            _fileService.CreateDirectory(path);
            var @class = unitTestGenerator.GenerateUnitTestClass(typeUnderTest);
            _codeSaver.SaveCodeToFile(@class, Path.Combine(path, $"{typeUnderTest.FormattedClassName()}Tests.cs"));
        }

    }
}
