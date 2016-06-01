using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day18
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = File.ReadAllLines("../../input").Select(s => s.Select(c => c == '#').ToArray()).ToArray();

            // Part 1
            var part1 = start.Select(a => a.ToArray()).ToArray();
            for (var i = 0; i < 100; i++)
                part1 = Step(part1, false);
            Console.WriteLine($"After 100 steps, {part1.Select(s => s.Count(e => e)).Sum()} lights are turned on.");

            // Part 2
            var part2 = start.Select(a => a.ToArray()).ToArray();
            // Initialize corners to true
            part2[0][0] = true;
            part2[0][part2[0].Length - 1] = true;
            part2[part2.Length - 1][0] = true;
            part2[part2.Length - 1][part2[0].Length - 1] = true;
            for (var i = 0; i < 100; i++)
                part2 = Step(part2, true);
            Console.WriteLine(
                $"After 100 steps with corners stuck, {part2.Select(s => s.Count(e => e)).Sum()} lights are turned on.");
        }

        private static bool[][] Step(IReadOnlyList<bool[]> current, bool part2)
        {
            var newArr = current.Select(a => a.ToArray()).ToArray();
            for (var i = 0; i < current.Count; i++)
            {
                for (var j = 0; j < current[0].Length; j++)
                {
                    var count = GetNeighbourCount(current, i, j);
                    if (part2 && (i == 0 || i == current.Count - 1) && (j == 0 || j == current[0].Length - 1))
                        continue;
                    if (current[i][j] && count != 2 && count != 3) newArr[i][j] = false;
                    if (!current[i][j] && count == 3) newArr[i][j] = true;
                }
            }
            return newArr;
        }

        private static int GetNeighbourCount(IReadOnlyList<bool[]> arr, int x, int y)
        {
            Tuple<int, int>[] neighbours = {
                Tuple.Create(-1, -1), Tuple.Create(0, -1), Tuple.Create(1, -1),
                Tuple.Create(-1,  0),                      Tuple.Create(1,  0),
                Tuple.Create(-1,  1), Tuple.Create(0,  1), Tuple.Create(1,  1)
            };
            if (x == 0)                         neighbours = neighbours.Where(t => t.Item1 > -1).ToArray();
            else if (x == arr.Count - 1)        neighbours = neighbours.Where(t => t.Item1 < 1).ToArray();
            if (y == 0)                         neighbours = neighbours.Where(t => t.Item2 > -1).ToArray();
            else if (y == arr[0].Length - 1)    neighbours = neighbours.Where(t => t.Item2 < 1).ToArray();

            return neighbours.Sum(t => Convert.ToInt32(arr[x+t.Item1][y+t.Item2]));
        }
    }
}
