using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Testura.Code.Extensions.Naming;
using Testura.Code.Saver;
using Testura.Code.UnitTestGenerator.Factories;
using Testura.Code.UnitTestGenerator.Generators.UnitTestGenerators;
using Testura.Code.UnitTestGenerator.Services;
using Testura.Code.UnitTestGenerator.Util;
using Testura.Code.Util.AppDomains.Proxies;

namespace Testura.Code.UnitTestGenerator
{
    public class UnitTestGeneratorProxy : CodeGeneratorProxy
    {
        private readonly ICodeSaver _codeSaver;
        private readonly IFileService _fileService;

        public UnitTestGeneratorProxy()
        {
            _codeSaver = new CodeSaver();
            _fileService = new FileService();
        }

        /// <summary>
        /// Generate unit tests from an assembly
        /// </summary>
        /// <param name="assembly">Path to the assembly</param>
        /// <param name="extraData">Extra data containing test framework, mock framework and output path</param>
        protected override void GenerateCode(Assembly assembly, IDictionary<string, object> extraData)
        {
            if (!extraData.ContainsKey("mockFramework"))
            {
                throw new ArgumentException("Expects extra data to contain the mock framework");
            }

            if (!extraData.ContainsKey("testFramework"))
            {
                throw new ArgumentException("Expects extra data to contain the test framework");
            }

            if (!extraData.ContainsKey("outputPath"))
            {
                throw new ArgumentException("Expects extra data to contain the output path");
            }

            GenerateCode(assembly, (MockFrameworks)extraData["mockFramework"], (TestFrameworks)extraData["testFramework"], extraData["outputPath"].ToString());
        }

        private void GenerateCode(Assembly assembly, MockFrameworks mockFramework, TestFrameworks testFramework, string outputPath)
        {
            var mockGenerator = MockGeneratorFactory.GetMockGenerator(mockFramework);
            var unitTestGenerator = UnitTestGeneratorFactory.GetUnitTestGenerator(testFramework, mockGenerator);

            var assemblyName = assembly.GetName().Name;
            var assemblyTestName = $"{assemblyName}.Tests";
            var exportedTypes = assembly.ExportedTypes.Where(t => t.IsPublic && !t.IsAbstract && !t.IsInterface);
            _fileService.CreateDirectory(Path.Combine(outputPath, assemblyTestName));
            foreach (var typeUnderTest in exportedTypes)
            {
                GenerateClass(typeUnderTest, assemblyName, assemblyTestName, unitTestGenerator, outputPath);
            }
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
