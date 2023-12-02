using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c11
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class GenericAttribute<T> : Attribute
    {
        public T Value { get; }

        public GenericAttribute(T value)
        {
            Value = value;
        }
    }

    // Use the generic attribute
    [GenericAttribute<string>("Example")]
    public class GenericStringType
    {
        public void test()
        {
            var attribute = (GenericAttribute<string>)Attribute.GetCustomAttribute(typeof(GenericStringType), typeof(GenericAttribute<string>));
            Console.WriteLine(attribute.Value);
        }
    }

}
