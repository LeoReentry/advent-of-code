using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day17
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Array.ConvertAll(File.ReadAllLines("../../input"), int.Parse);

            var storagePossibilites = search(150, new List<int>(), input.ToList());
            Console.WriteLine("We have {0} possibilities of storing the eggnog.", storagePossibilites.Count);
            var minContainers = storagePossibilites.Select(p => p.Length).ToList().Min();
            var possibleSolutions = storagePossibilites.Count(c => c.Length == minContainers);
            Console.WriteLine("We have {0} possibilities of storing the eggnog with only {1} containers.", possibleSolutions, minContainers);


        }

        private static List<int[]> search(int maxSize, List<int> used, List<int> available)
        {
            var returnlist = new List<int[]>();
            var missing = maxSize - used.Sum();
            if (missing == 0)
                returnlist.Add(used.ToArray());
            else if (missing > 0)
            {
                for (int i = 0; i < available.Count; i++)
                {
                    used.Add(available[i]);
                    var newAvailable = available.Skip(i + 1).ToList();
                    returnlist.AddRange(search(maxSize, used, newAvailable));
                    used.Remove(available[i]);
                }

            }
            return returnlist;
        }
    }
}
