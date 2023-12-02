using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c12
{
    public class OptionLamdaExample //not fully working yet
    {
        public Func<int, int, int> IncrementBy = (int source, int increment = 1) => source + increment;
    }
}
