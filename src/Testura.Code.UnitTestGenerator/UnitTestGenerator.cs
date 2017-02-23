using System.IO;
using System.Linq;
using Mono.Cecil;
using Testura.Code.CecilHelpers.Extensions;
using Testura.Code.Saver;
using Testura.Code.UnitTestGenerator.Factories;
using Testura.Code.UnitTestGenerator.Services;
using Testura.Code.UnitTestGenerator.Util;

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
            var assembly = AssemblyDefinition.ReadAssembly(assemblyPath);
            var assemblyName = assembly.Name.Name;
            var types = assembly.MainModule.Types.Where(t => t.IsPublic && !t.IsAbstract && !t.IsInterface);
            var mockGenerator = MockGeneratorFactory.GetMockGenerator(mockFramework);
            var unitTestGenerator = UnitTestGeneratorFactory.GetUnitTestGenerator(testFramework, mockGenerator);
            _fileService.CreateDirectory(Path.Combine(outputDirectory, assemblyName));
            foreach (var type in types)
            {
                var @namespace = type.Namespace.Replace($"{assemblyName}.", string.Empty).Split('.');
                var path = Path.Combine(outputDirectory, assemblyName, string.Join(@"/", @namespace));
                if (!_fileService.DirectoryExists(path))
                {
                    _fileService.CreateDirectory(path);
                }

                var @class = unitTestGenerator.GenerateUnitTest(type);
                _codeSaver.SaveCodeToFile(@class, Path.Combine(path, $"{type.FormatedClassName()}Tests.cs"));
            }
        }
    }
}
