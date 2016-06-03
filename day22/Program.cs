using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day22
{
    class Program
    {
        static Random rand = new Random();
        static void Main(string[] args)
        {
            var easy = SearchOptimal();
            Console.WriteLine("========================================\nENTERING HARD MODE\n========================================");
            var hard = SearchOptimal(true);
            Console.WriteLine("\n\n========================================\nLowest cost in easy mode is " + easy);
            Console.WriteLine("Lowest cost in hard mode is " + hard + "\n========================================");
            Console.Write("Are these solutions correct? (y/n) ");
            if (!((char) Console.Read()).Equals('y'))
            {
                easy = SearchOptimal(deep:true);
                Console.WriteLine("========================================\nENTERING HARD MODE\n========================================");
                hard = SearchOptimal(true, true);
                Console.WriteLine("\n\n========================================\nLowest cost in easy mode is " + easy);
                Console.WriteLine("Lowest cost in hard mode is " + hard + "\n========================================");
            }
        }
        
        private static int SearchOptimal(bool hardmode = false, bool deep = false)
        {
            // Algorithmic stats
            // Our backwards path
            var path = new List<Spells.Spell>(150);
            // The number of the spell we had for each step
            var spellCount = new List<int>();
            // A number of tries we had for each step
            var tries = new List<int>();
            // Current depth
            var depth = 0;
            const int maxDepth = 15;

            // Game stats
            var win = false;
            var lowestCost = int.MaxValue;
            var availableSpells = new List<Spells.Spell>();
            availableSpells = Fight(new Wizard(), new Boss(), path, out win, hardmode);
            while (true)
            {
                // We will not have to go deeper than 15 actually
                // The game can be theoretically infinitely long, so we have to set a hard cap
                if (depth == maxDepth)
                {
                    depth--;
                    spellCount[depth]++;
                    path.RemoveAt(depth);
                    availableSpells = Fight(new Wizard(), new Boss(), path, out win, hardmode);
                    continue;
                }

                var currentCost = path.Sum(s => s.Cost);

                // If we enter this depth for the first time, add a count of 0 for this depth
                if (spellCount.Count <= depth)
                    spellCount.Add(0);

                //// Get the list of spells we can cast
                //availableSpells = Fight(new Wizard(), new Boss(), path, out win, hardmode);

                // If we tried all possible spells at this point or are at a dead end, go back a node
                if (spellCount[depth] == availableSpells.Count || !availableSpells.Any())
                {
                    spellCount[depth] = 0;
                    depth--;
                    if (depth == -1)
                        return lowestCost;
                    path.RemoveAt(path.Count - 1);
                    availableSpells = Fight(new Wizard(), new Boss(), path, out win, hardmode);
                    continue;
                }
                if (spellCount.Count > 0 && depth == 0)
                {
                    Console.WriteLine("Entering section {0}", spellCount[0] + 1);
                    if (!hardmode && spellCount[0] == 2 && !deep)
                    {
                        return lowestCost;
                    }
                    if (hardmode && spellCount[0] == 3 && !deep)
                    {
                        return lowestCost;
                    }
                }

                // Choose the next spell
                while (true)
                {
                    if (spellCount[depth] >= availableSpells.Count)
                    {
                        // If we tried all spells and none worked, go back a node
                        spellCount[depth] = 0;
                        depth--;
                        path.RemoveAt(path.Count - 1);
                        availableSpells = Fight(new Wizard(), new Boss(), path, out win, hardmode);
                        break;
                    }
                    // Select always the most expensive spell, since it's got more value for money
                    var spell = availableSpells
                        .OrderByDescending(s => s.Cost)
                        .Skip(spellCount[depth])
                        .First();
                    // Add it to our current node
                    if (path.Count > depth)
                        path[depth] = spell;
                    else
                        path.Add(spell);

                    // If the path we create is too expensive, try next spell in list
                    if (path.Sum(s => s.Cost) > lowestCost)
                    {
                        spellCount[depth]++;
                        path.RemoveAt(path.Count - 1);
                        continue;
                    }
                    var nextSpells = Fight(new Wizard(), new Boss(), path, out win, hardmode);
                    // If the path actually wins, try next spell in list
                    if (win)
                    {
                        var cost = path.Sum(s => s.Cost);
                        if (cost < lowestCost)
                        {
                            lowestCost = cost;
                            Console.WriteLine("Found new lowest cost: " + cost);
                        }
                        spellCount[depth]++;
                        path.RemoveAt(path.Count - 1);
                        continue;
                    }
                    // If player loses, try next
                    if (!nextSpells.Any())
                    {
                        spellCount[depth]++;
                        path.RemoveAt(path.Count - 1);
                        continue;
                    }
                    // If the path neither loses nor wins, we have to look into it
                    spellCount[depth]++;
                    depth++;
                    availableSpells = nextSpells;
                    break;
                }
            }
        }
        private static List<Spells.Spell> Fight(Wizard player, Character boss, List<Spells.Spell> spellQueue, out bool win, bool hardmode = false)
        {
            win = false;
            var spellN = 0;
            while (true)
            {
                // Player turn

                // Hard mode. Player loses one HP 
                if (hardmode)
                {
                    player.HitPoints--;
                    if (player.HitPoints == 0) return new List<Spells.Spell>();
                }
                // All effects that still have a counter will now be activated
                player.ActivateEffects(boss);
                // If boss is dead, return a win and empty list
                if (boss.HitPoints <= 0)
                {
                    win = true;
                    return new List<Spells.Spell>();
                }

                // If there are no spells in our queue, return a list of available spells
                //if (!spellQueue.Any()) return player.GetCastableSpells();
                if (spellN == spellQueue.Count) return player.GetCastableSpells();

                // Cast next spell
                var spell = spellQueue[spellN];
                spellN++;
                player.Spells.First(s => s.GetType() == spell.GetType()).Cast(player, boss);
                //spell.Cast(player, boss);

                // Boss turn
                // All effects that still have a counter will now be activated
                player.ActivateEffects(boss);
                // If boss is dead, return a win and empty list
                if (boss.HitPoints <= 0)
                {
                    win = true;
                    return new List<Spells.Spell>();
                }
                // Boss attacks
                player.HitPoints -= Math.Max(1, boss.Damage - player.Armor);
                // Check if player is still alive
                if (player.HitPoints <= 0) return new List<Spells.Spell>();
            }
        }
    }

    #region Spells
    internal class Spells : IEnumerable<Spells.Spell>
    {
        #region Instantiation of all Spells
        public MagicMissileSpell MagicMissile { get; } = new MagicMissileSpell();
        public DrainSpell Drain { get; } = new DrainSpell();
        public PoisonSpell Poison { get; } = new PoisonSpell();
        public ShieldSpell Shield { get; } = new ShieldSpell();
        public RechargeSpell Recharge { get; } = new RechargeSpell();
        private readonly List<Spell> _spellList;
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

            // This function applies the effect of an active spell
            public bool Effect(Wizard w, Character c)
            {
                if (Turns <= 0)
                    return false;
                // Apply effect
                c.HitPoints -= Damage;
                w.Mana += Recover;
                w.HitPoints += Heal;
                _turns--;
                // When turns reaches 0, armor boost runs out
                // This works, because our only spell with armor boost has 6 turns and thus it runs out in the player's turn
                // If it lasted for an uneven number of turns, we'd have to move this to a new function and explicitly call it
                if (Turns == 0)
                    w.Armor -= Armor;
                return true;
            }
            #endregion
        }

        internal class MagicMissileSpell : Spell
        {
            public MagicMissileSpell() : base(cost: 53, damage: 4) { }
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
            public RechargeSpell() : base(cost: 229, recover: 101) { }

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
            Spells.Where(s => s.Turns > 0).ToList().ForEach(s => s.Effect(this, c));
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
