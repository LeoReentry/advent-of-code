using System;
using System.Text.RegularExpressions;
using System.Text;

namespace day10
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string input = "1113122113";
			string pattern = @"(\d)\1*";
			for(int i = 0; i < 50; i++){
				MatchCollection matches = Regex.Matches (input, pattern);
				StringBuilder sb = new StringBuilder ();
				foreach (Match m in matches) {
					string match = m.ToString ();
					sb.Append(match.Length.ToString() + match [0]);
				}
				input = sb.ToString();
				Console.WriteLine ("Iteration {0:00}: Length is {1}", i+1, input.Length);
			}
		}
	}
}
