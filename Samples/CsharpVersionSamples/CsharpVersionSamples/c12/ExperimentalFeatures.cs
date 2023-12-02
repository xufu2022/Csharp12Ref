using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c12
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly)]
    public class ExperimentalAttribute : Attribute { }

    public class ExperimentalClass
    {
        [Experimental]
        public void ExperimentalMethod()
        {
            Console.WriteLine("This method is experimental.");
        }
    }
}
