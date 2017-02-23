using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace Testura.Code.UnitTests.Util.Extensions
{
    public static class TypeReferenceExtensions
    {
        public static string FormatedFieldName(this TypeReference type)
        {
            var typeName = type.Name;

            if (type.HasGenericParameters)
            {
                typeName = FormatGenericName(type);
            }

            if (type.Resolve().IsInterface)
            {
                typeName = typeName.Remove(0, 1);
            }

            return typeName.FirstLetterToLowerCase();
        }

        public static string FormatedTypeName(this TypeReference type)
        {
            var typeName = type.Name;

            if (type.IsGenericInstance)
            {
                typeName = FormatGenericName(type);
            }

            if (type.Resolve().IsInterface)
            {
                typeName = typeName.Remove(0, 1);
            }

            if (typeName == "String")
            {
                return "string";
            }

            if (type.Resolve().IsValueType)
            {
                if (type.Name  == "Int32")
                {
                    return "int";
                }

                if (type.Name == "Double")
                {
                    return "double";
                }

                if (type.Name == "Int64")
                {
                    return "long";
                }

                if (type.Name == "UInt64")
                {
                    return "ulong";
                }

                if (type.Name == "Single")
                {
                    return "float";
                }

                if (type.Name == "Byte")
                {
                    return "byte";
                }

                if (type.Name == "String")
                {
                    return "string";
                }

                if (type.Name == "SByte")
                {
                    return "sbyte";
                }

                if (type.Name == "UInt16")
                {
                    return "ushort";
                }

                if (type.Name == "UInt32")
                {
                    return "uint";
                }

                if (type.Name == "Boolean")
                {
                    return "bool";
                }

                if (type.Name == "Char")
                {
                    return "char";
                }

                if (type.Name == "Decimal")
                {
                    return "decimal";
                }
            }

            return typeName;
        }

        private static string FormatGenericName(TypeReference type)
        {
            var generic = type as GenericInstanceType;

            if (generic == null)
            {
                return FormatedTypeName(type);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(generic.Name.Substring(0, type.Name.LastIndexOf("`", StringComparison.Ordinal)));
            sb.Append(generic.GenericArguments.Aggregate("<", (aggregate, genericType) => aggregate + (aggregate == "<" ? string.Empty : ",") + FormatGenericName(genericType.Resolve())));
            sb.Append(">");
            return sb.ToString();
        }
    }
}
