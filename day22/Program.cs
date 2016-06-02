using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace day22
{
    class Program
    {
        static Random rand = new Random();
        static void Main(string[] args)
        {
            var lowestCost = int.MaxValue;
            // Many, many, many iterations for hard mode
            for (int i = 0; i < 3000000; i++)
            {
                var result = RandomFight(new Wizard(), new Boss(), lowestCost, hardmode: false);
                if (result == 0 || result > lowestCost) continue;
                lowestCost = result;
                Console.Write("\rCurrent lowest cost is: {0:D10}", result);
            }
            Console.WriteLine();
        }

        private static int RandomFight(Wizard player, Character boss, int lowestCost, int seed = 0, bool hardmode = false)
        {
            var totalCost = 0;
            //var rand = seed == 0 ? new Random() : new Random(seed);
            while (player.HitPoints > 0 && boss.HitPoints > 0)
            {
                // Player turn
                if (hardmode)
                {
                    player.HitPoints--;
                    if (player.HitPoints == 0) return 0;
                }
                // All effects that still have a counter will now be activated
                player.ActivateEffects(boss);
                // If boss is dead, return the total amount of mana spent
                if (boss.HitPoints <= 0) return totalCost;
                // Get a list of all spells we can use
                var spells = player.GetCastableSpells().Where(s => s.Cost <= player.Mana && s.Cost + totalCost < lowestCost).ToList();
                // If we can cast no spell, we lost
                if (!spells.Any()) return 0;
                // Cast a random spell
                var spell = spells[rand.Next(spells.Count)];
                spell.Cast(player, boss);
                totalCost += spell.Cost;

                // Boss turn
                // All effects that still have a counter will now be activated
                player.ActivateEffects(boss);
                // If boss is dead, return the total amount of mana spent
                if (boss.HitPoints <= 0) return totalCost;
                // Boss attacks
                player.HitPoints -= Math.Max(1, boss.Damage - player.Armor);
                // Check if player is still alive
                if (player.HitPoints <= 0) return 0;
            }
            return totalCost;
        }
    }

    #region Spells
    internal class Spells : IEnumerable<Spells.Spell>
    {
        #region Instantiation of all Spells
        public MagicMissileSpell MagicMissile { get; } = new MagicMissileSpell();
        public DrainSpell Drain { get; } = new DrainSpell();
        public PoisonSpell Poison { get; } = new PoisonSpell();
        public ShieldSpell Shield { get;  } = new ShieldSpell();
        public RechargeSpell Recharge { get; }  = new RechargeSpell();
        private List<Spell> _spellList;
        #endregion

        #region Constructor
        public Spells()
        {
            _spellList = new List<Spell> { MagicMissile, Drain, Poison, Shield, Recharge };
        }
        #endregion

        #region Spell Classes

        internal abstract class Spell
        {
            #region Fields and Properties
            protected int _turns;
            public int Turns => _turns;
            public int Cost { get; }
            public int Damage { get; }
            public int Armor { get; }
            public int Heal { get; }
            public int Recover { get; }
            #endregion

            protected Spell(int cost, int damage = 0, int armor = 0, int heal = 0, int recover = 0)
            {
                Cost = cost;
                _turns = 0;
                Damage = damage;
                Armor = armor;
                Heal = heal;
                Recover = recover;
            }

            #region Methods
            public virtual bool Cast(Wizard w, Character c)
            {
                if (w.Mana < Cost)
                    return false;
                w.Mana -= Cost;
                c.HitPoints -= Damage;
                w.Armor += Armor;
                w.Mana += Recover;
                w.HitPoints += Heal;
                return true;
            }

            public bool Effect(Wizard w, Character c)
            {
                if (Turns <= 0)
                    return false;
                c.HitPoints -= Damage;
                w.Mana += Recover;
                w.HitPoints += Heal;
                _turns--;
                if (Turns == 0)
                    w.Armor -= Armor;
                return true;
            }
            #endregion
        }
        internal class MagicMissileSpell : Spell
        {
            public MagicMissileSpell() : base(cost: 53, damage:4) { }
        }

        internal class DrainSpell : Spell
        {
            public DrainSpell() : base(cost: 73, damage: 2, heal: 2) { }
        }

        internal class ShieldSpell : Spell
        {
            private const int Duration = 6;
            public ShieldSpell() : base(cost: 113, armor: 7) { }

            public override bool Cast(Wizard w, Character c)
            {
                if (w.Mana < Cost)
                    return false;
                w.Mana -= Cost;
                w.Armor += Armor;
                _turns = Duration;
                return true;
            }
        }

        internal class PoisonSpell : Spell
        {
            private const int Duration = 6;
            public PoisonSpell() : base(cost: 173, damage: 3) { }

            public override bool Cast(Wizard w, Character c)
            {
                if (w.Mana < Cost)
                    return false;
                w.Mana -= Cost;
                w.Armor += Armor;
                _turns = Duration;
                return true;
            }
        }

        internal class RechargeSpell : Spell
        {
            private const int Duration = 5;
            public RechargeSpell() : base(cost:229, recover:101) { }

            public override bool Cast(Wizard w, Character c)
            {
                if (w.Mana < Cost)
                    return false;
                w.Mana -= Cost;
                w.Armor += Armor;
                _turns = Duration;
                return true;
            }
        }

        #endregion

        #region Implementation of IEnumerable
        public IEnumerator<Spell> GetEnumerator()
        {
            return _spellList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
    #endregion

    #region Character classes

    internal class Character
    {
        public int Damage { get; set; }
        public int Armor { get; set; }
        public int HitPoints { get; set; }

        public Character(int damage, int armor, int hitpoints)
        {
            this.Damage = damage;
            this.Armor = armor;
            this.HitPoints = hitpoints;
        }
    }

    internal class Wizard : Character
    {
        public int Mana { get; set; }
        public Spells Spells { get; }
        public Wizard() : base(0, 0, 50)
        {
            this.Mana = 500;
            Spells = new Spells();
        }

        public Wizard(int damage, int armor, int hitpoints, int mana) : base(damage, armor, hitpoints)
        {
            this.Mana = mana;
            Spells = new Spells();
        }

        public List<Spells.Spell> GetCastableSpells()
        {
            return Spells.Where(s => s.Turns == 0).ToList();
        }

        public void ActivateEffects(Character c)
        {
            var spells = Spells.Where(s => s.Turns > 0);
            spells.ToList().ForEach(s => s.Effect(this, c));
        }
    }

    internal class Boss : Character
    {
        public Boss() : base(10, 0, 71)
        {
            
        }
    }

#endregion
}
