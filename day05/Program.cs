using System;
using System.Text.RegularExpressions;
using System.IO;

namespace day05
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string[] strings = File.ReadAllLines("../../input");

			// Part one
			Regex r1 = new Regex (@"[aeiou].*[aeiou].*[aeiou]");
			Regex r2 = new Regex (@"(\w)\1");
			Regex r3 = new Regex (@"ab|cd|pq|xy");
			Regex r4 = new Regex (@"(.{2}).*\1");
			Regex r5 = new Regex (@"(.).\1");
			// Part two
			int count1 = 0;
			int count2 = 0;
			foreach (string s in strings) {
				if (r1.IsMatch (s) && r2.IsMatch (s) && !r3.IsMatch (s))
					count1++;
				if (r4.IsMatch (s) && r5.IsMatch (s))
					count2++;
				
			}
			Console.WriteLine ("In part 1 the number of strings is " + count1);
			Console.WriteLine ("In part 2 the number of strings is " + count2);

		}
	}
}
