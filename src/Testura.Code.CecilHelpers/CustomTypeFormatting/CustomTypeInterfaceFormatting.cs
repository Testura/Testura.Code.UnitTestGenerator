using Mono.Cecil;
using Testura.Code.CecilHelpers.Extensions;

namespace Testura.Code.CecilHelpers.CustomTypeFormatting
{
    public static class CustomTypeInterfaceFormatting
    {
        public static string FormatType(TypeReference typeReference)
        {
            var name = FormatName(typeReference);
            return name.FirstLetterToUpperCase();
        }

        public static string FormatName(TypeReference typeReference)
        {
            var typeName = typeReference.Name; 
            return typeName.Remove(0, 1);
        }
    }
}
