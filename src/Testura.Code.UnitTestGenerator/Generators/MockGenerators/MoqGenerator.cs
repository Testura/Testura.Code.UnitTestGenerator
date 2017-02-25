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
        public string[] RequiredNamespaces => new[] { "Moq" };

        public IEnumerable<FieldDeclarationSyntax> GenerateFields(Type typeUnderTest, IEnumerable<Parameter> parameters)
        {
            if (typeUnderTest.ContainsGenericParameters)
            {
                return new List<FieldDeclarationSyntax>();
            }

            var fields = new List<Field>();
            foreach (var parameter in parameters)
            {
                var type = parameter.Type;
                if ((type.IsInterface || type.IsAbstract) && !type.IsCollection() && !type.IsICollection())
                {
                    fields.Add(new Field(
                        $"{parameter.Name}Mock",
                        CustomType.Create($"Mock<{parameter.Type.FormattedTypeName()}>"),
                        new List<Modifiers> { Modifiers.Private }));
                }
                else if ((type.IsClass && !type.IsValueType && type.Name != "String") || type.IsICollection())
                {
                    fields.Add(new Field(
                        $"{parameter.Name}",
                        parameter.Type,
                        new List<Modifiers> { Modifiers.Private }));
                }
            }

            fields.Add(new Field(
                typeUnderTest.FormattedFieldName(),
                typeUnderTest,
                new List<Modifiers> { Modifiers.Private }));

            return fields.Select(FieldGenerator.Create);
        }

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
                        new ReferenceArgument(new VariableReference($"{parameter.Name}Mock", new Code.Models.References.MemberReference("Object"))));
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
    }
}