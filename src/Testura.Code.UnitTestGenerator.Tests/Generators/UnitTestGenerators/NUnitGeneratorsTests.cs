using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using NUnit.Framework;
using Testura.Code.UnitTestGenerator.Generators.MockGenerators;
using Testura.Code.UnitTestGenerator.Generators.UnitTestClassGenerators;
using Testura.Code.UnitTestGenerator.Tests.Helpers;

namespace Testura.Code.UnitTestGenerator.Tests.Generators.UnitTestGenerators
{
    [TestFixture]
    public class NUnitGeneratorsTests
    {
        private Mock<IMockGenerator> _mockGeneratorMock;
        private NunitTestClassGenerator _nunitTestClassGenerator;

        [SetUp]
        public void SetUp()
        {
            _mockGeneratorMock = new Mock<IMockGenerator>();
            _nunitTestClassGenerator = new NunitTestClassGenerator(_mockGeneratorMock.Object);
        }

        [Test]
        public void GenerateUnitTests_WhenGenerateUnitTest_ShouldContainSetUp()
        {
            _mockGeneratorMock.Setup(m => m.GenerateFields(typeof(NUnitGeneratorsTests), It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<FieldDeclarationSyntax>());
            _mockGeneratorMock.Setup(m => m.GenerateSetUpStatements(typeof(NUnitGeneratorsTests), It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<StatementSyntax>());
            var generatedCode = _nunitTestClassGenerator.GenerateUnitTestClass(typeof(NUnitGeneratorsTests));
            Assert.IsTrue(generatedCode.ToString().Contains("[SetUp]"));
        }

        [Test]
        public void GenerateUnitTests_WhenGenerateUnitTest_ShouldContainTestFixture()
        {
            _mockGeneratorMock.Setup(m => m.GenerateFields(typeof(NUnitGeneratorsTests), It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<FieldDeclarationSyntax>());
            _mockGeneratorMock.Setup(m => m.GenerateSetUpStatements(typeof(NUnitGeneratorsTests), It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<StatementSyntax>());
            var generatedCode = _nunitTestClassGenerator.GenerateUnitTestClass(typeof(NUnitGeneratorsTests));
            Assert.IsTrue(generatedCode.ToString().Contains("[TestFixture]"));
        }

        [Test]
        public void GenerateUnitTests_WhenGenerateUnitTest_ShouldContainCorrectNamespace()
        {
            _mockGeneratorMock.Setup(m => m.GenerateFields(typeof(NUnitGeneratorsTests), It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<FieldDeclarationSyntax>());
            _mockGeneratorMock.Setup(m => m.GenerateSetUpStatements(typeof(NUnitGeneratorsTests), It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<StatementSyntax>());
            var generatedCode = _nunitTestClassGenerator.GenerateUnitTestClass(typeof(NUnitGeneratorsTests));

            var o = generatedCode.NormalizeWhitespace().ToString();
            Assert.IsTrue(generatedCode.ToString().Contains("Testura.Code.UnitTestGenerator.Tests.Generators.UnitTestGenerators"));
        }

        [Test]
        public void GenerateUnitTests_WhenGenerateUnitTestWithGenericClass_ShouldHaveCorrectName()
        {
            _mockGeneratorMock.Setup(m => m.GenerateFields(typeof(MyClassWithGeneric<int>), It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<FieldDeclarationSyntax>());
            _mockGeneratorMock.Setup(m => m.GenerateSetUpStatements(typeof(MyClassWithGeneric<int>), It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<StatementSyntax>());
            var generatedCode = _nunitTestClassGenerator.GenerateUnitTestClass(typeof(MyClassWithGeneric<int>));

            var o = generatedCode.NormalizeWhitespace().ToString();
            Assert.IsTrue(generatedCode.ToString().Contains("publicclassMyClassWithGenericTests"));
        }
    }
}