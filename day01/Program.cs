using System;
using System.Linq;
using System.IO;

namespace day01
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string input = File.ReadLines("../../input").First();
			int floor = 0;
			bool basement = false;
			for (int i = 0; i < input.Length; i++) {
				if (input [i].Equals ('('))
					floor++;
				else
					floor--;
				if (floor == -1 && !basement) {
					Console.WriteLine ("Enter basement on position " + (i + 1));
					basement = true;
				}
			}
			Console.WriteLine ("We have " + floor + " floors.");

		}
	}
}
