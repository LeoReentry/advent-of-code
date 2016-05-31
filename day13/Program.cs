using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day13
{
    class Program
    {
        public static List<string[]> PermutationsList = new List<string[]>();

        static void Main(string[] args)
        {
            // Input
            var input = File.ReadAllLines("../../input");
            // Regex to match all variables
            Regex r = new Regex(@"^(\w+) would (\w+) (\d+) happiness units by sitting next to (\w+)");
            // Array with all groups for each line
            var instructions = input.Select(selector: el => r.Match(el).Groups.Cast<Group>().Select(selector: c => c.Value).ToArray())
            // Generate an object out of each and turn everything into array
            .Select(selector: el => new
             {
                Name = el[1],
                Neighbour = el[4],
                Happiness = (el[2].Equals("gain") ? 1 : -1) * Int32.Parse(el[3])
             }).ToArray();
            // Get a list of different names
            var names = instructions.Select(i => i.Name).Distinct().ToList();
            // Get a dictionary of happiness 
            var happiness = instructions.ToDictionary(i => i.Name + i.Neighbour, i => i.Happiness);
            // Calculate all possible permutations
            permutations(names.Count, names);

            var maxHappiness = PermutationsList.Select(
                // Generate pairwise of every permutation
                selector: p => p.Select((e, i) => new {A = e, B = p[(i + 1) == p.Length ? 0 : i + 1]})
                    // Sum the happiness of those pairs
                    .Sum(pair => happiness[pair.A+pair.B] + happiness[pair.B+pair.A]))
                    // Get the maximum happiness
                    .Max();

            Console.WriteLine("Maximum happiness: " + maxHappiness);
            
            // ===============
            // PART 2
            // ===============
            // Include myself in happiness dictionary
            foreach(string name in names)
            {
                happiness[name + "Me"] = 0;
                happiness["Me"+ name] = 0;
            };
            // Add myself to list of names
            names.Add("Me");
            // Clear all permutations
            PermutationsList = new List<string[]>();
            // Generate new permutations
            permutations(names.Count, names);
            // Calculate happiness as before
            maxHappiness = PermutationsList.Select(
                selector: p => p.Select((e, i) => new { A = e, B = p[(i + 1) == p.Length ? 0 : i + 1] })
                    .Sum(pair => happiness[pair.A + pair.B] + happiness[pair.B + pair.A])).Max();
            Console.WriteLine("Maximum happiness with me: " + maxHappiness);

        }

        private static void permutations(int n, List<string> names)
        {
            if (n == 1)
            {
                PermutationsList.Add(names.ToArray());
                return;
            }
            for (int i = 0; i < n-1; i++)
            {
                permutations(n-1, names);
                var swap = n%2 == 0 ? i : 0;
                var tmp = names[swap];
                names[swap] = names[n - 1];
                names[n - 1] = tmp;
            }
            permutations(n-1, names);
        }
    }
}
