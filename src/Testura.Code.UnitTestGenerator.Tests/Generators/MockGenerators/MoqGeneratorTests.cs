using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Testura.Code.Models;
using Testura.Code.UnitTestGenerator.Generators.MockGenerators;
using Testura.Code.UnitTestGenerator.Tests.Helpers;

namespace Testura.Code.UnitTestGenerator.Tests.Generators.MockGenerators
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
            var fields = _moqGenerator.GenerateFields(
                typeof(MoqGeneratorTests),
                new List<Parameter>());
            Assert.AreEqual(1, fields.Count());
        }

        [Test]
        public void CreateFields_WhenCreatingFieldsWithMockTypes_ShouldContainTypeUnderTestAndMockTypes()
        {
            var fields = _moqGenerator.GenerateFields(
                typeof(MoqGeneratorTests),
                new List<Parameter> { new Parameter("test", typeof(MoqGeneratorTests)) });
            Assert.AreEqual(2, fields.Count());
        }

        [Test]
        public void CreateFields_WhenCreatingFields_ShouldContainCorrectInformation()
        {
            var fields = _moqGenerator.GenerateFields(
                typeof(MoqGeneratorTests),
                new List<Parameter>());
            Assert.AreEqual("privateMoqGeneratorTestsmoqGeneratorTests;", fields.First().ToString());
        }

        [Test]
        public void CreateFields_WhenCreatingFieldsWithAbstractParameter_ShoulContainFieldAsMock()
        {
            var fields = _moqGenerator.GenerateFields(
                typeof(MoqGeneratorTests),
                new List<Parameter> { new Parameter("myMockClass", typeof(MyAbstractClass)) });
            Assert.AreEqual("privateMock<MyAbstractClass>myMockClassMock;", fields.First().ToString());
        }

        [Test]
        public void CreateFields_WhenCreatingFieldsWithArrayParameter_ShoulContainFieldAsArray()
        {
            var fields = _moqGenerator.GenerateFields(
                typeof(MoqGeneratorTests),
                new List<Parameter> { new Parameter("myArray", typeof(int[])) });
            Assert.AreEqual("privateint[]myArray;", fields.First().ToString());
        }

        [Test]
        public void CreateFields_WhenCreatingFieldsWithIListParameter_ShoulContainFieldAsIList()
        {
            var fields = _moqGenerator.GenerateFields(
                typeof(MoqGeneratorTests),
                new List<Parameter> { new Parameter("myList", typeof(IList<string>)) });
            Assert.AreEqual("privateIList<string>myList;", fields.First().ToString());
        }
    }
}
