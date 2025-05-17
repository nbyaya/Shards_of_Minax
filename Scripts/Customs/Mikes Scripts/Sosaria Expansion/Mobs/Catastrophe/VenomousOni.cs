using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Targeting; // Needed for Toxic Cloud targeting if implemented that way
using Server.Network; // Needed for particle effects

namespace Server.Mobiles
{
    [CorpseName("a venomous oni corpse")]
    public class VenomousOni : BaseCreature
    {
        private DateTime m_NextVenomSpit;
        private DateTime m_NextToxicCloud;
        private bool m_IsInFrenzy;
        private Timer m_FrenzyTimer;

        // Optional: Define a unique hue (e.g., a sickly green or toxic purple)
        private const int VenomousHue = 0x48F; // Example: A toxic green hue

        [Constructable]
        public VenomousOni()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.15, 0.3) // Faster reaction times
        {
            Name = "a Venomous Oni";
            Body = 241; // Standard Oni body
            Hue = VenomousHue; // Apply the unique hue

            // --- Stats ---
            SetStr(1000, 1200); // Significantly stronger
            SetDex(200, 250);  // More agile
            SetInt(250, 300);  // Smarter, potentially for ability logic

            SetHits(1200, 1500); // Much tougher

            // --- Damage & Resistances ---
            SetDamage(25, 35); // Higher base damage

            // Damage distribution includes significant Poison
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            // High poison resistance is key
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 90, 100); // Very high poison resist
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Skills ---
            // High combat skills
            SetSkill(SkillName.Wrestling, 110.1, 120.0);
            SetSkill(SkillName.Tactics, 110.1, 120.0);
            SetSkill(SkillName.MagicResist, 100.1, 115.0);
            SetSkill(SkillName.Anatomy, 100.1, 110.0);

            // Essential skill for the theme
            SetSkill(SkillName.Poisoning, 110.1, 120.0);

            // --- Fame & Karma ---
            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 70; // High inherent armor

            // --- Special Abilities (Not inherent spells, but custom logic) ---
            // (Implemented below in OnActionCombat and OnGaveMeleeAttack)

            // Add some appropriate thematic items to pack
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
            PackItem(new GreaterPoisonPotion());

