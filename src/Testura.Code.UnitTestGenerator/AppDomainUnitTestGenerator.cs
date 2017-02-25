using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Testura.Code.Extensions.Naming;
using Testura.Code.Saver;
using Testura.Code.UnitTestGenerator.Factories;
using Testura.Code.UnitTestGenerator.Services;
using Testura.Code.UnitTestGenerator.Util;
using Testura.Code.Util.AppDomains.Proxies;

namespace Testura.Code.UnitTestGenerator
{
    public class AppDomainUnitTestGenerator : CodeGeneratorProxy
    {
        private readonly ICodeSaver _codeSaver;
        private readonly IFileService _fileService;
        private readonly TestFrameworks _testFramework;
        private readonly MockFrameworks _mockFramework;
        private readonly string _outputDirectory;

        public AppDomainUnitTestGenerator()
        {
            _codeSaver = new CodeSaver();
            _fileService = new FileService();
        }

        public AppDomainUnitTestGenerator(TestFrameworks testFramework, MockFrameworks mockFramework, string outputDirectory)
        {

            _testFramework = testFramework;
            _mockFramework = mockFramework;
            _outputDirectory = outputDirectory;
        }

        protected override void GenerateCode(Assembly assembly, IDictionary<string, object> extraData)
        {
            var assemblyName = assembly.FullName.Split(',').First();
            var assemblyTestName = $"{assemblyName}.Tests";
            var types = assembly.ExportedTypes.Where(t => t.IsPublic && !t.IsAbstract && !t.IsInterface);
            var mockGenerator = MockGeneratorFactory.GetMockGenerator((MockFrameworks)extraData["mockFramework"]);
            var unitTestGenerator = UnitTestGeneratorFactory.GetUnitTestGenerator((TestFrameworks)extraData["testFramework"], mockGenerator);
            _fileService.CreateDirectory(Path.Combine(extraData["outputPath"].ToString(), assemblyTestName));
            foreach (var type in types)
            {
                var @namespace = type.Namespace.Replace($"{assemblyName}.", string.Empty).Split('.');
                var path = Path.Combine(extraData["outputPath"].ToString(), assemblyTestName, string.Join(@"/", @namespace));
                if (!_fileService.DirectoryExists(path))
                {
                    _fileService.CreateDirectory(path);
                }

                var @class = unitTestGenerator.GenerateUnitTest(type, assemblyName);
                _codeSaver.SaveCodeToFile(@class, Path.Combine(path, $"{type.FormattedClassName()}Tests.cs"));
            }
        }
    }
}
