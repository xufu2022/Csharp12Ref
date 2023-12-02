using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c11
{
    // This type is only accessible within this file
    // File-local types in C# 11 are a new feature that allows you to declare types that are only accessible within the file where they are defined.
    // This can be useful for encapsulating types that are only relevant to the implementation details of a specific file.
    file class FileLocalClass
    {
        public string GetMessage() => "Hello from a file-local class!";

        public static void test()
        {
            var publicClass = new PublicClass();
            Console.WriteLine(publicClass.UseFileLocalClass());
        }
    }

    public class PublicClass
    {
        public string UseFileLocalClass()
        {
            var localClass = new FileLocalClass();
            return localClass.GetMessage();
        }
    }
}
