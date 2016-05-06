using System;
using System.Security.Cryptography;

namespace day04
{
	class MainClass
	{
		public static string ByteArrayToString (byte[] ba)
		{
			string hex = BitConverter.ToString (ba);
			return hex.Replace ("-", "");
		}

		public static void Main (string[] args)
		{
			const string input = "ckczppom";
			MD5 md5 = System.Security.Cryptography.MD5.Create ();
			int i = 0;
			while (true) {
				byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes (input + i);
				byte[] hash = md5.ComputeHash (inputBytes);	
				string hashString = ByteArrayToString (hash);
				if (hashString.StartsWith ("00000"))
					Console.WriteLine ("\rFound nonce for part 1: " + i);
				if (hashString.StartsWith ("000000")) {
					Console.WriteLine ("\rFound nonce for part 2: " + i);
					break;
				} 
				i++;
				Console.Write ("\r" + i);
			}
		}
	}
}