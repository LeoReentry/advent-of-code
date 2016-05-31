using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day15
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("../../input");
            var r = new Regex(@"(-?\d+)");
            var ingredients = input.Select(l => r.Matches(l).Cast<Match>().Select(m => Int32.Parse(m.Value)));
            // Ingredient's properties except calories
            var properties = ingredients.Select(i => i.Reverse().Skip(1).ToArray()).ToArray();
            // Calories for each ingredient
            var calories = ingredients.Select(i => i.Last()).ToArray();
            // All possible distributions of ingredients
            var distributions = GenerateDistribution(0, 100, new int[properties.Length]);
            // Calculate quality for each distribution
            var qualities = distributions.Select(d => CalcScore(d, properties));
            Console.WriteLine("Maximum achievable quality: " + qualities.Max());

            // PART 2
            // Get all distributions where calory count is exactly 500
            var distributions_500 = distributions.Where(d => d.Zip(calories, (a, c) => a*c).Sum() == 500);
            // Calculate quality
            var qualities_500 = distributions_500.Select(d => CalcScore(d, properties));
            Console.WriteLine("Maximum achievable quality with 500 calories: " + qualities_500.Max());
        }

        private static int CalcScore(int[] distribution, int[][] ingredients)
        {
            // Calculate the score for each ingredient by multipying each property with the ingredient's amount
            var scores = distribution.Zip(ingredients, (n, ingredient) => ingredient.Select(p => p*n));
            // Add the scores of all ingredients property-wise
            var scoreSum = scores.Aggregate((a, b) => a.Zip(b, (j,k) => j+k));
            // Now that we have a final value for each property, multiply all the properties. If a property has negative quality, it becomes 0
            return scoreSum.Aggregate((a, b) => (a<0 || b<0) ? 0 : a*b);
        }

        private static List<int[]> GenerateDistribution(int idx, int maxCount, int[] arr)
        {
            var missing = maxCount - arr.Sum();
            if (idx == arr.Length - 1)
            {
                arr[idx] = missing;
                return new List<int[]>() {arr.ToArray()};
            }
            List<int[]> list = new List<int[]>();
            for (int i = 0; i <= missing; i++)
            {
                var newArr = arr.ToArray();
                newArr[idx] = i;
                list.AddRange(GenerateDistribution(idx + 1, maxCount, newArr));
            }
            return list;
        }
    }
}
