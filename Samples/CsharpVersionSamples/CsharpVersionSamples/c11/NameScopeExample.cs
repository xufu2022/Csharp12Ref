using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c11
{
    public class NameScopeExample
    {
        private class NestedClass { }

        public string GetNameOfNestedClass() => nameof(NestedClass);

        public static void Main()
        {
            var example = new NameScopeExample();
            Console.WriteLine(example.GetNameOfNestedClass()); // Output: NestedClass
        }
    }
}
