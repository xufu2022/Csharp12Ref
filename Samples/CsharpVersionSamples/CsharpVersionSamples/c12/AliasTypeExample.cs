using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c12;

using StringList = System.Collections.Generic.List<string>;

public class AliasExample
{
    public StringList GetNames()
    {
        return new StringList { "Alice", "Bob", "Charlie" };
    }
}