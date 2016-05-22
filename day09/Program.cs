using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace day09
{
	class MainClass
	{
		// List with all the places Santa can visist
		public static List<string> keys;
		// List to store all the possible routes
		public static List<string[]> permutations = new List<string[]>();
		// Dicyionary with all distances
		public static Dictionary<string, Dictionary<string, int>> routes = new Dictionary<string, Dictionary<string, int>>();

		public static void Main (string[] args)
		{
			// read file
			using (StreamReader sr = new StreamReader("../../input")) {
				string line;

				// Regex to match origin, destination and distance
				Regex r = new Regex(@"(\w+) to (\w+) = (\d+)");

				// Fill routes with all distances
				while ((line = sr.ReadLine()) != null) {

					// Match origin, destination and distannce
					GroupCollection groups = r.Match(line).Groups;
					string origin = groups [1].ToString ();
					string destination = groups [2].ToString ();
					int distance = Int32.Parse (groups [3].ToString ());

					// If Subdictionary is unitialized, initialize it
					if (!routes.ContainsKey(destination))
						routes [destination] = new Dictionary<string, int> ();
					if (!routes.ContainsKey(origin))
						routes [origin] = new Dictionary<string, int> ();

					// Assign distances
					routes [destination] [origin] = distance;						
					routes [origin] [destination] = distance;					
				}

				// Save all the keys (places santa can visit) to a list
				keys = routes.Keys.ToList ();

				// Generate all permutations recursively
				RecursiveSearch (keys, new List<string>());

				// Calculate the length of all the possible routes
				List<int> lengths = new List<int> ();
				foreach (string[] s in permutations) {
					// Iterate through all places
					int length = 0;
					for(int i = 0; i < s.Length - 1; i++) {
						// Add the distance between place s[i] and place s[i+1] to the total length of that route
						length += routes [s [i]] [s [i + 1]];
					}
					// Store that length in list
					lengths.Add (length);
				}
				// PART 1: Shortest distance
				// PART 2: Longest distance
				Console.WriteLine ("Shortest distance: " + lengths.Min());
				Console.WriteLine ("Longest distance: " + lengths.Max());
			}
		}

		public static void RecursiveSearch(List<string> missing, List<string> current) {
			// Missing contains all the missing places for the current permutation
			// If it has no elements, we have completed the permutation
			// Store it as array in our permutations list
			if (missing.Count == 0) {
				string[] permutation = new string[current.Count];
				current.CopyTo (permutation);
				permutations.Add (permutation);
			} else {
				// If we still have places to visit, visist each place and search for the other possibilities recursively
				foreach(string s in missing) {
					current.Add (s);
					List<string> newMissing = missing.Where (el => !el.Equals (s)).ToList();
					RecursiveSearch (newMissing, current);
					current.Remove (s);
				}

			}
		}
	}
}
