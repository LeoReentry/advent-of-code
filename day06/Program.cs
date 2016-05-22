using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace day06
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            string[] instructions = File.ReadAllLines("../../input");

			// Part 1
			// Bitarray more efficient, but hey, 1 million booles is only 1 MB...
			bool[,] lights = new bool[1000, 1000];
			// Part 2
			int[,] lightsBrightness = new int[1000, 1000];

			// Regex for getting the numbers of the instructions
			Regex r = new Regex (@"(\d+),(\d+) through (\d+),(\d+)");
			// Array for storing these indices
			int[] i = new int[4];
			foreach (string s in instructions) {
				// Get the numbers
				Match match = r.Match (s);
				i [0] = Int32.Parse (match.Groups [1].ToString ());
				i [1] = Int32.Parse (match.Groups [2].ToString ());
				i [2] = Int32.Parse (match.Groups [3].ToString ());
				i [3] = Int32.Parse (match.Groups [4].ToString ());

				if (s.StartsWith ("turn off")) {
					editArrBool (lights, x => false, i);
					editArrByte (lightsBrightness, x => x - 1, i);
				} else if (s.StartsWith ("toggle")) {
					editArrBool (lights, x => !x, i);
					editArrByte (lightsBrightness, x => x + 2, i);
				} else {
					editArrBool (lights, x => true, i);		
					editArrByte (lightsBrightness, x => x + 1, i);			
				}
			}
			List<bool> list = lights.Cast<bool> ().ToList ();
			List<int> listBrightness = lightsBrightness.Cast<int> ().ToList ();
			Console.WriteLine ("There are {0} lights turned on ", list.Count (c => c));
			Console.WriteLine ("There is a total brightness of {0}", listBrightness.Sum ());

		}

		public static void editArrBool (bool[,] arr, Func<bool, bool> op, int[] range)
		{
			for (int i = range [0]; i <= range [2]; i++) {
				for (int j = range [1]; j <= range [3]; j++) {
					arr [i, j] = op (arr [i, j]);
				}
			}
		}

		public static void editArrByte (int[,] arr, Func<int, int> op, int[] range)
		{
			for (int i = range [0]; i <= range [2]; i++) {
				for (int j = range [1]; j <= range [3]; j++) {
					arr [i, j] = op (arr [i, j]);
					if (arr [i, j] < 0)
						arr [i, j] = 0;
				}
			}
		}
	}
}
