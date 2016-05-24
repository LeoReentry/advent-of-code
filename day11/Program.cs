using System;
using System.Text.RegularExpressions;
using System.Text;

namespace day11
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string input = "vzbxkghb";
			bool second = false;
			// This regex matches if there are two occurrences of non overlapping pairs of letters that are different
			Regex r1 = new Regex(@"(\w)\1.*(?!\1)(\w)\2");
			// This regex matches if the characters i, o or l are included
			Regex r2 = new Regex(@"^[^iol]+$");
			// This regex matches three characters in alphabetical order, but only of our reduced alphabet not containing iol
			Regex r3 = new Regex(@"abc|bcd|cde|def|efg|fgh|mnp|pqr|qrs|rst|stu|tuv|uvw|vwx|wxy|xyz");

			while(true) {
				if (r1.IsMatch (input) && r2.IsMatch (input) && r3.IsMatch (input)) {
					if (second) {
						Console.WriteLine ("\rSolution for part 2: " + input);
						break;
					}
					Console.WriteLine ("\rSolution for part 1: " + input);
					second = true;
					input = incrementString (input);
				} else {
					input = incrementString (input);
					Console.Write ("\rTesting string: " + input);
				}
			}

		}

		// Function that increments a string from 'abc' to 'abd' and from 'abz' to 'aca'
		public static string incrementString(string str) {

			// Look for forbidden characters and replace them 
			int idx = str.IndexOf('i');
			if (idx > -1)
				return str.Substring (0, idx) + 'j' + new string ('a', str.Length - idx - 1);
			idx = str.IndexOf('l');
			if (idx > -1)
				return str.Substring (0, idx) + 'm' + new string ('a', str.Length - idx - 1);
			idx = str.IndexOf('o');
			if (idx > -1)
				return str.Substring (0, idx) + 'p' + new string ('a', str.Length - idx - 1);

			// Using StringBuilder since string is immutable object and this is more efficient than creating new strings
			StringBuilder sb = new StringBuilder (str);


			for(int i = sb.Length - 1; i >= 0; i--) {
				if (sb[i] < 'z') {
					sb [i]++;
					return sb.ToString ();
				} else 
					sb[i] = 'a';
			}

			return sb.ToString();
		}
	}
}
