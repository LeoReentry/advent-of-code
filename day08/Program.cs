using System;
using System.IO;
using System.Text.RegularExpressions;

namespace day08
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Open the text file using a stream reader.
            using (StreamReader sr = new StreamReader("../../input.txt"))
            {
                string line;
                int length = 0;
                int unescapedLength = 0;
                int escapedLength = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    // Add line length (all characters)
                    length += line.Length;
                    // PART 1
                    // Unescape the line and add the length of that line
                    // Length - 2, because the quotes at the beginning and end don't unescape into a string variable
                    unescapedLength += Regex.Unescape(line).Length - 2;
                    // PART 2
                    // Remove first and last character
                    // Two extra characters for new quotes
                    escapedLength += Regex.Escape(line).Length + 2;
                    // Count the ocurrences of " and add an extra character for each, since Regex.Escape doesn't escape quotes
					escapedLength += line.Split ('"').Length - 1;
                }
                // Difference of both
                Console.WriteLine("Length: {0}\nUnescaped: {1}\nEscaped: {3}\nDiff Part 1: {2}\nDiff Part 2: {4}", length, unescapedLength, length - unescapedLength, escapedLength, escapedLength - length);
            }
        }

    }
}
