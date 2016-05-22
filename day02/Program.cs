using System;
using System.Linq;
using System.IO;

namespace day02
{
    class MainClass
    {
        public static void Main(string[] args)
        {
			string[] dims = File.ReadAllLines("../../input");
            int area = 0;
            int ribbon = 0;
            foreach (string s in dims) {
                int[] packDims = Array.ConvertAll(s.Split('x'), int.Parse);
                int[] areas = { packDims[0] * packDims[1], packDims[0] * packDims[2], packDims[2] * packDims[1] };
                area += areas.Min() + 2 * areas.Sum();


                Array.Sort(packDims);
                ribbon += packDims.Aggregate(1, (a, b) => a * b) + 2*(packDims[0] + packDims[1]);
            }
						Console.WriteLine("Area: "+area);
						Console.WriteLine("Ribbon: "+ribbon);

        }
    }
}
