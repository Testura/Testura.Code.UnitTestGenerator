using Mono.Cecil;

namespace Testura.Code.CecilHelpers.CustomTypeFormatting.NameFormatting
{
    public static class CustomTypeInterfaceFormatting
    {
        public static string FormatName(TypeReference typeReference)
        {
            var typeName = typeReference.Name; 
            return typeName.Remove(0, 1);
        }
    }
}
