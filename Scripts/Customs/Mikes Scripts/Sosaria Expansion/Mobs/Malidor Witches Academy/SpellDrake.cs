using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for spell effects, SpellHelper
using Server.Network; // Needed for effects
using System.Collections.Generic; // Needed for lists in AoE targeting

namespace Server.Mobiles
{
    // Using a more evocative corpse name for a magical creature
    [CorpseName("a pulsating arcane core")]
    public class SpellDrake : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextManaBurstTime;
        private DateTime m_NextArcaneNovaTime;
        private DateTime m_NextPhaseShiftTime;
        private DateTime m_NextRuneTrapTime;

        // Unique Hue - Example: 1157 is a vibrant magical blue/purple.
        private const int UniqueHue = 1157;

        [Constructable]
        public SpellDrake() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2) // Higher perception, faster reaction
        {
            Name = "a Spell Drake";
            Body = Utility.RandomList(60, 61); // Drake bodies
            BaseSoundID = 362; // Drake sound
            Hue = UniqueHue;

            // --- Significantly Boosted & Re-themed Stats ---
            SetStr(400, 450); // Less physical than Cold Drake
            SetDex(160, 190); // Good for casting/defense
            SetInt(480, 550); // Very high for magic power

            SetHits(1600, 1900); // Very high health pool
            SetStam(160, 190); // Decent stamina
            SetMana(800, 1000); // Large mana pool for abilities

            SetDamage(20, 26); // Base damage is moderate, abilities are key

            // Primarily Energy damage, representing raw magic
            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Energy, 75);

            // --- Adjusted Resistances for Arcane Nature ---
            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 15, 30); // Vulnerable to Fire
            SetResistance(ResistanceType.Cold, 45, 60);
            SetResistance(ResistanceType.Poison, 65, 80); // Resistant to toxins
            SetResistance(ResistanceType.Energy, 80, 95); // Very high energy resist

            // --- Enhanced Magical Skills ---
            SetSkill(SkillName.EvalInt, 115.1, 130.0);
            SetSkill(SkillName.Magery, 115.1, 130.0);
            SetSkill(SkillName.MagicResist, 125.2, 140.0); // Very high magic defense
            SetSkill(SkillName.Tactics, 90.1, 105.0); // Reduced physical focus
            SetSkill(SkillName.Wrestling, 90.1, 105.0); // Reduced physical focus
            SetSkill(SkillName.Meditation, 100.0, 115.0); // Mana regeneration is key
            SetSkill(SkillName.Anatomy, 70.0, 85.0); // Basic understanding

            Fame = 22000; // Higher fame/karma
            Karma = -22000;

            VirtualArmor = 85; // Higher passive defense
            Tamable = false; // Not tameable
            ControlSlots = 5; // Boss-level difficulty marker
            MinTameSkill = 120.0; // Irrelevant but set high

            // Initialize ability cooldowns staggered
            m_NextManaBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextArcaneNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextPhaseShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextRuneTrapTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));


            // Magic-themed pack items
            PackItem(new ArcaneGem(Utility.RandomMinMax(2, 4))); // Assumes ArcaneGem exists
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(5, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));
        }

        // --- Thinking Process for Special Attacks ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on cooldowns and range
            if (DateTime.UtcNow >= m_NextPhaseShiftTime && Utility.RandomDouble() < 0.15) // Chance to phase shift defensively/aggressively
            {
                PhaseShiftAttack();
                 m_NextPhaseShiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                 m_NextManaBurstTime += TimeSpan.FromSeconds(1); // Slightly delay other attacks after shifting
                 m_NextArcaneNovaTime += TimeSpan.FromSeconds(1);
            }
            else if (DateTime.UtcNow >= m_NextArcaneNovaTime && this.InRange(Combatant.Location, 8) && Utility.RandomDouble() < 0.25) // Less frequent but powerful
            {
                ArcaneNovaAttack();
                m_NextArcaneNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
            else if (DateTime.UtcNow >= m_NextManaBurstTime && this.InRange(Combatant.Location, 10) && Utility.RandomDouble() < 0.30) // More frequent mana attack
            {
                ManaBurstAttack();
                m_NextManaBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 16));
            }
            else if (DateTime.UtcNow >= m_NextRuneTrapTime && this.InRange(Combatant.Location, 12) && Utility.RandomDouble() < 0.10) // Lay traps occasionally
            {
                 LayRuneTrap();
                 m_NextRuneTrapTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
            }
        }

        // --- Unique Ability: Mana Burst ---
        // Fires bolts of energy that drain mana and deal damage.
        public void ManaBurstAttack()
        {
             if (Combatant == null || Map == null) return;

            Mobile target = Combatant as Mobile;
            if (target == null || !CanBeHarmful(target))
                 return;

            this.Say("*Feel your power unravel!*"); // Flavor text
            this.PlaySound(0x1F8); // Mana drain sound

            int bolts = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < bolts; i++)
            {
                 // Delay bolts slightly for visual effect
                 Timer.DelayCall(TimeSpan.FromMilliseconds(i * 150), () =>
                 {
                     if (!Alive || target == null || !target.Alive || Map == null) return;

                     // Send visual effect: Energy Bolt graphic
                     Effects.SendMovingParticles(this, target, 0x379F, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

                     // Use check inside the delayed call again
                     if (target is Mobile mobileTarget && CanBeHarmful(mobileTarget))
                     {
                        DoHarmful(mobileTarget);
                        int damage = Utility.RandomMinMax(20, 30); // Moderate damage per bolt
                        int manaDrain = Utility.RandomMinMax(25, 40); // Significant mana drain

                        // Deal 100% Energy damage
                        AOS.Damage(mobileTarget, this, damage, 0, 0, 0, 0, 100);

                        mobileTarget.Mana -= manaDrain;
                        if (mobileTarget.Mana < 0) mobileTarget.Mana = 0;

                        mobileTarget.FixedParticles(0x374A, 1, 15, 5032, UniqueHue, 0, EffectLayer.Waist); // Mana drain effect on target
                        mobileTarget.SendMessage("Your magical energies are violently drained!");
                     }
                 });
            }
        }


        // --- Unique Ability: Arcane Nova ---
        // A powerful explosion of raw magic energy.
        public void ArcaneNovaAttack()
        {
            if (Map == null) return;

            this.PlaySound(0x211); // Energy Bolt sound (or another suitable magic explosion)
            this.FixedParticles(0x3709, 1, 30, 9963, UniqueHue, 0, EffectLayer.Waist); // Large energy explosion effect on self

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 7); // 7 tile radius AoE

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
                // Visual effect outwards
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x376A, 1, 60, UniqueHue, 0, 9966, 0); // Energy field effect expanding

                foreach (Mobile target in targets)
                {
                    if (target is Mobile mobileTarget) // Check if target is Mobile
                    {
                        DoHarmful(mobileTarget);
                        int damage = Utility.RandomMinMax(50, 75); // High AoE damage
                        // Deal 100% energy damage
                        AOS.Damage(mobileTarget, this, damage, 0, 0, 0, 0, 100);

                        // Add a secondary effect: potential temporary stat debuff (e.g., intelligence)
                        if(Utility.RandomDouble() < 0.3) // 30% chance
                        {
                            mobileTarget.AddStatMod(new StatMod(StatType.Int, "ArcaneNovaDebuff", -Utility.RandomMinMax(10, 25), TimeSpan.FromSeconds(10)));
                            mobileTarget.FixedParticles(0x3779, 1, 15, 9963, UniqueHue, 0, EffectLayer.Head); // Debuff visual
                            mobileTarget.SendMessage("You feel mentally scrambled by the arcane energy!");
                        }
                        else
                        {
                             mobileTarget.FixedParticles(0x373A, 1, 15, 9963, UniqueHue, 0, EffectLayer.Waist); // Standard hit visual
                        }
                    }
                }
            }
        }

        // --- Unique Ability: Phase Shift ---
        // Teleports erratically, potentially leaving a damaging vortex.
        public void PhaseShiftAttack()
        {
            if (Map == null) return;

            Point3D originalLocation = this.Location;
            Point3D targetLocation = this.Location; // Default to current location

            // Try to teleport near the combatant, otherwise random nearby location
            if (Combatant != null && this.InRange(Combatant.Location, 12))
            {
                targetLocation = Combatant.Location;
            }

            int attempts = 0;
            Point3D newLocation = Point3D.Zero;

            while (attempts < 10) // Try finding a valid spot
            {
                int x = targetLocation.X + Utility.RandomMinMax(-6, 6);
                int y = targetLocation.Y + Utility.RandomMinMax(-6, 6);
                int z = Map.GetAverageZ(x, y);

                newLocation = new Point3D(x, y, z);

                // Check if the new location is valid and not too close to the original
                if (Map.CanSpawnMobile(newLocation) && !this.InRange(newLocation, 1))
                {
                    break;
                }
                newLocation = Point3D.Zero; // Reset if invalid
                attempts++;
            }

            // If we failed to find a good spot, try one last random jump
            if (newLocation == Point3D.Zero)
            {
                 newLocation = FindRandomLocationInRange(originalLocation, 4, 8, Map);
            }


            if (newLocation != Point3D.Zero && newLocation != originalLocation)
            {
                Effects.SendLocationParticles(EffectItem.Create(originalLocation, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5032, 0); // Effect at start point
                this.Location = newLocation; // Move the drake
                this.ProcessDelta(); // Update mobile's position immediately
                Effects.SendLocationParticles(EffectItem.Create(newLocation, Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5032, 0); // Effect at end point
                this.PlaySound(0x1FE); // Teleport sound

                // Leave a temporary damaging field at the OLD location
                VortexTile vortex = new VortexTile(); // Use VortexTile
                vortex.Hue = UniqueHue; // Match hue
                vortex.MoveToWorld(originalLocation, this.Map);
            }
        }

        // --- Unique Ability: Lay Rune Trap ---
        // Creates a hidden magical trap on the ground.
        public void LayRuneTrap()
        {
            if (Map == null) return;

            // Find a suitable location near the drake
            Point3D trapLocation = FindRandomLocationInRange(this.Location, 1, 3, Map);

            if (trapLocation != Point3D.Zero)
            {
                this.Say("*A hidden ward for the unwary!*");
                this.PlaySound(0x210); // Magic trap sound

                // Create a temporary, hidden trap item
                MagicRuneTrap trap = new MagicRuneTrap(this, 30, 50, UniqueHue); // Caster, MinDamage, MaxDamage, Hue
                trap.MoveToWorld(trapLocation, Map);
                Effects.SendLocationParticles(EffectItem.Create(trapLocation, Map, TimeSpan.FromSeconds(0.5)), 0x37C4, 10, 10, UniqueHue, 0, 5049, 0); // Brief rune visual effect
            }
        }

         // Define the MagicRuneTrap Item (Needs its own file or placed within the same namespace)
         public class MagicRuneTrap : Item
         {
            private Mobile m_Caster;
            private int m_MinDamage;
            private int m_MaxDamage;
            private Timer m_Timer;

            [Constructable]
            public MagicRuneTrap(Mobile caster, int minDamage, int maxDamage, int hue) : base(0x1F17) // Invisible item ID, or a subtle graphic
            {
                Visible = false; // Initially invisible
                Movable = false;
                Hue = hue;
                Name = "a hidden magical rune";

                m_Caster = caster;
                m_MinDamage = minDamage;
                m_MaxDamage = maxDamage;

                // Trap lasts for 30 seconds
                m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(30.0), new TimerCallback(OnExpire));
                m_Timer.Start();
            }

             public override bool OnMoveOver(Mobile m)
             {
                 if (m != null && m.Alive && m != m_Caster && m_Caster != null && m_Caster.CanBeHarmful(m, false))
                 {
                     Trigger(m);
                     return false; // Allow movement over, trap is triggered
                 }
                 return true; // Allow caster or non-harmful target to pass
             }

            public void Trigger(Mobile target)
            {
                if (m_Timer != null)
                    m_Timer.Stop();

                if (m_Caster == null || target == null)
                {
                     Delete();
                     return;
                }

                // Make visible briefly on trigger
                Visible = true;
                ItemID = 0x3967; // Magic circle graphic
                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () => Visible = false); // Become invisible again briefly before deleting

                Effects.PlaySound(this.Location, this.Map, 0x207); // Explosion sound
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, Hue, 0, 5049, 0); // Energy explosion visual

                 if (m_Caster.CanBeHarmful(target, false))
                 {
                     m_Caster.DoHarmful(target);
                     int damage = Utility.RandomMinMax(m_MinDamage, m_MaxDamage);
                     AOS.Damage(target, m_Caster, damage, 0, 0, 0, 0, 100); // 100% Energy damage
                     target.SendMessage("You trigger a volatile magical rune!");
                 }

                Delete(); // Delete the trap after triggering
            }

            public void OnExpire()
            {
                Delete();
            }

             public MagicRuneTrap(Serial serial) : base(serial) { }

             public override void Serialize(GenericWriter writer)
             {
                 base.Serialize(writer);
                 writer.Write((int)0); // version
                 writer.Write(m_Caster);
                 writer.Write(m_MinDamage);
                 writer.Write(m_MaxDamage);
                 // Timer will restart on deserialize if needed, or handle expiration
             }

             public override void Deserialize(GenericReader reader)
             {
                 base.Deserialize(reader);
                 int version = reader.ReadInt();
                 m_Caster = reader.ReadMobile();
                 m_MinDamage = reader.ReadInt();
                 m_MaxDamage = reader.ReadInt();

                 // Restart timer or handle expiration logic if needed
                 // For simplicity, let's just delete traps on server load/restart
                 Delete();
                 // If you want persistence:
                 // m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(30.0), new TimerCallback(OnExpire));
                 // m_Timer.Start();
             }
         }


        // --- Death Effect: Arcane Detonation ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            Effects.PlaySound(this.Location, this.Map, 0x207); // Large explosion sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0); // Big central energy explosion

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 8); // 8 tile radius for death explosion

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
                     if (target is Mobile mobileTarget) // Check if target is Mobile
                    {
                        DoHarmful(mobileTarget);
                        int damage = Utility.RandomMinMax(60, 90); // High death damage
                        int manaDrain = Utility.RandomMinMax(50, 80); // Significant mana drain on death

                        // Deal 100% energy damage
                        AOS.Damage(mobileTarget, this, damage, 0, 0, 0, 0, 100);

                        mobileTarget.Mana -= manaDrain;
                        if (mobileTarget.Mana < 0) mobileTarget.Mana = 0;

                        mobileTarget.FixedParticles(0x374A, 10, 30, 5032, UniqueHue, 0, EffectLayer.Waist); // Strong mana drain effect
                        mobileTarget.SendMessage("The drake's death unleashes a devastating wave of arcane energy!");

                        // Chance to leave a temporary ManaDrainTile under the target
                        if(Utility.RandomDouble() < 0.4)
                        {
                            ManaDrainTile tile = new ManaDrainTile();
                            tile.Hue = UniqueHue;
                            tile.MoveToWorld(mobileTarget.Location, mobileTarget.Map);
                        }
                    }
                }
            }

            base.OnDeath(c);
        }


        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } } // Magical beings don't bleed normally
        public override Poison PoisonImmune { get { return Poison.Lethal; } } // High poison immunity
        public override int TreasureMapLevel { get { return 5; } } // High-level treasure

        // Increased Dispel difficulty for a magical creature
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3); // Generous base loot
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 3)); // Good chance for high scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10)); // Lots of gems

            // Chance for a unique drop related to the Academy or magic
            if (Utility.RandomDouble() < 0.01) // 1% chance
            {
                 // PackItem(new MalidorsResearchNotes()); // Assuming this item exists
                 PackItem(new Spellbook()); // Or just a blank spellbook as a placeholder
            }
             if (Utility.RandomDouble() < 0.05) // 5% chance for another potential rare drop
            {
                 PackItem(new VoidEssence(Utility.RandomMinMax(1, 3))); // Assuming this rare resource exists
            }
        }

        // --- Serialization ---
        public SpellDrake(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            // Save cooldown timers
            writer.Write(m_NextManaBurstTime);
            writer.Write(m_NextArcaneNovaTime);
            writer.Write(m_NextPhaseShiftTime);
            writer.Write(m_NextRuneTrapTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Load cooldown timers
            m_NextManaBurstTime = reader.ReadDateTime();
            m_NextArcaneNovaTime = reader.ReadDateTime();
            m_NextPhaseShiftTime = reader.ReadDateTime();
            m_NextRuneTrapTime = reader.ReadDateTime();

            // Optional: If timers are in the past on load, reset them to avoid instant ability spam
             DateTime now = DateTime.UtcNow;
             if (m_NextManaBurstTime < now) m_NextManaBurstTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(2, 5));
             if (m_NextArcaneNovaTime < now) m_NextArcaneNovaTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
             if (m_NextPhaseShiftTime < now) m_NextPhaseShiftTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(1, 3));
             if (m_NextRuneTrapTime < now) m_NextRuneTrapTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
        }

        // Helper function to find a random valid location nearby
        private Point3D FindRandomLocationInRange(Point3D center, int minRange, int maxRange, Map map)
        {
             if (map == null || map == Map.Internal) return Point3D.Zero;

             for (int i = 0; i < 15; i++) // Try 15 times
             {
                 int x = center.X + Utility.RandomMinMax(-maxRange, maxRange);
                 int y = center.Y + Utility.RandomMinMax(-maxRange, maxRange);

                 // Ensure it's outside the minRange if minRange > 0
                 if (minRange > 0 && Math.Abs(x - center.X) < minRange && Math.Abs(y - center.Y) < minRange)
                     continue;

                 int z = map.GetAverageZ(x, y);
                 Point3D randomLocation = new Point3D(x, y, z);

                 if (map.CanSpawnMobile(randomLocation))
                 {
                     return randomLocation;
                 }
             }
             return Point3D.Zero; // Return zero if no valid spot found
         }
    }
}