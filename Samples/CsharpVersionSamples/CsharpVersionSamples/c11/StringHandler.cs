using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c11
{
    /// <summary>
    /// Raw string literals in C# 11 simplify the way developers can handle long, multiline strings
    /// and strings containing special characters like backslashes, often used in file paths or regular expressions.
    /// </summary>
    public class StringHandler
    {
        public string FormatJsonString()
        {
            // Using raw string literals for multiline strings
            return """
                   {
                       "name": "John Doe",
                       "age": 30,
                       "isEmployee": true
                    }
                   """;
        }

        public string FormatWelcomeMessage(string name, DateTime date)
        {
            // Using newlines directly in string interpolation
            return $"""
                    Hello, {name}.
                    Welcome on {date:MMMM dd, yyyy}.
                    Enjoy your stay!
                    """;
        }
    }
}
