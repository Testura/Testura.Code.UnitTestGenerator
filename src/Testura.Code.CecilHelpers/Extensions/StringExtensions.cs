namespace Testura.Code.CecilHelpers.Extensions
{
    public static class StringExtensions
    {
        public static string FirstLetterToLowerCase(this string value)
        {
            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static string FirstLetterToUpperCase(this string value)
        {
            return char.ToUpperInvariant(value[0]) + value.Substring(1);
        }
    }
}
