using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Testura.Code.Extensions;
using Testura.Code.Extensions.Naming;
using Testura.Code.Generators.Class;
using Testura.Code.Generators.Common;
using Testura.Code.Generators.Common.Arguments.ArgumentTypes;
using Testura.Code.Models;
using Testura.Code.Models.References;
using Testura.Code.Models.Types;
using Testura.Code.Statements;

namespace Testura.Code.UnitTestGenerator.Generators.MockGenerators
{
    public class MoqGenerator : IMockGenerator
    {
        /// <summary>
        /// Gets the required namespaces for this mock framework
        /// </summary>
        public string[] RequiredNamespaces => new[] { "Moq" };

        /// <summary>
        /// Generate all fields for a unit test class
        /// </summary>
        /// <param name="typeUnderTest">Type under test</param>
        /// <param name="parameters">Parameters that we should create fields from</param>
        /// <returns>All generated fields</returns>
        public IEnumerable<FieldDeclarationSyntax> GenerateFields(Type typeUnderTest, IEnumerable<Parameter> parameters)
        {
            // Don't support classes with generic parameters yet..
            if (typeUnderTest.ContainsGenericParameters)
            {
                return new List<FieldDeclarationSyntax>();
            }

            var fields = new List<Field>();
            foreach (var parameter in parameters)
            {
                var field = CreateFieldFromType(parameter.Name, parameter.Type);
                if (field != null)
                {
                    fields.Add(field);
                }
            }

            fields.Add(new Field(
                typeUnderTest.FormattedFieldName(),
                typeUnderTest,
                new List<Modifiers> { Modifiers.Private }));

            return fields.Select(FieldGenerator.Create);
        }

        /// <summary>
        /// Generate the assign statements inside a set up
        /// </summary>
        /// <param name="typeUnderTest">Type under tests</param>
        /// <param name="parameters">Parameters that we should create assign statements from</param>
        /// <returns>All generated assign statements</returns>
        public IEnumerable<StatementSyntax> GenerateSetUpStatements(Type typeUnderTest, IEnumerable<Parameter> parameters)
        {
            var statements = new List<StatementSyntax>();
            var arguments = new List<IArgument>();

            if (typeUnderTest.ContainsGenericParameters)
            {
                return new List<StatementSyntax>();
            }

            foreach (var parameter in parameters)
            {
                var type = parameter.Type;

                if ((type.IsInterface || type.IsAbstract) && !type.IsICollection())
                {
                    statements.Add(Statement.Declaration.Assign(
                        $"{parameter.Name}Mock",
                        CustomType.Create($"Mock<{parameter.Type.FormattedTypeName()}>"),
                        ArgumentGenerator.Create()));

                    arguments.Add(
                        new ReferenceArgument(new VariableReference($"{parameter.Name}Mock", new MemberReference("Object"))));
                }
                else if (type.IsCollection())
                {
                    statements.Add(Statement.Declaration.Assign(
                        $"{parameter.Name}",
                        parameter.Type,
                        ArgumentGenerator.Create()));

                    arguments.Add(
                        new ReferenceArgument(new VariableReference($"{parameter.Name}")));
                }
                else if (type.IsICollection())
                {
                    statements.Add(Statement.Declaration.Assign(
                        $"{parameter.Name}",
                        CustomType.Create($"{parameter.Type.FormattedTypeName().Remove(0, 1)}"),
                        ArgumentGenerator.Create()));
                    arguments.Add(
                        new ReferenceArgument(new VariableReference($"{parameter.Name}")));
                }
                else if (type.IsValueType)
                {
                    arguments.Add(new ValueArgument(0));
                }
                else if (type.Name == "String")
                {
                    arguments.Add(
                        new ReferenceArgument(
                            new VariableReference("string", new MemberReference("Empty"))));
                }
                else if (type.IsArray)
                {
                    arguments.Add(new VariableArgument(parameter.Name));
                }
                else
                {
                    arguments.Add(new ReferenceArgument(new NullReference()));
                }
            }

            statements.Add(Statement.Declaration.Assign(
                typeUnderTest.FormattedFieldName(),
                CustomType.Create(typeUnderTest.FormattedTypeName()),
                ArgumentGenerator.Create(arguments.ToArray())));
            return statements;
        }

        private Field CreateFieldFromType(string name, Type type)
        {
            if ((type.IsInterface || type.IsAbstract) && !type.IsCollection() && !type.IsICollection())
            {
                return new Field(
                    $"{name}Mock",
                    CustomType.Create($"Mock<{type.FormattedTypeName()}>"),
                    new List<Modifiers> { Modifiers.Private });
            }
            else if ((type.IsClass && !type.IsValueType && type.Name != "String") || type.IsICollection())
            {
                return new Field(
                    name,
                    type,
                    new List<Modifiers> { Modifiers.Private });
            }

            return null;
        }
    }
}