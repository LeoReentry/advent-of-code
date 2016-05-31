using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day14
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Input
            var input = File.ReadAllLines("../../input");
            // Regex
            var r = new Regex(@"^(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+)");
            var reindeers = input.Select(l => r.Match(l).Groups.Cast<Group>().Select(g => g.Value).ToArray())
                .Select(c => new Reindeer(int.Parse(c[2]), int.Parse(c[3]), int.Parse(c[3]) + int.Parse(c[4]))).ToList();

            for (var i = 1; i <= 2503; i++)
            {
                reindeers.ForEach(s =>
                {
                    s.rounds = (int) Math.Floor(1.0*i/s.roundTime);
                    s.distance = s.speed*s.time*s.rounds +
                                 s.speed*(i - s.rounds*s.roundTime > s.time ? s.time : i - s.rounds*s.roundTime);
                });
                reindeers = reindeers.OrderByDescending(s => s.distance).ToList();
                var j = 0;
                while (true)
                {
                    reindeers[j].points++;
                    if (reindeers[j].distance == reindeers[j + 1].distance)
                        j++;
                    else
                        break;
                }
            }
            Console.WriteLine("Maximum distance: " + reindeers.Max(s => s.distance));
            Console.WriteLine("Maximum points: " + reindeers.Max(s => s.points));
        }

        private class Reindeer
        {
            public int distance;
            public int points;
            public int rounds;
            public readonly int roundTime;
            public readonly int speed;
            public readonly int time;

            public Reindeer(int speed, int time, int roundTime)
            {
                this.speed = speed;
                this.time = time;
                this.roundTime = roundTime;
                rounds = 0;
                points = 0;
                distance = 0;
            }
        }
    }
}