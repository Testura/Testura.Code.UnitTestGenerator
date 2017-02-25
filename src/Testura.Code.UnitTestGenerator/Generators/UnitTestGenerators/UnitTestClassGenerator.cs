using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Testura.Code.Builders;
using Testura.Code.Extensions.Naming;
using Testura.Code.Extensions.Reflection;
using Testura.Code.Generators.Common;
using Testura.Code.UnitTestGenerator.Generators.MockGenerators;
using Attribute = Testura.Code.Models.Attribute;

namespace Testura.Code.UnitTestGenerator.Generators.UnitTestGenerators
{
    public abstract class UnitTestClassGenerator : IUnitTestClassGenerator
    {
        private readonly IMockGenerator _mockGenerator;
        private readonly List<string> _usings;

        protected UnitTestClassGenerator(IMockGenerator mockGenerator)
        {
            _mockGenerator = mockGenerator;
            _usings = new List<string>();
        }

        protected abstract string ClassAttribute { get; }

        protected abstract string SetUpAttribute { get; }

        protected abstract string[] RequiredNamespaces { get; }

        public virtual CompilationUnitSyntax GenerateUnitTest(Type typeUnderTest, string assemblyNamespace)
        {
            SetUpUsings(typeUnderTest);
            var parameters = GetConstructorParameters(typeUnderTest);
            var fields = _mockGenerator.GenerateFields(typeUnderTest, parameters);
            var setUp = GenerateSetUp(typeUnderTest, parameters);

            return new ClassBuilder($"{typeUnderTest.FormattedClassName()}Tests", $"{assemblyNamespace}.Tests{typeUnderTest.Namespace.Replace(assemblyNamespace, string.Empty)}")
                .WithAttributes(new Attribute("TestFixture"))
                .WithUsings(_usings.ToArray())
                .WithModifiers(Modifiers.Public)
                .WithFields(fields.ToArray())
                .WithMethods(setUp)
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

        private IEnumerable<Models.Parameter> GetConstructorParameters(Type typeUnderTest)
        {
            var parameters = new List<Models.Parameter>();
            var constructor = typeUnderTest.GetConstructors();
            if (constructor.Any())
            {
                foreach (var parameter in constructor.First().GetParameters())
                {
                    parameters.Add(parameter.ToParameter());
                    AddUsing(parameter.ParameterType.Namespace);
                }
            }

            if (parameters.Any(p => p.Type.IsAbstract || p.Type.IsInterface))
            {
                var mockUsings = _mockGenerator.RequiredNamespaces;
                foreach (var mockUsing in mockUsings)
                {
                    AddUsing(mockUsing);
                }
            }

            return parameters;
        }

        private MethodDeclarationSyntax GenerateSetUp(Type typeUnderTest, IEnumerable<Models.Parameter> parameters)
        {
            return new MethodBuilder("SetUp")
                .WithAttributes(new Attribute("SetUp"))
                .WithModifiers(Modifiers.Public)
                .WithBody(BodyGenerator.Create(_mockGenerator.GenerateSetUpStatements(typeUnderTest, parameters).ToArray()))
                .Build();
        }

        private void SetUpUsings(Type typeUnderTest)
        {
            _usings.Clear();
            AddUsing(typeUnderTest.Namespace);
            foreach (var requiredNamespace in RequiredNamespaces)
            {
                AddUsing(requiredNamespace);
            }
        }
    }
}