using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace Testura.Code.UnitTests.Models
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