            // Initialize ability cooldowns
            m_NextVenomSpit = DateTime.UtcNow;
            m_NextToxicCloud = DateTime.UtcNow;
        }

        // --- Standard Oni Sounds ---
        public override int GetAngerSound() { return 0x4E3; }
        public override int GetIdleSound() { return 0x4E2; }
        public override int GetAttackSound() { return 0x4E1; }
        public override int GetHurtSound() { return 0x4E4; }
        public override int GetDeathSound() { return 0x4E0; }

        // --- Properties ---
        public override bool CanRummageCorpses { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } } // Immune to poison
        public override int TreasureMapLevel { get { return 5; } } // Higher level map
        public override int Meat { get { return 4; } } // More meat

        // --- Ability Implementation ---

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            // Safety check for Combatant type
            if (Combatant is Mobile target)
            {
                // Venom Spit Ability
                if (DateTime.UtcNow >= m_NextVenomSpit && InLOS(target) && GetDistanceToSqrt(target) >= 3 && GetDistanceToSqrt(target) <= 10) // Range check
                {
                    if (CanBeHarmful(target) && 0.2 > Utility.RandomDouble()) // 20% chance to use if in range & LOS
                    {
                        DoVenomSpit(target);
                        m_NextVenomSpit = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15)); // Cooldown
                    }
                }

                // Toxic Cloud Ability
                if (DateTime.UtcNow >= m_NextToxicCloud)
                {
                    // Use more frequently if surrounded or fighting multiple opponents
                    int enemiesInRange = 0;
                    foreach (Mobile mob in GetMobilesInRange(4))
                    {
                        if (mob != this && CanBeHarmful(mob) && mob.Player) // Count player enemies
                            enemiesInRange++;
                    }

                    double chanceToUseCloud = (enemiesInRange > 1) ? 0.3 : 0.1; // Higher chance if more enemies

                    if (chanceToUseCloud > Utility.RandomDouble())
                    {
                        DoToxicCloud();
                        m_NextToxicCloud = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25)); // Cooldown
                    }
                }
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // Safety check for defender type
            if (defender is Mobile targetMobile)
            {
                // Passive Poisonous Touch - Higher chance if Frenzied
                double poisonChance = m_IsInFrenzy ? 0.6 : 0.4; // 60% chance if frenzied, 40% otherwise

                if (poisonChance > Utility.RandomDouble())
                {
                    // Apply poison based on Poisoning skill
                    Poison poison = GetPoison();
                    if (poison != null)
                    {
                        targetMobile.ApplyPoison(this, poison);
                        targetMobile.SendLocalizedMessage(1008084, true, Name); // You have been poisoned by
                        targetMobile.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist); // Poison effect
                        targetMobile.PlaySound(0x474); // Poison sound
                    }
                }

                // Possible additional melee effect (e.g., Stamina Drain)
                if (0.15 > Utility.RandomDouble()) // 15% chance
                {
                    targetMobile.SendMessage("The Oni's venomous touch saps your energy!");
                    targetMobile.Stam = Math.Max(0, targetMobile.Stam - Utility.RandomMinMax(20, 40));
                    targetMobile.FixedParticles(0x374A, 1, 15, 9909, EffectLayer.Waist); // Stam drain effect
                }
            }
        }

        // --- Helper Methods for Abilities ---

        private Poison GetPoison()
        {
            // Determine poison level based on skill
            // Example: Needs high skill for Lethal
            if (Skills.Poisoning.Value >= 100.0)
                return Poison.Lethal;
            else if (Skills.Poisoning.Value >= 80.0)
                return Poison.Deadly;
            else
                return Poison.Greater; // Minimum Greater
        }

        private void DoVenomSpit(Mobile target)
        {
            this.Say("Hsssk!"); // Sound cue
            this.Animate(AnimationType.Attack, 0); // Attack animation
            this.MovingParticles(target, 0x36D4, 7, 0, false, true, 9502, 4019, 0x169); // Greenish projectile
            this.PlaySound(0x102); // Spit sound?

            // Delay for travel time
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                if (target != null && target.Alive && GetDistanceToSqrt(target) <= 12) // Check target still valid
                {
                    DoHarmful(target);
                    target.PlaySound(0xDD); // Impact sound?

                    // Apply high level poison directly
                    Poison poisonToApply = Poison.Lethal; // Always Lethal spit? Or based on skill?
                    target.ApplyPoison(this, poisonToApply);
                    target.SendLocalizedMessage(1008084, true, "venomous spit"); // You have been poisoned by...
                    target.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist); // Poison effect
                    AOS.Damage(target, this, Utility.RandomMinMax(20, 30), 0, 0, 0, 100, 0); // Direct Poison damage
                }
            });
        }

        private void DoToxicCloud()
        {
            this.Say("*Chokes out a noxious cloud!*");
            this.Animate(AnimationType.Attack, 0); // Or a spell casting animation if preferred
            this.PlaySound(0x231); // Gas cloud sound

            // Create toxic cloud effect around the Oni
            Point3D center = this.Location;
            Map map = this.Map;
            int duration = Utility.RandomMinMax(8, 12); // Duration of the cloud in seconds
            int cloudRadius = 3;

            // Static particles for the cloud area
            for (int dx = -cloudRadius; dx <= cloudRadius; ++dx)
            {
                for (int dy = -cloudRadius; dy <= cloudRadius; ++dy)
                {
                    // Simple circular check
                    if (Math.Sqrt(dx * dx + dy * dy) <= cloudRadius)
                    {
                        Point3D point = new Point3D(center.X + dx, center.Y + dy, center.Z);
                        if (map != null && map.CanFit(point, 16, false, false))
                        {
                             // Greenish cloud particles
                             Effects.SendLocationParticles(EffectItem.Create(point, map, EffectItem.DefaultDuration),
                                 0x3709, 10, duration * 10, VenomousHue, 0); // Use the Oni's hue
                        }
                    }
                }
            }


            // Timer to apply effects periodically within the cloud
            Timer cloudTimer = new ToxicCloudTimer(this, center, map, cloudRadius, DateTime.UtcNow + TimeSpan.FromSeconds(duration));
            cloudTimer.Start();
        }

        // Timer class for the Toxic Cloud effect
        private class ToxicCloudTimer : Timer
        {
            private VenomousOni m_Owner;
            private Point3D m_Center;
            private Map m_Map;
            private int m_Radius;
            private DateTime m_EndTime;

            public ToxicCloudTimer(VenomousOni owner, Point3D center, Map map, int radius, DateTime endTime)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.5)) // Damage tick rate
            {
                m_Owner = owner;
                m_Center = center;
                m_Map = map;
                m_Radius = radius;
                m_EndTime = endTime;
                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || m_Map == null || DateTime.UtcNow > m_EndTime)
                {
                    Stop();
                    return;
                }

                // Find mobiles within the cloud radius
                List<Mobile> targets = new List<Mobile>();
                IPooledEnumerable eable = m_Map.GetMobilesInRange(m_Center, m_Radius);
                foreach(Mobile m in eable)
                {
                     if(m != m_Owner && m_Owner.CanBeHarmful(m) && m.Map == m_Map && m.Location.Z >= m_Center.Z - 5 && m.Location.Z <= m_Center.Z + 5)
                     {
                         targets.Add(m);
                     }
                }
                eable.Free();

                // Apply effects to targets
                foreach (Mobile target in targets)
                {
                    // Safety check
                    if (target is Mobile targetMobile)
                    {
                        m_Owner.DoHarmful(targetMobile);
                        // Apply poison and direct damage
                        targetMobile.ApplyPoison(m_Owner, m_Owner.GetPoison()); // Apply Oni's current poison level
                        AOS.Damage(targetMobile, m_Owner, Utility.RandomMinMax(10, 15), 0, 0, 0, 100, 0); // Poison damage
                        targetMobile.SendAsciiMessage("You choke on the toxic fumes!");
                    }
                }
            }
        }

        private void EnterFrenzy()
        {
            if (m_IsInFrenzy || Deleted || !Alive) return;

            m_IsInFrenzy = true;
            this.Say("*Enters a VENOMOUS FRENZY!*");
            this.PlaySound(GetAngerSound());

            // Visual effect for frenzy
            this.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist); // Example: Red glow

            // Apply frenzy effects (e.g., faster attack speed)
            this.ActiveSpeed = 0.1; // Adjust as needed for your shard's speed settings
            this.PassiveSpeed = 0.2;

            // Start a timer to end the frenzy after a duration
            if (m_FrenzyTimer != null) m_FrenzyTimer.Stop();
            m_FrenzyTimer = Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20)), ExitFrenzy); // Frenzy duration
        }

        private void ExitFrenzy()
        {
            m_IsInFrenzy = false;
            this.Say("*The frenzy subsides...*");

            // Reset speed
            this.ActiveSpeed = 0.15; // Back to normal active speed
            this.PassiveSpeed = 0.3; // Back to normal passive speed

             // Clear the timer reference
            m_FrenzyTimer = null;
        }

        // --- Loot ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3); // Generous standard loot
            AddLoot(LootPack.MedScrolls, 2);
            AddLoot(LootPack.HighScrolls, 1);
            AddLoot(LootPack.Gems, 5);

            // Chance for a unique rare drop
            if (Utility.RandomDouble() < 0.01) // 1% chance
            {
                // *** NOTE: You need to create this item class separately ***
                PackItem(new VenomousOniHeart());
            }

            // Chance for the base Oni mask
            if (Utility.RandomDouble() < 0.005) // Lower chance than base Oni (0.5%)
            {
                PackItem(new DemonicOniMask());
            }
        }

        // --- Serialization ---
        public VenomousOni(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            // No extra variables to serialize currently (timers are transient)
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // If frenzy state needs persistence (less common for temporary buffs):
            // m_IsInFrenzy = reader.ReadBool();
            // If m_IsInFrenzy, potentially restart the ExitFrenzy timer here
        }
    }

    // *** Placeholder for the required unique item ***
    // You MUST create this item class for the loot generation to work without errors.
    public class VenomousOniHeart : Item
    {
        [Constructable]
        public VenomousOniHeart() : base(0x1CED) // Example ItemID (Bloody Heart)
        {
            Name = "Venomous Oni Heart";
            Weight = 1.0;
            LootType = LootType.Regular; // Or Blessed/Newbied if desired
        }

        public VenomousOniHeart(Serial serial) : base(serial)
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
}