using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace day03
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string input = File.ReadLines("../../input").First();
			Dictionary < Tuple<int, int>, int> houses = new Dictionary<Tuple<int, int>, int> ();
			int x = 0, y = 0, santaX = 0, santaY = 0, robotX = 0, robotY = 0;
			bool robot = false;
			houses [Tuple.Create (0, 0)] = 1;
			foreach (char c in input) {
				if (robot) {
					if (c.Equals ('>'))
						robotX++;
					else if (c.Equals ('<'))
						robotX--;
					else if (c.Equals ('^'))
						robotY++;
					else if (c.Equals ('v'))
						robotY--;
					x = robotX;
					y = robotY;
					robot = !robot;
				} else {
					if (c.Equals ('>'))
						santaX++;
					else if (c.Equals ('<'))
						santaX--;
					else if (c.Equals ('^'))
						santaY++;
					else if (c.Equals ('v'))
						santaY--;
					x = santaX;
					y = santaY;
					robot = !robot;					
				}
				// Key and val
				var key = Tuple.Create (x, y);
				int val;
				// Try get value. If key does not exist, value will be setto 0.
				houses.TryGetValue (key, out val);
				houses [key] = ++val;
			}
			foreach (KeyValuePair<Tuple<int,int>, int> kvp in houses) {
				Console.WriteLine ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);				
			}
			Console.WriteLine (houses.LongCount ());
//			Console.WriteLine (houses.Where (r => r.Value > 0).LongCount ());
		}
	}
}
