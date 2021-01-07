using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Testura.Code.Builders;
using Testura.Code.Extensions.Naming;
using Testura.Code.Extensions.Reflection;
using Testura.Code.Generators.Common;
using Testura.Code.Models;
using Testura.Code.UnitTestGenerator.Generators.MockGenerators;
using Attribute = Testura.Code.Models.Attribute;

namespace Testura.Code.UnitTestGenerator.Generators.UnitTestClassGenerators
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

        /// <summary>
        /// Gets the name of the class attribute
        /// </summary>
        protected abstract string ClassAttribute { get; }

        /// <summary>
        /// Gets he name of the set up method attribute
        /// </summary>
        protected abstract string SetUpAttribute { get; }

        /// <summary>
        /// Gets the required namespaces
        /// </summary>
        protected abstract string[] RequiredNamespaces { get; }

        /// <summary>
        /// Generate the unit test class for a type
        /// </summary>
        /// <param name="typeUnderTest">Type to generate unit test from</param>
        /// <returns>The generated class syntax</returns>
        public virtual CompilationUnitSyntax GenerateUnitTestClass(Type typeUnderTest)
        {
            var assemblyNamespaceName = typeUnderTest.Assembly.GetName().Name;
            var generatedUnitTestNamespace = $"{assemblyNamespaceName}.Tests{typeUnderTest.Namespace.Replace(assemblyNamespaceName, string.Empty)}";

            SetUpUsings(typeUnderTest);

            var parameters = GetConstructorParameters(typeUnderTest);
            var fields = _mockGenerator.GenerateFields(typeUnderTest, parameters);
            var setUp = GenerateSetUp(typeUnderTest, parameters);

            return new ClassBuilder($"{typeUnderTest.FormattedClassName()}Tests", generatedUnitTestNamespace)
                .WithAttributes(new Attribute(ClassAttribute))
                .WithUsings(_usings.ToArray())
                .WithModifiers(Modifiers.Public)
                .WithFields(fields.ToArray())
                .WithMethods(setUp)
                .Build();
        }

        private IEnumerable<Parameter> GetConstructorParameters(Type typeUnderTest)
        {
            var parameters = new List<Parameter>();
            var constructor = typeUnderTest.GetConstructors();

            if (constructor.Any())
            {
                foreach (var parameter in constructor.First().GetParameters())
                {
                    parameters.Add(parameter.ToParameter());
                    AddUsing(parameter.ParameterType.Namespace);
                    if (parameter.ParameterType.IsGenericType)
                    {
                        foreach (var generic in parameter.ParameterType.GetGenericArguments())
                        {
                            AddUsing(generic.Namespace);
                        }
                    }
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
            return (MethodDeclarationSyntax)new MethodBuilder("SetUp")
                .WithAttributes(new Attribute(SetUpAttribute))
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

        private void AddUsing(string @namespace)
        {
            if (string.IsNullOrEmpty(@namespace) || _usings.Contains(@namespace))
            {
                return;
            }

            _usings.Add(@namespace);
        }
    }
}