using System;
using Mono.Cecil;

namespace Testura.Code.CecilHelpers.CustomTypeFormatting.TypeFormatting
{
    public static class CustomTypeValueFormatting
    {
        public static string FormatType(TypeReference typeReference)
        {
            switch (typeReference.Name)
            {
                case "Int32":
                    return "int";
                case "Double":
                    return "double";
                case "Int64":
                    return "long";
                case "UInt64":
                    return "ulong";
                case "Single":
                    return "float";
                case "Byte":
                    return "byte";
                case "String":
                    return "string";
                case "SByte":
                    return "sbyte";
                case "UInt16":
                    return "ushort";
                case "UInt32":
                    return "uint";
                case "Boolean":
                    return "bool";
                case "Char":
                    return "char";
                case "Decimal":
                    return "decimal";
                default:
                    return typeReference.Name;
            }
        }
    }
}
