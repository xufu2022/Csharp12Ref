using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpVersionSamples.c11
{
    public struct PointAutodefaultStructs
    {
        public int X;
        public int Y;
        // No parameterless constructor is needed

        //public  void Main()
        //{
        //    PointAutodefaultStructs p; // Automatically defaults to X=0, Y=0
        //    Console.WriteLine($"Point: X={p.X}, Y={p.Y}");
        //}
    }
}
