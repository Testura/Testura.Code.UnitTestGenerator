using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace Testura.Code.UnitTests.Util.Extensions
{
    public static class TypeDefinitionExtensions
    {
        public static string FormatedFieldName(this TypeDefinition type)
        {
            var typeName = type.Name;

            if (type.HasGenericParameters)
            {
                typeName = FormatGenericName(type);
            }

            if (type.IsInterface)
            {
                typeName = typeName.Remove(0, 1);
            }

            return typeName.FirstLetterToLowerCase();
        }

        public static string FormatedTypeName(this TypeDefinition type)
        {
            var typeName = type.Name;

            if (type.HasGenericParameters)
            {
                typeName = FormatGenericName(type);
            }

            if (type.IsInterface)
            {
                typeName = typeName.Remove(0, 1);
            }

            return typeName;
        }

        private static string FormatGenericName(TypeDefinition type)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(type.Name.Substring(0, type.Name.LastIndexOf("`", StringComparison.Ordinal)));
            sb.Append(type.GenericParameters.Aggregate("<", (aggregate, genericType) => aggregate + (aggregate == "<" ? string.Empty : ",") + FormatGenericName(genericType.Resolve())));
            sb.Append(">");
            return sb.ToString();
        }
    }
}
