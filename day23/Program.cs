using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day23
{
    class Program
    {
        static void Main(string[] args)
        {
            // Yay, assembly language ^_^
            var input = File.ReadAllLines("../../input");
            var commands = input.Select(s => s.Split(' ')).ToArray();
            // PART A
            //var r = new Dictionary<char, uint> { { 'a', 0}, { 'b', 0 } };
            // PART B
            var r = new Dictionary<char, uint> {{'a', 1}, {'b', 0}};
            int i = 0;
            while(i < commands.Length)
            {
                var command = commands[i][0];
                var reg = commands[i][1][0];
                Console.WriteLine($"C={i + 1,2}: {input[i]}");
                switch (command)
                {
                    case "hlf":
                        r[reg] >>= 1;
                        goto case "increment";
                    case "tpl":
                        r[reg] = r[reg] + (r[reg] << 1);
                        goto case "increment";
                    case "inc":
                        r[reg]++;
                        goto case "increment";
                    case "jmp":
                        i += int.Parse(commands[i][1]);
                        break;
                    case "jie":
                        if ((r[reg] & (uint)1) == 0)
                        {
                            i += int.Parse(commands[i][2]);
                            break;
                        }
                        goto case "increment";
                    case "jio":
                        if (r[reg] == 1)
                        {
                            i += int.Parse(commands[i][2]);
                            break;
                        }
                        goto case "increment";
                    case "increment":
                        i++;
                        break;
                }
                Console.WriteLine($"RA: {r['a'],5}\nRB: {r['b'],5}");
            }
            Console.WriteLine($"\nFINISHED\nRA: {r['a'],5}\nRB: {r['b'],5}");
            
        }
    }
}
