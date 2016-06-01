using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day20
{
    class Program
    {
        static void Main(string[] args)
        {
            const int minPresents = 29000000;
            // We need at most a tenth of the houses than presents, since elf #minPresents/10 will deliver minPresents presents to that house
            var houses = new int[minPresents/10];
            for (var elf = 1; elf < minPresents/10; elf++)
            {
                // Each elf starts delivering at the house of its own number and increases by that number
                for (var houseN = elf; houseN < houses.Length; houseN += elf)
                {
                    houses[houseN - 1] += elf*10;
                }
            }
            Console.WriteLine($"First house with over {minPresents} presents is house number {houses.ToList().FindIndex(h => h > minPresents) + 1}.");

            houses = new int[(int)Math.Ceiling(1.0 * minPresents / 11)];
            for (var elf = 1; elf < houses.Length; elf++)
            {
                var n = 0;
                
                for (var houseN = elf; houseN < houses.Length; houseN+=elf)
                {
                    houses[houseN - 1] += elf*11;
                    n++;
                    if (n == 50) break;
                }
            }
            Console.WriteLine($"First house with over {minPresents} presents for part 2 is house number {houses.ToList().FindIndex(h => h > minPresents) + 1}.");
        }
    }
}
