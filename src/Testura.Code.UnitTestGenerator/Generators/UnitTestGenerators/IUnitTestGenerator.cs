using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mono.Cecil;

namespace Testura.Code.UnitTestGenerator.Generators.UnitTestGenerators
{
    public interface IUnitTestGenerator
    {
        CompilationUnitSyntax GenerateUnitTest(TypeDefinition typeDefinition);
    }
}
