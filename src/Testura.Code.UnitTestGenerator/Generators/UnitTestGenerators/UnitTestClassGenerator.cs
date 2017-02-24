using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Testura.Code.Builders;
using Testura.Code.CecilHelpers.Extensions;
using Testura.Code.Generators.Common;
using Testura.Code.Models;
using Testura.Code.UnitTestGenerator.Generators.MockGenerators;

namespace Testura.Code.UnitTestGenerator.Generators.UnitTestGenerators
{
    public abstract class UnitTestClassGenerator : IUnitTestClassGenerator
    {
        protected readonly IMockGenerator _mockGenerator;
        private List<string> _usings;

        protected UnitTestClassGenerator(IMockGenerator mockGenerator)
        {
            _mockGenerator = mockGenerator;
        }

        protected abstract string ClassAttribute { get; }
        protected abstract string SetUpAttribute { get; }
        protected abstract string[] RequiredNamespaces { get; }

        public virtual CompilationUnitSyntax GenerateUnitTest(TypeDefinition typeUnderTest, string assemblyNamespace)
        {
            SetUpUsings(typeUnderTest);
            var parameters = GetConstructorParameters(typeUnderTest);
            var fields = _mockGenerator.CreateFields(typeUnderTest, parameters);
            var setUp = GenerateSetUp(typeUnderTest, parameters);

            return new ClassBuilder($"{typeUnderTest.FormatedClassName()}Tests", $"{assemblyNamespace}.Tests{typeUnderTest.Namespace.Replace(assemblyNamespace, string.Empty)}")
                .WithAttributes(new Attribute("TestFixture"))
                .WithUsings(_usings.ToArray())
                .WithModifiers(Modifiers.Public)
                .WithFields(fields.ToArray())
                .WithMethods(setUp)
                .Build();

        }

        private IEnumerable<Models.Parameter> GetConstructorParameters(TypeDefinition typeUnderTest)
        {
            var parameters = new List<Models.Parameter>();
            var constructor = typeUnderTest.GetConstructors();
            if (constructor.Any())
            {
                foreach (var parameter in constructor.First().Parameters)
                {
                    parameters.Add(new Models.Parameter(parameter.Name, parameter.ParameterType));
                    AddUsing(parameter.ParameterType.Namespace);
                }
            }

            if (parameters.Any(p => p.Type.Resolve().IsAbstract || p.Type.Resolve().IsInterface))
            {
                var mockUsings = _mockGenerator.GetRequiredNamespaces();
                foreach (var mockUsing in mockUsings)
                {
                    AddUsing(mockUsing);
                }
            }

            return parameters;
        }

        private MethodDeclarationSyntax GenerateSetUp(TypeDefinition typeUnderTest, IEnumerable<Models.Parameter> parameters)
        {
            return new MethodBuilder("SetUp")
                .WithAttributes(new Attribute("SetUp"))
                .WithModifiers(Modifiers.Public)
                .WithBody(BodyGenerator.Create(_mockGenerator.GenerateSetUpStatements(typeUnderTest, parameters).ToArray()))
                .Build();
        }

        protected void AddUsing(string @namespace)
        {
            if (string.IsNullOrEmpty(@namespace) || _usings.Contains(@namespace))
            {
                return;
            }

            _usings.Add(@namespace);
        }

        private void SetUpUsings(TypeDefinition typeUnderTest)
        {
            _usings = new List<string>();
            AddUsing(typeUnderTest.Namespace);
            foreach (var requiredNamespace in RequiredNamespaces)
            {
                AddUsing(requiredNamespace);
            }
        }
    }
}