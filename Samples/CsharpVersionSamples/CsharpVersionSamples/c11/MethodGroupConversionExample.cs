using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c11
{
    /// <summary>
    /// Previous versions of the standard prohibited the compiler from reusing the delegate object created for a method group conversion.
    /// The C# 11 compiler caches the delegate object created from a method group conversion and reuses that single delegate object.
    /// This feature was first available in Visual Studio 2022 version 17.2 as a preview feature, and in .NET 7 Preview 2.
    /// </summary>
    public class MethodGroupConversionExample
    {
        public static int Add(int a, int b) => a + b;
        public static double Add(double a, double b) => a + b;

        public static int Calculate(int x, int y, Func<int, int, int> f) => f(x, y);

        //public static void Main(string[] args)
        //{
        //    int x = 20,
        //        y = 10;

        //    var result = Calculate(x, y, MethodGroupConversionExample.Add);

        //    //WriteLine(result); // 30
        //}

    }
}
