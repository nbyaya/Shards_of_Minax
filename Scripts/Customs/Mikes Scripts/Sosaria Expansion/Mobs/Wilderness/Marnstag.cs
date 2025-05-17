using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells; // Needed for SpellHelper or similar utility functions if used

namespace Server.Mobiles
{
    [CorpseName("a marnstag corpse")]
    public class Marnstag : BaseCreature
    {
        // Unique Hue for Marnstag (Example: A deep forest green or a mystic blue)
        private const int UniqueHue = 0x8A1; // Example hue, choose one that fits your vision

        [Constructable]
        public Marnstag()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Marnstag";
            Body = 0xEA; // Uses the Great Hart body
            Hue = UniqueHue; // Apply the unique hue

            // Enhanced Stats (Significantly higher than Great Hart, comparable to or exceeding Hill Giant in some aspects)
            SetStr(300, 400);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(800, 1000);
            SetMana(50, 100); // Marnstag could have some mana for potential future abilities
            SetStam(250, 300);

            SetDamage(15, 25); // Higher base damage

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold, 40); // Adds Cold damage type

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 25, 35);
            SetResistance(ResistanceType.Cold, 50, 60); // Stronger Cold Resistance
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 95.0, 105.0);
            SetSkill(SkillName.Wrestling, 95.0, 105.0);
            SetSkill(SkillName.Anatomy, 75.0, 85.0); // Added Anatomy for potential special attacks
            SetSkill(SkillName.Healing, 60.0, 70.0); // Potential for self-healing or regeneration

            Fame = 10000; // High fame
            Karma = -10000; // Negative karma

            VirtualArmor = 50; // Increased armor

            // No tamable
            Tamable = false;
            ControlSlots = 5; // Requires more control slots if somehow tamed
            MinTameSkill = 120.0; // Very high tame skill requirement

            // Add some starting loot (optional)
             PackGold(200, 300);
             PackItem(new SpinedLeather(Utility.RandomMinMax(5, 10)));
             PackItem(new Bone(Utility.RandomMinMax(1, 3)));


            // Set a special weapon ability (optional)
            // SetWeaponAbility(WeaponAbility.BleedAttack);
        }

        public Marnstag(Serial serial)
            : base(serial)
        {
        }

        // Use Great Hart sounds
        public override int GetAttackSound()
        {
            return 0x82;
        }

        public override int GetHurtSound()
        {
            return 0x83;
        }

        public override int GetDeathSound()
        {
            return 0x84;
        }

        // --- Unique Abilities ---

        // AOE Windup Attack: 'Arctic Blast'
        private DateTime _NextArcticBlast;
        private TimeSpan ArcticBlastCooldown = TimeSpan.FromSeconds(15.0); // Cooldown for the ability
        private TimeSpan ArcticBlastWindup = TimeSpan.FromSeconds(3.0); // Windup time before the attack

        public void ArcticBlast()
        {
            // Check if the ability is on cooldown
            if (DateTime.UtcNow < _NextArcticBlast)
                return;

            // Check for valid targets in range before starting windup
            bool foundTarget = false;
            foreach (Mobile m in GetMobilesInRange(8)) // Check a range of 8 tiles for targets
            {
                if (m != this && CanBeHarmful(m))
                {
                    foundTarget = true;
                    break;
                }
            }

            if (!foundTarget)
                return; // No valid targets, don't use the ability

            // Announce the windup
            Say("A chilling wind begins to gather!");
            Animate(10, 5, 1, true, false, 0); // Example windup animation (adjust as needed)
            PlaySound(0x10); // Example windup sound (adjust as needed)


            // Start the windup timer
            Timer.DelayCall(ArcticBlastWindup, new TimerCallback(DoArcticBlast));

            // Set the next possible time for the ability
            _NextArcticBlast = DateTime.UtcNow + ArcticBlastCooldown;
        }

        public void DoArcticBlast()
        {
            // This method is called after the windup
            PlaySound(0x56C); // Example blast sound (adjust as needed)


            // Define the AOE range
            int aoeRange = 6;

            // Get all mobiles in the AOE range
            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in GetMobilesInRange(aoeRange))
            {
                // Check if the target is valid (not self, can be harmed)
                if (m != this && CanBeHarmful(m))
                {
                    targets.Add(m);
                }
            }

            // Apply damage and effects to targets
            foreach (Mobile target in targets)
            {
                DoHarmful(target); // Declare harmful action

                // Calculate damage (adjust damage range as needed)
                int rawDamage = Utility.RandomMinMax(30, 50);

                // Apply Cold damage
                AOS.Damage(target, this, rawDamage, 0, 0, 40, 0, 60); // 40% Cold, 60% Energy? (Adjust damage types as desired)

                // Optional: Add a freezing or slowing effect
                 if (target is Mobile mobTarget) // Using the specified check
                 {
                     if (Utility.RandomDouble() < 0.3) // 30% chance to apply a slow/freeze
                     {
                         mobTarget.SendLocalizedMessage(1004014); // You have been stunned! (Can change message)
                         mobTarget.Freeze(TimeSpan.FromSeconds(Utility.RandomMinMax(2, 4))); // Freeze for 2-4 seconds
                         mobTarget.PlaySound(0x204); // Example freeze sound
                     }
                 }

                // Visual effect on the target
                 target.FixedParticles(0x374A, 10, 15, 5013, 0x480, 0, EffectLayer.Waist); // Example icy particles
            }
        }

        // Melee abilities in OnGaveMeleeAttack
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // Ensure defender is a Mobile before accessing Mobile-specific properties
            if (defender is Mobile target)
            {
                // 30% chance for a special melee effect
                if (0.3 > Utility.RandomDouble())
                {
                    switch (Utility.Random(3))
                    {
                        case 0: // Stamina Drain
                            {
                                int drain = Utility.RandomMinMax(10, 20);
                                target.Stam -= drain;
                                target.SendAsciiMessage("The Marnstag's attack saps your stamina!");
                                PlaySound(0x23E); // Example drain sound
                                break;
                            }
                        case 1: // Frostbite (Damage over time)
                            {
                                target.SendAsciiMessage("You are hit by a chilling frostbite!");
                                new FrostbiteTimer(target, this).Start(); // Start a damage over time timer
                                PlaySound(0x20); // Example frost sound
                                break;
                            }
                        case 2: // Crippling Blow (Reduces target's damage)
                            {
                                target.SendAsciiMessage("The Marnstag's blow cripples you!");
                                // Implement a temporary damage reduction effect on the target
                                // This would require a custom buff or timed effect
                                // Example: target.AddBuff(new CripplingBlowBuff(TimeSpan.FromSeconds(5)));
                                PlaySound(0x22); // Example impact sound
                                break;
                            }
                    }
                }
            }
        }

        // Example of a damage over time timer
        private class FrostbiteTimer : Timer
        {
            private Mobile m_Target;
            private Mobile m_Source;
            private int m_Ticks;

            public FrostbiteTimer(Mobile target, Mobile source)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), 5) // 5 ticks, 1 second apart
            {
                m_Target = target;
                m_Source = source;
                m_Ticks = 0;
            }

            protected override void OnTick()
            {
                if (m_Target == null || m_Target.Deleted || !m_Target.Alive || m_Source == null || m_Source.Deleted || !m_Source.Alive)
                {
                    Stop();
                    return;
                }

                // Apply damage each tick
                int damage = Utility.RandomMinMax(3, 7);
                AOS.Damage(m_Target, m_Source, damage, 0, 0, 100, 0, 0); // Pure Cold damage for frostbite
                m_Target.FixedParticles(0x374A, 1, 20, 9502, 0x480, 0, EffectLayer.Waist); // Icy particles


                m_Ticks++;

                if (m_Ticks >= 5)
                {
                    Stop();
                    m_Target.SendAsciiMessage("The frostbite wears off.");
                }
            }
        }


        // Override OnThink to potentially trigger abilities
        public override void OnThink()
        {
            base.OnThink();

            // Trigger Arctic Blast if ready and a combatant exists
            if (Combatant != null && DateTime.UtcNow >= _NextArcticBlast)
            {
                // Ensure combatant is a Mobile before checking range and LOS
                 if (Combatant is Mobile combatantMobile)
                 {
                     if (InRange(combatantMobile, 8) && InLOS(combatantMobile))
                     {
                         ArcticBlast();
                     }
                 }
            }

            // Add other on-think behaviors here (e.g., healing, summoning)
            // Example: Chance to self-heal if low on health
            // if (Hits < HitsMax * 0.5 && !IsHealing && Utility.RandomDouble() < 0.05)
            // {
            //     IsHealing = true; // Custom flag to prevent spam healing
            //     Say("The Marnstag regenerates!");
            //     Timer.DelayCall(TimeSpan.FromSeconds(5.0), () => { Heal(Utility.RandomMinMax(20, 40)); IsHealing = false; });
            // }
        }


        // Add unique loot
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2); // More valuable standard loot
            AddLoot(LootPack.HighScrolls); // Chance for high-level scrolls

            // Custom rare loot
            if (Utility.RandomDouble() < 0.05) // 5% chance for a rare hide
            {
            }
            if (Utility.RandomDouble() < 0.01) // 1% chance for a powerful artifact
            {
                 PackItem(new WanderingFlameDo()); // Example: A custom artifact item
            }
        }

        // --- Serialization ---
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}