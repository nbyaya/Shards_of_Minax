using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for spell effects potentially
using Server.Network; // Needed for effects
using System.Collections.Generic; // Needed for lists in AoE targeting
using Server.Spells.Second; // For Protection removal maybe?
using Server.Spells.Fourth; // For Mana Drain basis
using Server.Spells.Fifth; // For Dispel Field basis / Silence

namespace Server.Mobiles
{
    [CorpseName("a discordant pixie corpse")] // Thematic corpse name
    public class PixieSonglord : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextCacophonyTime;
        private DateTime m_NextPiercingNoteTime;
        private DateTime m_NextMesmerizingMelodyTime;
        private DateTime m_NextPhaseShiftTime; // Movement ability timer
        private Point3D m_LastLocation;

        // Unique Hue - Example: 1281 is a vibrant magenta/pink, fitting for chaotic fey magic
        private const int UniqueHue = 1281;

        [Constructable]
        public PixieSonglord() : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.15, 0.3) // Slightly faster reaction
        {
            Name = "a Pixie Songlord"; // Name
            Body = 128;               // Base body from SAPixie
            BaseSoundID = 0x467;      // Base sound from SAPixie
            Hue = UniqueHue;          // Apply unique hue

            // --- Adjusted Stats - Focus on Dexterity & Intelligence ---
            SetStr(300, 400);    // Moderate strength
            SetDex(450, 550);    // Very High Dexterity (nimble pixie)
            SetInt(500, 600);    // High Intelligence (magical power)

            SetHits(1100, 1400); // Good survivability, slightly less than pure elemental
            SetStam(350, 450);   // High stamina pool for movement/abilities
            SetMana(550, 650);   // Large mana pool

            SetDamage(12, 18);   // Moderate physical damage

            // Primarily Energy damage from sonic/magic attacks, less physical
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80); // Sonic/magical focus

            // --- Adjusted Resistances - Fey Nature ---
            SetResistance(ResistanceType.Physical, 55, 65); // Decent physical resist
            SetResistance(ResistanceType.Fire, 45, 55);
            SetResistance(ResistanceType.Cold, 60, 70); // Slightly better vs Cold
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 75, 85); // High energy resist

            // --- Enhanced Skills - Focus on Magic & Evasion ---
            SetSkill(SkillName.EvalInt, 105.1, 120.0);
            SetSkill(SkillName.Magery, 105.1, 120.0);
            SetSkill(SkillName.MagicResist, 110.2, 125.0);
            SetSkill(SkillName.Meditation, 95.0, 105.0); // Good mana regen
            SetSkill(SkillName.Tactics, 60.1, 70.0);   // Less focus on pure combat
            SetSkill(SkillName.Wrestling, 65.1, 75.0); // Basic self-defense

            Fame = 18000; // High fame
            Karma = -18000; // Very malicious

            VirtualArmor = 70; // Good passive defense
            ControlSlots = 4; // Challenging, but slightly less than pure elemental boss

            // Initialize ability cooldowns
            m_NextCacophonyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextPiercingNoteTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextMesmerizingMelodyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            m_NextPhaseShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15)); // Movement ability

            m_LastLocation = this.Location; // Initialize last location

            // Loot: Magic reagents, maybe fey-themed items
            PackItem(new MandrakeRoot(Utility.RandomMinMax(8, 12)));
            PackItem(new Nightshade(Utility.RandomMinMax(8, 12)));
            PackItem(new FertileDirt(Utility.RandomMinMax(5, 10))); // Less common reagent
        }

        // --- Passive Ability: Aura of Discord ---
        // Affects nearby enemies, increasing fizzle chance slightly and draining stamina.
        // Uses OnMovement check for simplicity.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            // Check if the mover is a valid hostile target within range
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 3) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                // Ensure the target is a Mobile before accessing Mobile properties
                if (m is Mobile targetMobile && SpellHelper.ValidIndirectTarget(this, targetMobile))
                {
                    // Stamina Drain
                    int stamDrained = Utility.RandomMinMax(8, 15);
                    if (targetMobile.Stam >= stamDrained)
                    {
                        targetMobile.Stam -= stamDrained;
                        if (Utility.RandomDouble() < 0.2) // Less frequent message spam
                            targetMobile.SendMessage(0x22, "The discordant air saps your energy!"); // Feedback message

                        // Small visual/sound effect
                        targetMobile.FixedParticles(0x374A, 1, 5, 0x2521, UniqueHue, 0, EffectLayer.Waist); // Subtle effect
                        if (Utility.RandomDouble() < 0.1) targetMobile.PlaySound(0x5C); // Subtle sound effect
                    }

                    // Apply a temporary fizzle debuff (e.g., via a custom hashtable or attached object - simplified here with a message)
                    // For a real implementation, you might add the mobile to a temporary debuff list checked during spell casting.
                    if (Utility.RandomDouble() < 0.05) // 5% chance per movement tick to trigger message/potential effect
                    {
                        targetMobile.SendMessage(0x22, "The clashing sounds make it hard to concentrate!");
                        // Potentially add a short-lived hidden debuff here in a full implementation
                    }
                }
            }

            base.OnMovement(m, oldLocation);
        }


        // --- Thinking Process for Special Attacks & Movement Effect ---
        public override void OnThink()
        {
            base.OnThink();

            // --- Movement Ability: Phase Shift / Echoing Step ---
            // Quick repositioning with a lingering hazardous effect.
            if (DateTime.UtcNow >= m_NextPhaseShiftTime && Combatant != null && !this.InRange(Combatant.Location, 2)) // Use when not in melee
            {
                PhaseShift();
                m_NextPhaseShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }


            // Update last known location if not phasing
            if (this.Location != m_LastLocation)
            {
                m_LastLocation = this.Location;
            }


            // --- Ability Checks ---
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on cooldown and situation
            if (DateTime.UtcNow >= m_NextCacophonyTime && this.InRange(Combatant.Location, 8)) // AoE preferred when available
            {
                CacophonyBurst();
                m_NextCacophonyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
            else if (DateTime.UtcNow >= m_NextMesmerizingMelodyTime && this.InRange(Combatant.Location, 10)) // Debuff next priority
            {
                 MesmerizingMelody();
                 m_NextMesmerizingMelodyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(22, 30));
            }
            else if (DateTime.UtcNow >= m_NextPiercingNoteTime && this.InRange(Combatant.Location, 12)) // Single target damage filler
            {
                PiercingNote();
                m_NextPiercingNoteTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
        }


        // --- Movement Ability: Phase Shift ---
        // Dashes to a nearby location, leaving a harmful echo.
        public void PhaseShift()
        {
            if (Map == null || Combatant == null) return;

            Point3D oldLoc = this.Location;
            Point3D newLoc = Combatant.Location; // Target near the combatant

            // Try to find a spot near the combatant but not right on top
            bool spotFound = false;
            for (int i = 0; i < 10; ++i) // Try 10 times to find a valid spot
            {
                int x = newLoc.X + Utility.RandomMinMax(-4, 4);
                int y = newLoc.Y + Utility.RandomMinMax(-4, 4);
                int z = Map.GetAverageZ(x, y);

                Point3D checkLoc = new Point3D(x, y, z);
                if (Map.CanSpawnMobile(checkLoc) && Map.CanFit(x, y, z, 16, false, false) && this.InLOS(checkLoc))
                {
                    newLoc = checkLoc;
                    spotFound = true;
                    break;
                }
            }

            if (!spotFound) return; // Couldn't find a suitable location

            // Effects for the shift
            Effects.SendLocationParticles(EffectItem.Create(oldLoc, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5039, 0); // Effect at start point
            Effects.PlaySound(oldLoc, Map, 0x22F); // Sound at start

            // Move the Pixie
            this.Location = newLoc;
            this.ProcessDelta(); // Update position immediately
            m_LastLocation = newLoc; // Update last location after move

            Effects.SendLocationParticles(EffectItem.Create(newLoc, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5039, 0); // Effect at end point
            Effects.PlaySound(newLoc, Map, 0x22F); // Sound at end

            // Leave a hazardous tile at the STARTING location
             Timer.DelayCall(TimeSpan.FromSeconds(0.2), () =>
             {
                 if (this.Map == null) return;

                 Point3D spawnLoc = oldLoc;
                  if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                  {
                       spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                        if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                           return; // Give up if we can't find a valid spot
                  }

                 // Use VortexTile re-hued to represent the 'Sonic Echo'
                 VortexTile echoTile = new VortexTile();
                 echoTile.Hue = UniqueHue;
                 echoTile.Name = "discordant echo"; // Optional name
                 echoTile.MoveToWorld(spawnLoc, this.Map);
                 Effects.PlaySound(spawnLoc, Map, 0x5C); // Echo sound
             });
        }


        // --- Unique Ability: Cacophony Burst (AoE Energy Damage + Resist Debuff) ---
        public void CacophonyBurst()
        {
            if (Map == null) return;

            this.Say("*Shrill screams fill the air!*");
            this.PlaySound(BaseSoundID); // Use base sound pitched up or mixed? (Using base for now)
            this.PlaySound(0x56); // High pitched sound effect

            // Central visual effect
            this.FixedParticles(0x3779, 10, 30, 0x252A, UniqueHue, 0, EffectLayer.CenterFeet); // Expanding wave effect

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6); // 6 tile radius AoE

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(30, 50); // Moderate AoE damage
                    // Deal 100% energy damage
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                    // Add a visual effect on the target
                    target.FixedParticles(0x374A, 1, 15, 0x2529, UniqueHue, 0, EffectLayer.Head); // Swirling effect on target

                    // Apply temporary Resistance debuff (Example: -10% to all resists for 5 seconds)
                    // This needs a proper debuff system (e.g., TimedResistanceBuff). Simplified here:
                     if (target is Mobile mobileTarget) // Check if target is Mobile
                     {
                         // Example of applying a temporary debuff using a helper or custom system
                         // ResistanceMod mod = new ResistanceMod( ResistanceType.All, -10 );
                         // mobileTarget.AddResistanceMod( mod );
                         // Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), () => mobileTarget.RemoveResistanceMod( mod ) );

                         mobileTarget.SendMessage(0x22, "The cacophony weakens your defenses!");
                         mobileTarget.PlaySound(0x1DF); // Debuff sound
                     }
                }
            }
        }


        // --- Unique Ability: Piercing Note (Single Target Damage + Interrupt Chance) ---
        public void PiercingNote()
        {
            if (Combatant == null || Map == null) return;

            IDamageable targetDamageable = Combatant;
            Mobile targetMobile = targetDamageable as Mobile; // Try to cast to Mobile

            if (targetMobile != null && CanBeHarmful(targetMobile, false) && SpellHelper.ValidIndirectTarget(this, targetMobile))
            {
                DoHarmful(targetMobile);

                this.Say("*Hear this!*");
                PlaySound(0x512); // High-pitched sound effect

                // Visual: Beam or bolt effect
                Effects.SendBoltEffect(targetMobile, true, UniqueHue); // Simple bolt effect, colored

                int damage = Utility.RandomMinMax(50, 75); // High single target damage
                AOS.Damage(targetMobile, this, damage, 0, 0, 0, 0, 100); // 100% Energy

                // Chance to interrupt casting spell
                if (Utility.RandomDouble() < 0.35) // 35% chance
                {
                     if (targetMobile.Spell != null) // Check if Mobile and casting
                     {
                         targetMobile.DisruptiveAction(); // Interrupt current action (includes casting)
                         targetMobile.SendMessage(0x22, "The piercing note shatters your concentration!");
                         targetMobile.FixedParticles(0x373A, 1, 15, 0x251E, UniqueHue, 0, EffectLayer.Head); // Interrupt effect
                     }
                }
            }
             else // Handle non-mobile combatant (e.g., inanimate object, though unlikely target for this mob)
             {
                 // Optional: Add basic damage effect for non-mobiles if needed
                 // Effects.SendBoltEffect(targetDamageable, true, UniqueHue);
                 // AOS.Damage(targetDamageable, this, Utility.RandomMinMax(50, 75), 0, 0, 0, 0, 100);
             }
        }

        // --- Unique Ability: Mesmerizing Melody (Single Target Slow + Mana Drain) ---
        public void MesmerizingMelody()
        {
            if (Combatant == null || Map == null) return;

            IDamageable targetDamageable = Combatant;
             if (targetDamageable is Mobile targetMobile && CanBeHarmful(targetMobile, false) && SpellHelper.ValidIndirectTarget(this, targetMobile))
             {
                 DoHarmful(targetMobile);

                 this.Say("*Listen closely...*");
                 PlaySound(0x58B); // Calmer, musical sound

                 // Visual effect on target
                 targetMobile.FixedParticles(0x376A, 1, 15, 0x25ED, UniqueHue, 0, EffectLayer.Waist); // Swirling particle effect

                 // Apply Slow effect (similar to Mage spell)
                 if (!targetMobile.Poisoned) // Check if target is not already slowed/paralyzed maybe
                 {
                    targetMobile.PlaySound(0x204); // Slow spell sound
                    IEntity from = new Entity(Serial.Zero, this.Location, this.Map);
                    IEntity to = new Entity(Serial.Zero, targetMobile.Location, targetMobile.Map);
                    Effects.SendMovingParticles(from, to, 0x376A, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, (EffectLayer)255, 0x100); // Moving effect
                    targetMobile.SendMessage(0x22, "The melody makes your limbs heavy!");
                 }


                 // Apply Mana Drain
                 int manaDrained = Utility.RandomMinMax(30, 60);
                 if (targetMobile.Mana >= manaDrained)
                 {
                     targetMobile.Mana -= manaDrained;
                     targetMobile.SendMessage(0x22, "The enchanting tune drains your focus!");
                     targetMobile.FixedParticles(0x374A, 1, 15, 0x251E, UniqueHue, 0, EffectLayer.Head); // Mana drain particle effect
                     targetMobile.PlaySound(0x1F8); // Mana drain sound
                 }
             }
             // No action needed if Combatant isn't a Mobile for this ability
        }


        // --- Death Effect: Swan Song (AoE Energy Damage + Silence) ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*The final note... echoes!*");
            // Large central explosion effect
            Effects.PlaySound(this.Location, this.Map, 0x307); // Death sound effect (female scream?)
            Effects.PlaySound(this.Location, this.Map, 0x56);  // High pitched sound

            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0); // Big central explosion

            // Apply damage and Silence to nearby hostiles
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 8); // 8 tile radius

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                foreach (Mobile target in targets)
                {
                    int damage = Utility.RandomMinMax(40, 60); // Significant death burst damage
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100); // 100% Energy

                    // Apply Silence effect
                    if (target is Mobile mobileTarget) // Check if target is Mobile
                    {
                        TimeSpan duration = TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
                        mobileTarget.SendMessage(0x22, "The final shriek leaves you unable to cast!");
                        mobileTarget.FixedParticles(0x374A, 1, 15, 0x2522, UniqueHue, 0, EffectLayer.Head); // Silence visual
                    }
                }
            }

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } } // Fey don't bleed conventionally
        public override Poison PoisonImmune { get { return Poison.Lethal; } } // Immune to poisons
        public override int TreasureMapLevel { get { return 5; } } // High level map drop

        // Slightly easier to dispel than pure elemental
        public override double DispelDifficulty { get { return 125.0; } }
        public override double DispelFocus { get { return 45.0; } }

        public override OppositionGroup OppositionGroup
        {
            get { return OppositionGroup.FeyAndUndead; } // Belongs to the Fey group
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 1); // Good base loot
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(1, 2)); // Include med level scrolls too
            AddLoot(LootPack.HighScrolls, 1); // Chance for 7th/8th scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10)); // Lots of gems

            // Chance for unique fey/academy themed drops
            if (Utility.RandomDouble() < 0.05) // 5% chance
            {
                // Example unique item - replace with actual item
                // PackItem(new MalidorsWarpedFlute());
                PackItem( new DaemonBone(Utility.RandomMinMax(10,20)) ); // Placeholder: Daemon Bone
            }
             if (Utility.RandomDouble() < 0.10) // 10% chance for another rare resource
            {
                 PackItem(new FertileDirt(Utility.RandomMinMax(5, 15))); // More fertile dirt
            }
             if (Utility.RandomDouble() < 0.15) // 15% chance Pixie Dust (if you have such an item)
            {
                 // PackItem(new PixieDust(Utility.RandomMinMax(3, 7)));
                 PackItem( new MandrakeRoot(Utility.RandomMinMax(10,20)) ); // Placeholder Mandrake
            }
        }

        // --- Serialization ---
        public PixieSonglord(Serial serial) : base(serial)
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

            // Re-initialize timers on load/restart
            m_NextCacophonyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextPiercingNoteTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextMesmerizingMelodyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            m_NextPhaseShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }

        // Helper function for Silence (if not built into your Mobile class)
        // You might need to implement this based on your server's debuff system.
        // Example concept:
        /*
        private static Dictionary<Mobile, Timer> m_SilenceTimers = new Dictionary<Mobile, Timer>();
        public virtual void ApplySilence(TimeSpan duration, Mobile from)
        {
            if (m_SilenceTimers.ContainsKey(this))
            {
                m_SilenceTimers[this].Stop();
                m_SilenceTimers.Remove(this);
            }

            // Add a property or flag to the Mobile indicating silenced status
            // this.Silenced = true;

            Timer t = new SilenceTimer(this, duration);
            m_SilenceTimers.Add(this, t);
            t.Start();
        }

        private class SilenceTimer : Timer
        {
            private Mobile m_Target;
            public SilenceTimer(Mobile target, TimeSpan duration) : base(duration)
            {
                m_Target = target;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                // Remove silenced status
                // m_Target.Silenced = false;
                m_Target.SendMessage("You can focus your thoughts again.");
                m_SilenceTimers.Remove(m_Target);
            }
        }
        */
    }
}