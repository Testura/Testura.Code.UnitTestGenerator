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

        public static string FormatedClassName(this TypeReference typeReference)
        {
            return typeReference.Name.Replace("`", string.Empty);
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

        public static bool IsCollection(this TypeReference typeReference)
        {
            var name = typeReference.Name;
            return name.StartsWith("List") || 
                   name.StartsWith("Collection") || 
                   name.StartsWith("Dictionary") ||
                   name.StartsWith("Queue") || 
                   name.StartsWith("Stack") || 
                   name.StartsWith("LinkedList") ||
                   name.StartsWith("ObservableCollection") ||
                   name.StartsWith("SortedList") ||
                   name.StartsWith("HashSet");
        }

        public static bool IsICollection(this TypeReference typeReference)
        {
            var name = typeReference.Name;
            return name.StartsWith("IList") ||
                   name.StartsWith("ICollection") ||
                   name.StartsWith("IDictionary") ||
                   name.StartsWith("IQueue") ||
                   name.StartsWith("IStack") ||
                   name.StartsWith("ILinkedList") ||
                   name.StartsWith("IObservableCollection") ||
                   name.StartsWith("ISortedList") ||
                   name.StartsWith("IHashSet");
        }
    }
}
