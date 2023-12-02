using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c11
{
    /// <summary>
    /// In C# 11, you can use pattern matching with Span&lt;char&gt; to match against constant strings.
    /// This feature enhances the ability to perform efficient, type-safe comparisons without allocating new strings.
    /// </summary>
    public class SpanMatcher
    {
        static bool Is123(ReadOnlySpan<char> s)
        {
            return s is "123";
        }

        static bool IsABC(Span<char> s)
        {
            return s switch { "ABC" => true, _ => false };
        }

        static bool IsProvince(ReadOnlySpan<char> province )
        {
             //= "QC".AsSpan(); // Or Span<char>

            var DoWeSpeakFrenchHere = province switch
            {
                "QC" => true,
                "NB" => true,
                _ => false
            };
            return DoWeSpeakFrenchHere;

            var DoWeSpeakFrenchAgainHere = province is "ON"; // Return False
            return DoWeSpeakFrenchAgainHere;
        }
    }

}
