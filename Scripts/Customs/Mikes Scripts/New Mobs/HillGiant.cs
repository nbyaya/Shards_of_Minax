using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an hill giant corpse")]
    public class HillGiant : BaseCreature
    {
        
    
        [Constructable]
        public HillGiant()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a Hill giant";
            this.Body = 1;
            this.BaseSoundID = 604;

            this.SetStr(450, 550);
            this.SetDex(66, 75);
            this.SetInt(46, 70);

            this.SetHits(650, 700);

            this.SetDamage(10, 25);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 30, 40);
            this.SetResistance(ResistanceType.Poison, 40, 50);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.MagicResist, 75, 80.0);
            this.SetSkill(SkillName.Tactics, 75.1, 80.0);
            this.SetSkill(SkillName.Wrestling, 90.1, 95.0);
            this.SetSkill(SkillName.Throwing, 80.0, 90.0); // Adding Throwing skill

            this.Fame = 4000;
            this.Karma = -4000;

            this.VirtualArmor = 25;

            this.PackItem(new Club());

            SetWeaponAbility(WeaponAbility.Dismount);
        }

        public HillGiant(Serial serial)
            : base(serial)
        {
        }

        private DateTime _NextBoulder;
        private int _Thrown;

        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;

            if (DateTime.UtcNow < _NextBoulder || combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            ThrowBoulder(combatant);

            _Thrown++;

            if (0.75 >= Utility.RandomDouble() && (_Thrown % 2) == 1) // 75% chance to quickly throw another bomb
                _NextBoulder = DateTime.UtcNow + TimeSpan.FromSeconds(3.0);
            else
                _NextBoulder = DateTime.UtcNow + TimeSpan.FromSeconds(5.0 + (10.0 * Utility.RandomDouble())); // 5-15 seconds
        }



public void ThrowBoulder(Mobile m)
{
    // Define the minimum and maximum range for throwing the boulder
    int minRange = 4;  // Minimum range in tiles
    int maxRange = 15; // Maximum range in tiles

    // Calculate the distance to the target
    double distance = GetDistanceToSqrt(m);

    // Check if the target is within the range
    if (distance >= minRange && distance <= maxRange)
    {
        // Trigger the attack animation for the giant
        this.Animate(AnimationType.Attack, 0); // 0 is the animation frame delay; adjust as necessary

        DoHarmful(m);

        // Skill-based hit check: Compare Hill Giant's Throwing skill with the defender's weapon skill
        double throwingSkill = this.Skills[SkillName.Throwing].Value;
        double defenderSkill = m.Skills[SkillName.Wrestling].Value; // Default to Wrestling; modify as needed based on what the player is using

        // PvP-style hit chance formula
        double baseHitChance = (throwingSkill + 20.0) / ((defenderSkill * 2) + 20.0);

        // Ensure the hit chance is within 0 to 1 (0% to 100%)
        baseHitChance = Math.Max(0.05, Math.Min(0.95, baseHitChance)); // Min 5% hit chance, max 95%

        // Moving particles for the boulder (visual effect)
        this.MovingParticles(m, Utility.RandomList(0x1363, 0x1364, 0x1365, 0x1366, 0x1368), 10, 0, false, true, 0, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0);

        // Delay to simulate travel time of the boulder
        Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
        {
            // Check if the boulder hits or misses
            bool hits = Utility.RandomDouble() <= baseHitChance;

            if (hits)
            {
                // If the boulder hits, calculate and apply damage
                m.PlaySound(0x308);

                // Calculate raw damage
                int rawDamage = Utility.RandomMinMax(15, 20);

                // Calculate damage reduction based on physical resistance
                int physicalResistance = m.PhysicalResistance;
                int reducedDamage = (int)(rawDamage * (1.0 - (physicalResistance / 100.0)));

                // Ensure damage is at least 1
                reducedDamage = Math.Max(1, reducedDamage);

                // Deal damage to the target
                AOS.Damage(m, this, reducedDamage, 100, 0, 0, 0, 0);

                // Optional: Deal AoE damage to nearby enemies
                foreach (Mobile mob in m.GetMobilesInRange(2)) // Range of 2 tiles for AoE
                {
                    if (mob != m && mob != this && CanBeHarmful(mob))
                    {
                        DoHarmful(mob);
                        int aoeDamage = Utility.RandomMinMax(20, 35);

                        // Apply AoE damage reduction
                        int mobPhysicalResistance = mob.PhysicalResistance;
                        int reducedAoEDamage = (int)(aoeDamage * (1.0 - (mobPhysicalResistance / 100.0)));

                        // Ensure AoE damage is at least 1
                        reducedAoEDamage = Math.Max(1, reducedAoEDamage);

                        AOS.Damage(mob, this, reducedAoEDamage, 100, 0, 0, 0, 0); // Less damage for nearby enemies
                    }
                }
            }
            else
            {
                // The boulder misses
                m.SendAsciiMessage("The Hill Giant's boulder flies past you!");
                m.PlaySound(0x238); // Play the miss sound effect
            }
        });
    }
    else
    {
        // Notify if the target is out of range
       // this.SendAsciiMessage("The target is not within the appropriate range for throwing a boulder.");
    }
}

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.4 < Utility.RandomDouble())
                return;

            switch (Utility.Random(3))
            {
                case 0:
                    {
                        defender.SendLocalizedMessage(1004014); // You have been stunned!
                        defender.Freeze(TimeSpan.FromSeconds(4.0));
                        break;
                    }
                case 1:
                    {
                        defender.SendAsciiMessage("The giant grabs you!");
                        defender.Freeze(TimeSpan.FromSeconds(3.0));
                        break;
                    }
                case 2:
                    {
                        AOS.Damage(defender, this, Utility.Random(10, 5), 100, 0, 0, 0, 0);
                        defender.SendAsciiMessage("You have been hit by a critical strike!");
                        break;
                    }
            }
        }

        public override bool CanRummageCorpses { get { return true; } }

        public override int TreasureMapLevel { get { return 3; } }

        public override int Meat { get { return 2; } }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.UltraRich);
            this.AddLoot(LootPack.Potions);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
