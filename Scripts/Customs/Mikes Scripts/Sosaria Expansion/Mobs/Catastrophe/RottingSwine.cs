using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Targeting; // Needed for StatMod

namespace Server.Mobiles
{
    [CorpseName("a rotting swine corpse")]
    public class RottingSwine : BaseCreature
    {
        private DateTime _NextSpew; // Cooldown timer for Vile Spew

        [Constructable]
        public RottingSwine()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4) // Standard melee AI
        {
            this.Name = "a Rotting Swine";
            this.Body = 0xCB;          // Pig body
            this.BaseSoundID = 0xC4;   // Pig sound
            this.Hue = 2118;           // A sickly green/grey hue

            // Stats - Significantly stronger than a pig, leaning towards resilience and poison
            this.SetStr(300, 350);
            this.SetDex(80, 100);
            this.SetInt(50, 70);

            this.SetHits(400, 450);
            this.SetMana(0); // No mana needed for these abilities

            // Damage - Respectable melee damage
            this.SetDamage(12, 18);

            // Damage Type - Primarily physical melee
            this.SetDamageType(ResistanceType.Physical, 100);

            // Resistances - High poison and physical, moderate others
            this.SetResistance(ResistanceType.Physical, 50, 60);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 40, 50);
            this.SetResistance(ResistanceType.Poison, 70, 80); // Very high poison resist
            this.SetResistance(ResistanceType.Energy, 40, 50);

            // Skills - Strong combat skills + Poisoning
            this.SetSkill(SkillName.MagicResist, 70.0, 85.0);
            this.SetSkill(SkillName.Tactics, 80.0, 95.0);
            this.SetSkill(SkillName.Wrestling, 85.0, 100.0);
            this.SetSkill(SkillName.Poisoning, 60.0, 80.0); // Used implicitly by poison attacks

            // Fame/Karma - Infamous and evil
            this.Fame = 3500;
            this.Karma = -3500;

            // Virtual Armor - Decent protection
            this.VirtualArmor = 30;

            // Misc Properties
            this.Tamable = false; // Cannot be tamed
            // ControlSlots = X; // Not needed if not tamable

            // No default weapon needed, uses Wrestling
        }

        // --- Special Abilities ---

        // Ranged Attack Check (Similar to Hill Giant's Boulder logic)
        public override void OnActionCombat()
        {
            IDamageable combatant = Combatant; // Use IDamageable first

            // Standard checks for validity, range, LOS, cooldown
            if (DateTime.UtcNow < _NextSpew || combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 10) || !CanBeHarmful(combatant) || !InLOS(combatant))
            {
                base.OnActionCombat(); // Allow standard melee checks if ranged fails
                return;
            }

            // *** Crucial Check: Ensure combatant is a Mobile before using Mobile-specific properties/methods ***
            if (combatant is Mobile target)
            {
                // Only proceed with VileSpew if the combatant is a Mobile
                if (0.2 > Utility.RandomDouble()) // 20% chance to attempt spew if conditions met
                {
                     VileSpew(target);
                    _NextSpew = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18)); // Cooldown 10-18 seconds
                }
                else
                {
                    base.OnActionCombat(); // Proceed with standard melee if spew chance fails
                }
            }
            else
            {
                // If the combatant isn't a Mobile (e.g., a destructible object), just perform base combat actions
                base.OnActionCombat();
            }
        }

        // Vile Spew Ranged Attack
        public void VileSpew(Mobile m)
        {
            this.MovingParticles(m, 0x36D4, 7, 0, false, true, Hue -1 , 0, 9502, 4019, 0x160, EffectLayer.Waist, 0); // Greenish vomit effect
            this.PlaySound(0x108); // Sound effect (adjust if needed)
            DoHarmful(m);

            // Delay effect slightly to simulate travel time
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                if (m != null && !m.Deleted && m.Alive && InLOS(m)) // Check target validity again
                {
                    m.SendAsciiMessage("You are hit by a stream of vile, rotting bile!");

                    // Apply direct Poison damage
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 0, 0, 100, 0); // 100% Poison damage

                    // Apply a strong poison effect
                    m.ApplyPoison(this, Poison.Lethal); // Use Lethal poison level

                    // Optional: Small AoE effect at impact point
                    foreach (Mobile mob in m.GetMobilesInRange(1)) // Affect mobs within 1 tile of target
                    {
                        if (mob != m && mob != this && CanBeHarmful(mob))
                        {
                            DoHarmful(mob);
                            mob.SendAsciiMessage("You are splashed by the vile substance!");
                            // Apply a lesser poison to nearby targets
                            mob.ApplyPoison(this, Poison.Greater);
                        }
                    }
                }
            });
        }

        // --- Other Overrides ---

        public override bool CanRummageCorpses { get { return true; } } // Can scavenge corpses
        public override int Meat { get { return 0; } } // Rotting = No edible meat
        public override FoodType FavoriteFood { get { return FoodType.None; } } // Doesn't eat typical food
        public override int TreasureMapLevel { get { return 2; } } // Drops level 2 treasure maps

        // Loot Generation
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);          // Good general loot

            // Chance for a unique rare drop
            if (Utility.RandomDouble() < 0.02) // 2% chance
            {
                // Assuming you have an item named "RottingHeart" defined elsewhere
                 PackItem(new Item(Utility.RandomList(0x1CED, 0x1CEE)) { Name = "Rotting Heart", Hue = 33 }); // Example placeholder item
                 // Replace above line with: PackItem(new RottingHeart()); if you create a specific item class
            }
        }

        // --- Serialization ---
        public RottingSwine(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // Placeholder for the unique drop - Create this as a proper item if needed
    /*
    public class RottingHeart : Item
    {
        [Constructable]
        public RottingHeart() : base(0x1CED) // Example ItemID (Bloody Heart)
        {
            Name = "Rotting Heart";
            Hue = 33; // A sickly, rotten color
            Weight = 1.0;
            // Add any special properties for the item here
        }

        public RottingHeart(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
    */
}