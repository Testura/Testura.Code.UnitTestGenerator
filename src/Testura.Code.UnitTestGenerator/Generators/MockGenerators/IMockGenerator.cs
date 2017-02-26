using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Testura.Code.Models;

namespace Testura.Code.UnitTestGenerator.Generators.MockGenerators
{
    public interface IMockGenerator
    {
        /// <summary>
        /// Gets the required namespaces for this mock framework
        /// </summary>
        string[] RequiredNamespaces { get; }

        /// <summary>
        /// Generate all fields for a unit test class
        /// </summary>
        /// <param name="typeUnderTest">Type under test</param>
        /// <param name="parameters">Parameters that we should create fields from</param>
        /// <returns>All generated fields</returns>
        IEnumerable<FieldDeclarationSyntax> GenerateFields(Type typeUnderTest, IEnumerable<Parameter> parameters);

        /// <summary>
        /// Generate the assign statements inside a set up
        /// </summary>
        /// <param name="typeUnderTest">Type under tests</param>
        /// <param name="parameters">Parameters that we should create assign statements from</param>
        /// <returns>All generated assign statements</returns>
        IEnumerable<StatementSyntax> GenerateSetUpStatements(Type typeUnderTest, IEnumerable<Parameter> parameters);
    }
}
