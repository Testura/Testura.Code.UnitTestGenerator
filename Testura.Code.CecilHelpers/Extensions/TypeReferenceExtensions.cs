using System;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Testura.Code.CecilHelpers.CustomTypeFormatting;
using Testura.Code.CecilHelpers.CustomTypeFormatting.NameFormatting;
using Testura.Code.CecilHelpers.CustomTypeFormatting.TypeFormatting;

namespace Testura.Code.CecilHelpers.Extensions
{
    public static class TypeReferenceExtensions
    {
        public static string FormatedFieldName(this TypeReference typeReference)
        {
            var typeName = typeReference.Name;

            if (typeReference.HasGenericParameters)
            {
                typeName = CustomTypeGenericFormatting.FormatName(typeReference);
            }

            if (typeReference.Resolve().IsInterface)
            {
                typeName = CustomTypeInterfaceFormatting.FormatName(typeReference);
            }

            return typeName.FirstLetterToLowerCase();
        }

        public static string FormatedTypeName(this TypeReference typeReference)
        {
            var typeName = typeReference.Name;

            if (typeReference.IsGenericInstance)
            {
                return CustomTypeGenericFormatting.FormatType(typeReference);
            }

            if (typeReference.Resolve().IsValueType || typeName == "String")
            {
                return CustomTypeValueFormatting.FormatType(typeReference);
            }

            return typeName;
        }
    }
}
