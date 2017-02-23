using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mono.Cecil;
using Moq;
using NUnit.Framework;
using Testura.Code.Models;
using Testura.Code.UnitTestGenerator.Generators.MockGenerators;
using Testura.Code.UnitTestGenerator.Generators.UnitTestGenerators;

namespace Testura.Code.UnitTestGenerator.Tests.Generators.UnitTestGenerators
{
    [TestFixture]
    public class NUnitGeneratorsTests
    {
        private Mock<IMockGenerator> _mockGeneratorMock;
        private NunitTestGenerator _nunitTestGenerator;

        [SetUp]
        public void SetUp()
        {
            _mockGeneratorMock = new Mock<IMockGenerator>();
            _nunitTestGenerator = new NunitTestGenerator(_mockGeneratorMock.Object);
        }

        [Test]
        public void GemerateUnitTests_WhenGenerateUnitTest_ShouldContainSetUp()
        {
            var type = new TypeDefinition("myNamespace", "MyClass", TypeAttributes.Class);
            _mockGeneratorMock.Setup(m => m.CreateFields(type, It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<Field>());
            _mockGeneratorMock.Setup(m => m.GenerateSetUpStatements(type, It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<StatementSyntax>());
            var generatedCode = _nunitTestGenerator.GenerateUnitTest(type);
            Assert.IsTrue(generatedCode.ToString().Contains("[SetUp]"));
        }

        [Test]
        public void GemerateUnitTests_WhenGenerateUnitTest_ShouldContainTestFixture()
        {
            var type = new TypeDefinition("myNamespace", "MyClass", TypeAttributes.Class);
            _mockGeneratorMock.Setup(m => m.CreateFields(type, It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<Field>());
            _mockGeneratorMock.Setup(m => m.GenerateSetUpStatements(type, It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<StatementSyntax>());
            var generatedCode = _nunitTestGenerator.GenerateUnitTest(type);
            Assert.IsTrue(generatedCode.ToString().Contains("[TestFixture]"));
        }

        [Test]
        public void GemerateUnitTests_WhenGenerateUnitTest_ShouldContainCorrectNamespace()
        {
            var type = new TypeDefinition("myNamespace.my.space", "MyClass", TypeAttributes.Class);
            _mockGeneratorMock.Setup(m => m.CreateFields(type, It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<Field>());
            _mockGeneratorMock.Setup(m => m.GenerateSetUpStatements(type, It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<StatementSyntax>());
            var generatedCode = _nunitTestGenerator.GenerateUnitTest(type);

            var o = generatedCode.NormalizeWhitespace().ToString();
            Assert.IsTrue(generatedCode.ToString().Contains("namespacemyNamespace.my.space"));
        }

        [Test]
        public void GemerateUnitTests_WhenGenerateUnitTest_ShouldContainFields()
        {
            var type = new TypeDefinition("myNamespace", "MyClass", TypeAttributes.Class);
            _mockGeneratorMock.Setup(m => m.CreateFields(type, It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<Field> { new Field("myField", typeof(int)) });
            _mockGeneratorMock.Setup(m => m.GenerateSetUpStatements(type, It.IsAny<IEnumerable<Models.Parameter>>()))
                .Returns(new List<StatementSyntax>());
            var generatedCode = _nunitTestGenerator.GenerateUnitTest(type);
            Assert.IsTrue(generatedCode.ToString().Contains("intmyField"));
        }
    }
}