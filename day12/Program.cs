using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace day12
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string input = File.ReadLines("../../input").First();

			// Doing part 1 with Regex, just because 
			Console.Write("Solution for part 1: ");
			Console.WriteLine(Regex.Matches(input, @"[\-0-9]+").Cast<Match>().Select(match => int.Parse(match.Value)).Sum());

			var o = JsonConvert.DeserializeObject (input);
			Console.WriteLine ("Solution for part 2: {0}", recSum (o));

		}

		public static long recSum(dynamic o) {
			if (o is JObject) {
				if (((JObject)o).Properties().Select(t => t.Value).OfType<JValue>().Select(s => s.Value).Contains("red"))
					return 0;
				return ((JObject)o).Properties().Sum(a => recSum(a.Value));
			}
			if (o is JArray) {
				return ((JArray)o).Sum (e => recSum (e));
			}
			if (o is JValue) {
				if (o.Value is long)
					return o.Value;
				return 0;
			}
			return 0;
		}

	}
}
