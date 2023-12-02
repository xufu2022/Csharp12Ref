using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c11
{
    /// <summary>
    /// Numeric IntPtr in C# 11 enhances the IntPtr structure by enabling native-sized integer arithmetic directly.
    /// This means you can now perform arithmetic operations (like addition, subtraction, etc.) on IntPtr values without
    /// casting them to a specific numeric type first.
    /// </summary>
    public class IntPtrArithmetic
    {
        public IntPtr AddIntPtrs(IntPtr ptr1, IntPtr ptr2)
        {
            return ptr1 + ptr2;
        }

        public static void Main()
        {
            var calculator = new IntPtrArithmetic();
            var result = calculator.AddIntPtrs(new IntPtr(10), new IntPtr(20));
            Console.WriteLine(result); // Output: 30
        }
    }
}

