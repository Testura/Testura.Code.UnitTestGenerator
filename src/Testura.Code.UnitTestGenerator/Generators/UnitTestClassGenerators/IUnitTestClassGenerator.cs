using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Testura.Code.UnitTestGenerator.Generators.UnitTestClassGenerators
{
    public interface IUnitTestClassGenerator
    {
        /// <summary>
        /// Generate the unit test class for a type
        /// </summary>
        /// <param name="typeUnderTest">Type to generate unit test from</param>
        /// <returns>The generated class syntax</returns>
        CompilationUnitSyntax GenerateUnitTestClass(Type typeUnderTest);
    }
}
