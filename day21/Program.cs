using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day21
{
    class Program
    {
        private static readonly Tuple<int, int>[] Weapons ={Tuple.Create(8,4),Tuple.Create(10,5),Tuple.Create(25,6),Tuple.Create(40,7),Tuple.Create(74,8)};
        private static readonly Tuple<int,int>[] Armors = { Tuple.Create(0,0), Tuple.Create(13, 1), Tuple.Create(31, 2), Tuple.Create(53, 3), Tuple.Create(75, 4), Tuple.Create(102, 5) };
        private static readonly Tuple<int, int, int>[] Rings = { Tuple.Create(0, 0, 0), Tuple.Create(0, 0, 0), Tuple.Create(20, 1,0), Tuple.Create(40, 2,0), Tuple.Create(80, 3,0), Tuple.Create(25,0, 1), Tuple.Create(50,0, 2), Tuple.Create(100,0, 3) };
        static void Main(string[] args)
        {
            var equipments = new List<Equipment>();
            foreach (var weapon in Weapons)
                foreach (var armor in Armors)
                    for (var k = 0; k < Rings.Length - 1; k++)
                        for (var l = k+1; l < Rings.Length; l++)
                        {
                            equipments.Add(new Equipment(
                                gold: weapon.Item1 + armor.Item1 + Rings[k].Item1 + Rings[l].Item1, 
                                damage: weapon.Item2 + Rings[k].Item3 + Rings[l].Item3, 
                                armor: armor.Item2 + Rings[k].Item2 + Rings[l].Item2));
                        }
            foreach (var equipment in equipments.OrderBy(e => e.Gold))
            {
                if (!Fight(new Player(equipment.Damage, equipment.Armor), new Boss())) continue;
                Console.WriteLine("Minimum gold you have to spend on a fight is " + equipment.Gold);
                break;
            }
            foreach (var equipment in equipments.OrderByDescending(e => e.Gold))
            {
                if (Fight(new Player(equipment.Damage, equipment.Armor), new Boss())) continue;
                Console.WriteLine("Maximum gold you can spend and still lose is " + equipment.Gold);
                break;
            }
        }

        private static bool Fight(Player player, Boss boss)
        {
            var playerDamage = Math.Max(1, player.Damage - boss.Armor);
            var bossDamage = Math.Max(1, boss.Damage - player.Armor);
            while (player.HitPoints > 0 && boss.HitPoints > 0)
            {
                boss.HitPoints -= playerDamage;
                player.HitPoints -= bossDamage;
            }
            return boss.HitPoints <= 0;
        }
    }

    internal class Equipment
    {
        public Equipment(int gold, int damage, int armor)
        {
            Gold = gold;
            Damage = damage;
            Armor = armor;
        }

        public int Gold { get;  }
        public int Damage { get; }
        public int Armor { get; }
    }
    internal class Player
    {
        public int Damage { get; set; }
        public int Armor { get; set;  }
        public int HitPoints { get; set; }

        public Player()
        {
            this.Damage = 0;
            this.Armor = 0;
            this.HitPoints = 100;
        }
        public Player(int damage, int armor)
        {
            this.Damage = damage;
            this.Armor = armor;
            this.HitPoints = 100;
        }
    }

    internal class Boss : Player
    {
        public Boss()
        {
            this.Damage = 8;
            this.Armor = 1;
            this.HitPoints = 104;
        }
    }

}
