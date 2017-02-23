using NUnit.Framework;
using Testura.Code.UnitTests.Util;

namespace Testura.Code.UnitTests.Tests.Generators
{
    [TestFixture]
    public class Do
    {
        [Test]
        public void Hej()
        {
            var o = new UnitTestGenerator();
            o.GenerateUnitTests(@"C:\Users\Mille\Documents\Visual Studio 2017\Projects\TestFramework\TestFramework\bin\Debug\TestFramework.exe", TestFrameworks.NUnit, MockFrameworks.Moq, @"C:\Users\Mille\Documents\KodGenerated\Mo");
        }
    }
}