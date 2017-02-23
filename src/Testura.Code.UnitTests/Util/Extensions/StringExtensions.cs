using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testura.Code.UnitTests.Util.Extensions
{
    public static class StringExtensions
    {
        public static string FirstLetterToLowerCase(this string value)
        {
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}
