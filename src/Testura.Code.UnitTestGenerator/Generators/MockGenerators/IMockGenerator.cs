using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mono.Cecil;

namespace Testura.Code.UnitTestGenerator.Generators.MockGenerators
{
    public interface IMockGenerator
    {
        string[] RequiredNamespaces { get; }

        IEnumerable<FieldDeclarationSyntax> GenerateFields(Type typeUnderTest, IEnumerable<Models.Parameter> parameters);

        IEnumerable<StatementSyntax> GenerateSetUpStatements(Type typeUnderTest, IEnumerable<Models.Parameter> parameters);
    }
}
