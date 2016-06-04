using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day25
{
    class Program
    {
        static void Main(string[] args)
        {
            const ulong x = 3010, y = 3019;
            ulong a = 20151125;
            ulong r = 1, c = 1;

            while (true)
            {
                c++;
                r--;
                if (r == 0)
                {
                    r = c;
                    c = 1;
                }
                a = (a*252533L)%33554393L;
                if (r == x && c == y)
                    break;
            }
            Console.WriteLine("Solution is: " + a);

        }
    }
}
