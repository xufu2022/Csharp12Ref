using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c12
{
    public class C12Sample
    {
        // create required memebers sample here using c# 12 features



        // List Patterns:
        public static void ListPatterns()
        {
            int[] numbers = { 1, 2, 3 };
            if (numbers is [1, 2, 3]) // matches if the array contains exactly these three elements
            {
                Console.WriteLine("Matched the sequence 1, 2, 3");
            }
        }

        // Raw String Literals:

        public static void RawStringLiterals()
        {
            /// This represents a multi-line JSON string without the need for escaping double quotes.
            string json = """
{
    "name": "John",
    "age": 30
}
""";



        }

        // UTF-8 String Literals and Interpolated String Literals:

        //public static void UTF8StringLiterals()
        //{
        //    // UTF-8 String Literals
        //    string utf8String = "\u00A9"; // ©
        //    string utf8String2 = "\U000000A9"; // ©
        //    string utf8EncodedString = u8"This is a UTF-8 encoded string";
        //}
    }
}
