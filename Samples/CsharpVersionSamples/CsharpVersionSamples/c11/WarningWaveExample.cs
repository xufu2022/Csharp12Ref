using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c11
{
    /// <summary>
    /// In C# 11, Warning Wave 7 introduces a set of new compiler warnings to help developers identify and fix potential issues in their code.
    /// These warnings are designed to improve code quality and maintainability by alerting developers to common coding mistakes or bad practices.
    /// </summary>
    public class WarningWaveExample
    {
        public string GetGreeting(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null; // This line would trigger a new warning in C# 11's Warning Wave 7
            }

            return "Hello, " + name;
        }
    }

}
