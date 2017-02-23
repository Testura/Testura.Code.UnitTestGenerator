using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mono.Cecil;

namespace Testura.Code.UnitTests.UnitTestGenerators
{
    public interface IUnitTestGenerator
    {
        CompilationUnitSyntax GenerateUnitTest(TypeDefinition typeDefinition);
    }
}
