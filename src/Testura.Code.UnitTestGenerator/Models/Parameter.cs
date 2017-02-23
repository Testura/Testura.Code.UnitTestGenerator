using Mono.Cecil;

namespace Testura.Code.UnitTestGenerator.Models
{
    public class Parameter
    {
        public Parameter(string name, TypeReference type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }

        public TypeReference Type { get; set; }
    }
}
