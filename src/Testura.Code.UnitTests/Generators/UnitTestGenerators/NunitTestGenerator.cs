using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Testura.Code.Builders;
using Testura.Code.Generators.Common;
using Testura.Code.Models;
using Testura.Code.UnitTests.Generators.MockGenerators;

namespace Testura.Code.UnitTests.Generators.UnitTestGenerators
{
    public class NunitTestGenerator : IUnitTestGenerator
    {
        private readonly IMockGenerator _mockGenerator;

        public NunitTestGenerator(IMockGenerator mockGenerator)
        {
            _mockGenerator = mockGenerator;
        }

        public CompilationUnitSyntax GenerateUnitTest(TypeDefinition typeUnderTest)
        {
            var parameters = GetConstructorParameters(typeUnderTest);
            var fields = _mockGenerator.CreateFields(typeUnderTest, parameters);
            var setUp = GenerateSetUp(typeUnderTest, parameters);

            return new ClassBuilder($"{typeUnderTest.Name}Tests", typeUnderTest.Namespace)
                .WithAttributes(new Attribute("TestFixture"))
                .WithModifiers(Modifiers.Public)
                .WithFields(fields.ToArray())
                .WithMethods(setUp)
                .Build();
        }

        private IEnumerable<Models.Parameter> GetConstructorParameters(TypeDefinition typeUnderTest)
        {
            var constructor = typeUnderTest.GetConstructors();
            if (constructor.Any())
            {
                return
                    constructor.First().Parameters.Select(p => new Models.Parameter(p.Name, p.ParameterType));
            }

            return new List<Models.Parameter>();
        }

        private MethodDeclarationSyntax GenerateSetUp(TypeDefinition typeUnderTest, IEnumerable<Models.Parameter> parameters)
        {
            return new MethodBuilder("SetUp")
                .WithAttributes(new Attribute("SetUp"))
                .WithModifiers(Modifiers.Public)
                .WithBody(BodyGenerator.Create(_mockGenerator.GenerateSetUpStatements(typeUnderTest, parameters).ToArray()))
                .Build();
        }
    }
}
