using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mono.Cecil;

namespace Testura.Code.UnitTestGenerator.Generators.UnitTestGenerators
{
    public interface IUnitTestClassGenerator
    {
        CompilationUnitSyntax GenerateUnitTest(Type typeUnderTest, string assemblyNamespace);
    }
}
