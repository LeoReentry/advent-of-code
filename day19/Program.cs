using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day19
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("../../input");
            var startMolecule = input.Last();
            var r = new Regex(@"(\w+) => (\w+)");
            var replacements =
                input.Select(s => r.Match(s).Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray())
                    .Where(e => e.Length > 0)
                    .ToList();
            var possibleMolecules = Replacements(replacements, startMolecule);
            Console.WriteLine($"There are {possibleMolecules.Distinct().Count()} different molecules we can generate with one replacement.");

            var endMolecule = startMolecule;
            startMolecule = "e";
            Console.WriteLine($"There are {Search(startMolecule, endMolecule, replacements)} steps required to generate the medicine molecule from an electron.");
            ;

        }

        private static int Search(string startMolecule, string endMolecule, List<string[]> replacements)
        {
            var depth = 0;
            var currentMolecule = endMolecule;
            replacements = replacements.OrderByDescending(r => r[1].Length).ToList();
            // Our backwards path
            var path = new List<string>();
            // The number of the replacement we had for each step
            var repIndex = new List<int>();
            // A number of tries we had for each step
            var tries = new List<int>();

            path.Add(endMolecule);
            tries.Add(0);
            while (currentMolecule != startMolecule)
            {
                Console.Write("\rCurrent depth: " + depth);

                // We enter this depth for the first time
                if (repIndex.Count <= depth)
                    repIndex.Add(0);


                // Get the next best replacement we haven't tried at this depth yet
                var currentReplacement = replacements[repIndex[depth]];
                // Try to get the new molecule
                var tmp = ReverseReplace(currentReplacement, currentMolecule, tries[depth]);
                // This if block tests if we are in a place with no return
                if (repIndex[depth] == replacements.Count - 1 && tmp.Equals(""))
                {
                    // Go back a step
                    repIndex[depth] = 0;
                    tries[depth] = 0;
                    depth--;
                    path.Remove(currentMolecule);
                    currentMolecule = path.Last();
                }
                // If we were unsuccessful, get to the next replacement string
                if (tmp.Equals(""))
                {
                    repIndex[depth]++;
                    tries[depth] = 0;
                    continue;
                }
                tries[depth]++;
                depth++;
                currentMolecule = tmp;
                path.Add(tmp);
                tries.Add(0);
            }
            Console.Write("\r");
            return depth;
        }

        private static IEnumerable<string> Replacements(IEnumerable<string[]> replacements, string molecule)
        {
            var generatedMolecules = new List<string>();
            foreach (var replacement in replacements)
                generatedMolecules.AddRange(Replace(replacement, molecule));
            return generatedMolecules;
        }

        private static IEnumerable<string> Replace(IReadOnlyList<string> replacement, string molecule)
        {
            var originalElement = replacement[0];
            var replacedBy = replacement[1];
            var indices = Regex.Matches(molecule, originalElement).Cast<Match>().Select(m => m.Index).ToArray();
            return indices.Select(index => 
                // Generate new string out of substrings
                molecule.Substring(0, index) + replacedBy + ((index+ originalElement.Length >= molecule.Length) ? "" : molecule.Substring(index + originalElement.Length))
                ).ToList();
        }

        private static string ReverseReplace(IReadOnlyList<string> replacement, string molecule, int index)
        {
            var originalElement = replacement[1];
            var replacedBy = replacement[0];
            var indices = Regex.Matches(molecule, originalElement).Cast<Match>().Select(m => m.Index).ToArray();

            if (indices.Length == index || indices.Length == 0) return "";
            var idx = indices[index];

            return molecule.Substring(0, idx) + replacedBy +
                   ((idx + originalElement.Length >= molecule.Length)
                       ? ""
                       : molecule.Substring(idx + originalElement.Length));

        }
    }
}
