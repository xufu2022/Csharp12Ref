using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c12
{
    public class Person(string Name, int Age)
    {
        public string Name { get; } = Name;
        public int Age { get; } = Age;
    }
}
