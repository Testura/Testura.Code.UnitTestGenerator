using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Testura.Code.CecilHelpers.Extensions;

namespace Testura.Code.UnitTestGenerator.Tests.CecilHelpers.Extensions
{
    [TestFixture]
    public class StringExtensionTests
    {
        [Test]
        public void FirstLetterToLowerCase_WhenHavingAString_ShouldChangeFirstLetterToLowerCase()
        {
            var text = "Hello";
            Assert.AreEqual("hello", text.FirstLetterToLowerCase());
        }

        [Test]
        public void FirstLetterToLowerCase_WhenHavingAEmptyString_ShouldStillBeEmpty()
        {
            var text = string.Empty;
            Assert.AreEqual(string.Empty, text.FirstLetterToLowerCase());
        }

        [Test]
        public void FirstLetterToUpperCase_WhenHavingAString_ShouldChangeFirstLetterToUpperCase()
        {
            var text = "hello";
            Assert.AreEqual("Hello", text.FirstLetterToUpperCase());
        }

        [Test]
        public void FirstLetterToUpperCase_WhenHavingAEmptyString_ShouldStillBeEmpty()
        {
            var text = string.Empty;
            Assert.AreEqual(string.Empty, text.FirstLetterToUpperCase());
        }
    }
}
