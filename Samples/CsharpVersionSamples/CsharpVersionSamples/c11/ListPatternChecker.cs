using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c11
{
    public class ListPatternChecker
    {
        // List patterns in C# 11 introduce a new way to match sequences within collections, enhancing the pattern matching capabilities of the language.
        public bool StartsWithOneTwoThree(List<int> numbers)
        {
            // Using list patterns to check if the list starts with 1, 2, 3
            return numbers is [1, 2, 3, ..];
        }
    }
}
