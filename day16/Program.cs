using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day16
{
    class Program
    {
        private static readonly Dictionary<string, int> Sue = new Dictionary<string, int> {
                {"children", 3 },
                {"cats", 7 },
                {"samoyeds", 2 },
                {"pomeranians", 3 },
                {"akitas", 0 },
                {"vizslas", 0 },
                {"goldfish", 5 },
                {"trees", 3 },
                {"cars", 2 },
                {"perfumes", 1 }
        };

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("../../input");
            var r = new Regex(@"(\w+): (\d+)");
            var sues = input.Select(l => r.Matches(l).Cast<Match>().ToDictionary(
                    s => s.Groups[1].Value, 
                    t => int.Parse(t.Groups[2].Value)
                )).ToList();

            Console.WriteLine("We got the present from Sue {0}", sues.FindIndex(IsSue) + 1);
            Console.WriteLine("Oops, we actually gut it from Sue {0}", sues.FindIndex(IsSuePart2) + 1);
        }

        private static bool IsSue(Dictionary<string, int> possibleSue)
        {
            return possibleSue.All(kvp => kvp.Value == Sue[kvp.Key]);
        }
        private static bool IsSuePart2(Dictionary<string, int> possibleSue)
        {
            return possibleSue.All(kvp =>   kvp.Key == "cats" || kvp.Key == "trees" ? kvp.Value > Sue[kvp.Key] :
                                            kvp.Key == "pomeranians" || kvp.Key == "goldfish" ? kvp.Value < Sue[kvp.Key] :
                                            possibleSue[kvp.Key] == Sue[kvp.Key]);
        }
    }
}
