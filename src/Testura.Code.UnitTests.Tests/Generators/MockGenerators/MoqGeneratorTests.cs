using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using NUnit.Framework;
using Testura.Code.Models.Types;
using Testura.Code.UnitTests.Generators.MockGenerators;
using Testura.Code.UnitTests.Models;

namespace Testura.Code.UnitTests.Tests.MockGenerators
{
    [TestFixture]
    public class MoqGeneratorTests
    {
        private MoqGenerator _moqGenerator;

        [SetUp]
        public void SetUp()
        {
            _moqGenerator = new MoqGenerator();
        }

        [Test]
        public void CreateFields_WhenCreatingFieldsWithoutAnyMockTypes_ShouldOnlyContainTypeUnderTest()
        {
            var fields = _moqGenerator.CreateFields(
                new TypeDefinition("somenamespace", "MyClass", TypeAttributes.BeforeFieldInit),
                new List<Parameter>());
            Assert.AreEqual(1, fields.Count);
        }

        [Test]
        public void CreateFields_WhenCreatingFieldsWithMockTypes_ShouldContainTypeUnderTestAndMockTypes()
        {
            var fields = _moqGenerator.CreateFields(
                new TypeDefinition("somenamespace", "MyClass", TypeAttributes.BeforeFieldInit),
                new List<Parameter> { new Parameter("test", new TypeDefinition("test", "MyMockClass", TypeAttributes.Class)) });
            Assert.AreEqual(2, fields.Count);
        }

        [Test]
        public void CreateFields_WhenCreatingFields_ShouldContainCorrectInformation()
        {
            var fields = _moqGenerator.CreateFields(
                new TypeDefinition("somenamespace", "MyClass", TypeAttributes.BeforeFieldInit),
                new List<Parameter>());
            Assert.AreEqual("MyClass", ((CustomType)fields.First().Type).TypeName);
            Assert.AreEqual("myClass", fields.First().Name);
        }

        [Test]
        public void CreateFields_WhenCreatingFieldsWithAbstractParameter_ShoulContainFieldAsMock()
        {
            var fields = _moqGenerator.CreateFields(
                new TypeDefinition("somenamespace", "MyClass", TypeAttributes.BeforeFieldInit),
                new List<Parameter> { new Parameter("myMockClass", new TypeDefinition("test", "MyMockClass", TypeAttributes.Abstract)) });
            Assert.AreEqual("Mock<MyMockClass>", ((CustomType)fields.First().Type).TypeName);
            Assert.AreEqual("myMockClassMock", fields.First().Name);
        }

        [Test]
        public void CreateFields_WhenCreatingFieldsWithNonAbstractParameter_ShoulContainFieldButNotMock()
        {
            var fields = _moqGenerator.CreateFields(
                new TypeDefinition("somenamespace", "MyClass", TypeAttributes.BeforeFieldInit),
                new List<Parameter> { new Parameter("myMockClass", new TypeDefinition("test", "MyMockClass", TypeAttributes.Class)) });
            Assert.AreEqual("MyMockClass", ((CustomType)fields.First().Type).TypeName);
            Assert.AreEqual("myMockClass", fields.First().Name);
        }

        [Test]
        public void CreateFields_WhenCreatingFieldsWithAGenericParameter_ShoulContainGenericParameter()
        {
            var fields = _moqGenerator.CreateFields(
                new TypeDefinition("somenamespace", "MyClass", TypeAttributes.BeforeFieldInit),
                new List<Parameter> { new Parameter("myMockClass", new TypeDefinition("test", "MyMockClass", TypeAttributes.Class)) });
            Assert.AreEqual("MyMockClass", ((CustomType)fields.First().Type).TypeName);
            Assert.AreEqual("myMockClass", fields.First().Name);
        }

        [Test]
        public void GenerateSetUpStatements_WhenCreatingSetUpWithoutAnyMockTypes_ShouldOnlyGenerateASingleStatement()
        {
            var statements = _moqGenerator.GenerateSetUpStatements(new TypeDefinition("myNamespace", "MyClass", TypeAttributes.Public), new List<Parameter>());
            Assert.AreEqual(1, statements.Count);
        }

        [Test]
        public void GenerateSetUpStatements_WhenCreatingSetUpWithMockTypes_ShouldGenerateAllStatements()
        {
            var statements = _moqGenerator.GenerateSetUpStatements(new TypeDefinition("myNamespace", "MyClass", TypeAttributes.Public), new List<Parameter>() { new Parameter("myOtherClass", new TypeDefinition("myNamespace", "MyOtherClass", TypeAttributes.Abstract))});
            Assert.AreEqual(2, statements.Count);
        }

        [Test]
        public void GenerateSetUpStatements_WhenCreatingSetUpWithoutAnyMockTypes_ShouldGenerateCorrectStatement()
        {
            var statements = _moqGenerator.GenerateSetUpStatements(new TypeDefinition("myNamespace", "MyClass", TypeAttributes.Public), new List<Parameter>());
            Assert.AreEqual("myClass=newMyClass();", statements.First().ToString());
        }

        [Test]
        public void GenerateSetUpStatements_WhenCreatingSetUpWithMockTypes_ShouldGenerateCorrectStatements()
        {
            var statements = _moqGenerator.GenerateSetUpStatements(new TypeDefinition("myNamespace", "MyClass", TypeAttributes.Public), new List<Parameter>() { new Parameter("myOtherClass", new TypeDefinition("myNamespace", "MyOtherClass", TypeAttributes.Abstract)) });
            Assert.AreEqual("myOtherClassMock=newMock<MyOtherClass>();", statements.First().ToString());
            Assert.AreEqual("myClass=newMyClass(myOtherClassMock.Object);", statements.Last().ToString());
        }
    }
}
