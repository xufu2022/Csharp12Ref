using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c11
{
    public class PersonRequiredMember
    {
        public required string Name { get; set; }
        public required int Age { get; set; }
    }
}
