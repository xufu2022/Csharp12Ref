using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c11
{
    //In this example, the MathOperations<T> class can perform addition on any type that implements INumber<T>, which includes most built-in numeric types like int, double, etc.

    public class MathOperations<T> where T : INumber<T>
    {
        public T Add(T a, T b) => a + b;
    }
}
