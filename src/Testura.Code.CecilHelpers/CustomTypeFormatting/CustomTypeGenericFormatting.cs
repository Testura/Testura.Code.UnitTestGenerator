using System;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Testura.Code.CecilHelpers.Extensions;

namespace Testura.Code.CecilHelpers.CustomTypeFormatting
{
    public static class CustomTypeGenericFormatting
    {
        public static string FormatType(TypeReference typeReference)
        {
            var generic = typeReference as GenericInstanceType;

            if (generic == null)
            {
                return typeReference.FormatedTypeName();
            }

            var sb = new StringBuilder();
            sb.Append(generic.Name.Substring(0, typeReference.Name.LastIndexOf("`", StringComparison.Ordinal)));
            sb.Append(generic.GenericArguments.Aggregate("<", (aggregate, genericType) => aggregate + (aggregate == "<" ? string.Empty : ",") + FormatType(genericType.Resolve())));
            sb.Append(">");
            return sb.ToString();
        }

        public static string FormatName(TypeReference typeReference)
        {
            var index = typeReference.Name.IndexOf("`");
            if(index == -1)
            {
                return typeReference.Name;
            }
            return typeReference.Name.Substring(0, index + 1);
        }
    }
}
