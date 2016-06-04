using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day24
{
    class Program
    {
        private static int _bestCount = 100;
        private static int _size;
        private static bool partB = false;
        private static IEnumerable<long> packages;

        static void Main(string[] args)
        {
            packages = File.ReadAllLines("../../input").Select(long.Parse);
            _size = (int) packages.Sum()/3;
            Console.WriteLine("Minimum QE for part a: " + Find(packages.ToList()));
            _size = (int) packages.Sum()/4;
            _bestCount = 100;
            partB = true;
            Console.WriteLine("Minimum QE for part b: " + Find(packages.ToList()));
        }

        private static long Find(IEnumerable<long> packages)
        {
            // Order by descending to speed things up
            // Less packages are achieved with picking the highest first
            var list = GenerateDistribution(packages.OrderByDescending(p => p).ToList(), new List<long>());
            return list.Where(p => (int)(p.Count) == _bestCount).Min(p => p.Aggregate((a, b) => a*b));
        }

        private static List<List<long>> GenerateDistribution(List<long> available, List<long> used)
        {
            // We can no longer beat our best size
            if (used.Count > _bestCount)
                return new List<List<long>>();

            // The sum we still need to get perfect balance
            var missing = _size - used.Sum();
            // A return value
            var retVal = new List<List<long>>();
            // If we have have a perfect combination
            if (missing == 0)
            {
                // Create a new list of packages, ordered from highest to lowest to speed things up again
                var unused = packages.OrderByDescending(p => p).ToList();
                // Remove all packages we can't use anymore from that list
                used.ForEach(p => unused.Remove(p));
                // Run our check function to see if the packages are evenly distributable
                if (partB && !CheckB(unused, new List<long>())) return retVal;
                if (!partB && !Check(unused, new List<long>())) return retVal;
                // Update best count (if necessary)
                _bestCount = used.Count;
                // Add this to our return list
                retVal.Add(used.ToList());
            }
            else if (missing > 0)
            {
                // If we already are on our best count, adding a package will make everything worse!!
                // Just exit
                if (used.Count == _bestCount)
                    return retVal;
                // Run this recursively with all possible iterations
                for (int i = 0; i < available.Count; i++)
                {
                    var item = available[i];
                    if (item > missing) continue;

                    used.Add(item);
                    retVal.AddRange(GenerateDistribution(available.Skip(i+1).ToList(), used));
                    used.Remove(item);
                }
            }
            return retVal;
        }

        private static bool CheckB(List<long> available, List<long> used)
        {
            var missing = _size - used.Sum();
            if (missing == 0)
            {
                // Create a new list of packages, ordered from highest to lowest to speed things up again
                var unused = packages.OrderByDescending(p => p).ToList();
                // Remove all packages we can't use anymore from that list
                used.ForEach(p => unused.Remove(p));
                available.ForEach(p => unused.Remove(p));
                return Check(unused, new List<long>());
            }
            for (int i = 0; i < available.Count; i++)
            {
                var item = available[i];
                if (item > missing) continue;

                used.Add(item);
                if (CheckB(available.Skip(i + 1).ToList(), used))
                    return true;
                used.Remove(item);

            }
            return false;
        }

        private static bool Check(List<long> available, List<long> used)
        {
            var missing = _size - used.Sum();
            if (missing == 0) return true;
            for (int i = 0; i < available.Count; i++)
            {
                var item = available[i];
                if (item > missing) continue;

                used.Add(item);
                if (Check(available.Skip(i + 1).ToList(), used))
                    return true;
                used.Remove(item);

            }
            return false;
        }


    }
}
