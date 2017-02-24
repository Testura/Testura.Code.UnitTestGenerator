using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mono.Cecil;

namespace Testura.Code.UnitTestGenerator.Generators.UnitTestGenerators
{
    public interface IUnitTestClassGenerator
    {
        CompilationUnitSyntax GenerateUnitTest(TypeDefinition typeUnderTest, string assemblyNamespace);
    }
}
