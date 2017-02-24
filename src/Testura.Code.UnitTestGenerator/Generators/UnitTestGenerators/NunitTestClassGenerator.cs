using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Testura.Code.Builders;
using Testura.Code.CecilHelpers.Extensions;
using Testura.Code.Generators.Common;
using Testura.Code.Models;
using Testura.Code.UnitTestGenerator.Generators.MockGenerators;

namespace Testura.Code.UnitTestGenerator.Generators.UnitTestGenerators
{
    public class NunitTestClassGenerator : UnitTestClassGenerator
    {
        public NunitTestClassGenerator(IMockGenerator mockGenerator) : base(mockGenerator)
        {
        }

        protected override string ClassAttribute => "TestFixture";
        protected override string SetUpAttribute => "SetUp";
        protected override string[] RequiredNamespaces => new[] {"NUnit.Framework"};
    }
}