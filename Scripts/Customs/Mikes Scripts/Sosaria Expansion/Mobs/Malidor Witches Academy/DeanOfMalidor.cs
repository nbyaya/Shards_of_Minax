using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for spell effects potentially
using Server.Network; // Needed for effects
using Server.Spells.Necromancy; // For potential Necro effects
using Server.Spells.Fifth; // For Mind Blast basis
using Server.Spells.Seventh; // For Flame Strike basis

namespace Server.Mobiles
{
    [CorpseName("the Dean's corrupted remains")] // More thematic corpse name
    public class DeanOfMalidor : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextRiftPulseTime;
        private DateTime m_NextLessonTime;
        private DateTime m_NextSummonTime;
        private DateTime m_NextExpulsionTime;

        // Unique Hue - Example: 1170 is a deep, sickly purple/magenta, fitting corrupted magic.
        private const int UniqueHue = 1170;

        // Store summoned experiments
        private List<Mobile> m_SummonedExperiments;

        [Constructable]
        public DeanOfMalidor() : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.15, 0.3) // Slightly faster reaction
        {
            Name = "Dean of Malidor";
            Body = 79; // Lich Lord Body
            BaseSoundID = 412; // Lich Lord Sound
            Hue = UniqueHue;

            // --- Significantly Boosted Stats - Necro/Mage Focused ---
            SetStr(550, 650);      // Stronger than base Lich Lord
            SetDex(180, 220);      // Decent casting speed/agility
            SetInt(800, 950);      // Very High Intelligence - Core stat

            SetHits(2800, 3500);   // Significantly higher HP for boss status
            SetStam(180, 220);      // Good stamina pool
            SetMana(1500, 2000);   // Very Large mana pool for abilities

            SetDamage(18, 24);      // Moderate physical damage, abilities are main threat

            // Mixed damage types reflecting chaotic/necrotic magic
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 40);
            SetDamageType(ResistanceType.Energy, 40); // Arcane/Rift Energy

            // --- Adjusted Resistances - Strong vs Magic/Necro ---
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 40, 50);   // Standard Fire Resist
            SetResistance(ResistanceType.Cold, 70, 80);   // Strong Cold Resist (Lich base)
            SetResistance(ResistanceType.Poison, 70, 80); // Strong Poison Resist (Necro theme)
            SetResistance(ResistanceType.Energy, 65, 75); // Strong Energy Resist

            // --- Enhanced Skills - Focus on Necro/Mage ---
            SetSkill(SkillName.Necromancy, 115.0, 125.0); // Master Necromancer
            SetSkill(SkillName.SpiritSpeak, 115.0, 125.0); // Master Spirit Speaker

            SetSkill(SkillName.EvalInt, 110.1, 120.0);
            SetSkill(SkillName.Magery, 110.1, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 135.0); // Very high resist skill
            SetSkill(SkillName.Meditation, 110.0, 120.0); // Excellent mana regen

            SetSkill(SkillName.Tactics, 80.1, 95.0);
            SetSkill(SkillName.Wrestling, 90.1, 105.0); // Competent melee defense

            Fame = 24000; // Very High Fame
            Karma = -24000; // Extremely Evil

            VirtualArmor = 75; // High passive defense
            ControlSlots = 5; // Boss-level creature

            // Initialize ability cooldowns with random variance
            m_NextRiftPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextLessonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_NextExpulsionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));

            m_SummonedExperiments = new List<Mobile>();

            // Loot: Necro reagents, high-end regs, potential unique drops
            PackNecroReg(20, 50); // Generous Necro Regs
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new Nightshade(Utility.RandomMinMax(15, 20)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(15, 20))); // Add Mandrake
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));

            // Pack a Gnarled Staff like the Lich Lord, but maybe a better one sometimes?
            if (0.1 > Utility.RandomDouble())
                PackItem( new BoneHarvester() ); // 10% chance for Bone Harvester
            else
                PackItem(new GnarledStaff());
        }

        // --- Unique Ability: Aura of Failed Lessons ---
        // Constant Drain on nearby casters trying to cast spells. Minor damage.
        public override void OnThink() // Check aura logic more frequently in OnThink
        {
            base.OnThink();

            // --- Aura Effect ---
            if (this.Alive && this.Map != null && this.Map != Map.Internal)
            {
                List<Mobile> targets = new List<Mobile>();
                IPooledEnumerable eable = this.Map.GetMobilesInRange(this.Location, 4); // Aura Radius 4

                foreach (Mobile m in eable)
                {
                    // Target players who are alive, not the Dean, and potentially casting
                    if (m != null && m != this && m.Player && m.Alive && CanBeHarmful(m, false) && m.Spell != null)
                    {
                        targets.Add(m);
                    }
                }
                eable.Free();

                foreach (Mobile target in targets)
                {
                    if (Utility.RandomDouble() < 0.10) // 10% chance per tick while casting in aura
                    {
                        // Ensure the target is a Mobile before accessing Mobile properties
                        if (target is Mobile targetMobile && SpellHelper.ValidIndirectTarget(this, targetMobile))
                        {
                            DoHarmful(targetMobile);

                            // Mana Drain Effect
                            int manaDrained = Utility.RandomMinMax(15, 25);
                            if (targetMobile.Mana >= manaDrained)
                            {
                                targetMobile.Mana -= manaDrained;
                                targetMobile.SendMessage(0x22, "The Dean's presence disrupts your focus!");
                                targetMobile.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head); // Purple drain effect
                                targetMobile.PlaySound(0x1F8); // Mana drain sound
                            }

                            // Minor Cold Damage (fitting Lich base)
                            AOS.Damage(targetMobile, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0); // 100% Cold
                        }
                    }
                }
            }


            // --- Ability Checks ---
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Clean up dead summons from list
            m_SummonedExperiments.RemoveAll(m => m == null || !m.Alive || m.Deleted);

            // Prioritize summoning if below limit
            if (DateTime.UtcNow >= m_NextSummonTime && m_SummonedExperiments.Count < 3) // Max 3 experiments
            {
                SummonErrantExperiment();
                m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45)); // Longer cooldown after summoning
            }
            // Use abilities based on cooldown and target proximity
            else if (DateTime.UtcNow >= m_NextLessonTime && this.InRange(Combatant.Location, 10))
            {
                PainfulLessonAttack();
                m_NextLessonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextExpulsionTime && this.InRange(Combatant.Location, 12))
            {
                ExpulsionBoltAttack();
                 m_NextExpulsionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            }
             else if (DateTime.UtcNow >= m_NextRiftPulseTime && this.InRange(Combatant.Location, 8))
            {
                ChaoticRiftPulseAttack();
                m_NextRiftPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
        }


        // --- Unique Ability 1: Chaotic Rift Pulse (AoE Energy Burst + Random Teleport) ---
        public void ChaoticRiftPulseAttack()
        {
            if (Map == null) return;

            this.Say("*The veil thins!*");
            this.PlaySound(0x209); // Wisp sound? Or 0x5C3 (Unholy Bone)
            this.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet); // Large purple explosion effect on self

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
                // Visual effect outward
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x37CC, 10, 40, UniqueHue, 0, 5029, 0); // Gate travel style effect

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(40, 60); // Significant AoE damage
                    // Deal 100% energy damage (rift energy)
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                    // Visual effect on the target
                    target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head); // Purple hit effect

                    // Chance for random short-range teleport
                    if (Utility.RandomDouble() < 0.30) // 30% chance
                    {
                        // Ensure target is Mobile before using Mobile-specific methods
                        if (target is Mobile targetMobile)
                        {
                            Point3D originalLocation = targetMobile.Location;
                            Map map = targetMobile.Map;

                            if (map != null)
                            {
                                // Try to find a random spot nearby
                                for (int i = 0; i < 10; ++i) // Try 10 times
                                {
                                    int x = targetMobile.X + Utility.RandomMinMax(-3, 3);
                                    int y = targetMobile.Y + Utility.RandomMinMax(-3, 3);
                                    int z = targetMobile.Z;

                                    if (map.CanFit(x, y, z, 16, false, false))
                                    {
                                        if (map.GetAverageZ(x, y) == z) // Check if it's likely ground level
                                        {
                                             targetMobile.Location = new Point3D(x, y, z);
                                             targetMobile.ProcessDelta();
                                             targetMobile.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist); // Teleport particle
                                             targetMobile.PlaySound(0x1FE); // Teleport sound
                                             targetMobile.SendMessage(0x35, "You are violently shunted through space!");
                                             break; // Stop trying once teleported
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        // --- Unique Ability 2: Painful Lesson (Targeted Debuff AoE Hazard - Mana Drain Tile) ---
        public void PainfulLessonAttack()
        {
            if (Combatant == null || Map == null) return;

            IDamageable targetDamageable = Combatant;
            Point3D targetLocation;
            Mobile primaryTargetMobile = null;

             // Check if combatant is a mobile and can be harmed
            if(targetDamageable is Mobile targetMobile && CanBeHarmful(targetMobile, false))
            {
                 primaryTargetMobile = targetMobile;
                 targetLocation = targetMobile.Location;
            }
            else // If not mobile or cannot be harmed, target its location
            {
                 targetLocation = targetDamageable.Location;
            }

            this.Say("*Attend this lesson in suffering!*");
            PlaySound(0x482); // Necro Laugh

             // Effect: Dark energy converges on the target location
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 20, UniqueHue, 0, 5039, 0); // Dark swirling particles

            // Create the hazardous tile(s) after a brief delay
            Timer.DelayCall(TimeSpan.FromSeconds(0.75), () =>
            {
                if (this.Map == null) return; // Re-check map validity

                // Drop 1-3 tiles around the target location
                int tileCount = Utility.RandomMinMax(1, 3);
                for(int i = 0; i < tileCount; ++i)
                {
                     Point3D spawnLoc = targetLocation;
                     if(i > 0) // Offset subsequent tiles slightly
                     {
                         spawnLoc.X += Utility.RandomMinMax(-1, 1);
                         spawnLoc.Y += Utility.RandomMinMax(-1, 1);
                     }

                    // Check if the tile can be placed, adjust Z if necessary
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                    {
                        spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                        if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                             continue; // Skip if we can't find a valid spot
                    }

                    ManaDrainTile drainTile = new ManaDrainTile(); // Use ManaDrainTile
                    drainTile.Hue = UniqueHue; // Match Dean's hue
                    drainTile.MoveToWorld(spawnLoc, this.Map);

                    // Play sound at tile location
                    Effects.PlaySound(spawnLoc, this.Map, 0x1F6); // Magic trap sound? Or 0x204 (Pain Spike)
                }

                // Apply a direct Necromancy debuff to the primary target (if it was a mobile)
                if (primaryTargetMobile != null && primaryTargetMobile.Alive && Utility.RandomDouble() < 0.6) // 60% chance to apply curse
                {
                    if (SpellHelper.ValidIndirectTarget(this, primaryTargetMobile))
                    {
                        DoHarmful(primaryTargetMobile);
                        primaryTargetMobile.FixedParticles(0x374A, 10, 15, 5002, EffectLayer.Head); // Curse effect
                        primaryTargetMobile.SendMessage(0x22, "The Dean inflicts a debilitating curse upon you!");
                    }
                }
            });
        }

        // --- Unique Ability 3: Summon Errant Experiment ---
        // Summons a weaker, unstable creature based on Academy themes
        public void SummonErrantExperiment()
        {
            if (Map == null || Map == Map.Internal) return;

            this.Say("*Another failure... repurposed!*");
            PlaySound(0x216); // Summon Daemon sound

            // Select a random experiment type to summon
            Mobile experiment = null;
            switch(Utility.Random(3))
            {
                case 0: experiment = new Spectre(); break;        // Failed spiritual binding
                case 1: experiment = new Imp(); break;            // Rogue summoned familiar
                case 2: experiment = new Balron(); break;      // Flesh experiment gone wrong (if available, else maybe Gore Fiend)
            }

            if (experiment != null)
            {
                Point3D spawnLoc = this.Location;
                 // Try to find a valid spot near the Dean
                bool summoned = false;
                for (int i = 0; i < 10; ++i)
                {
                    int x = this.X + Utility.RandomMinMax(-2, 2);
                    int y = this.Y + Utility.RandomMinMax(-2, 2);
                    int z = this.Z;

                    if (Map.CanFit(x, y, z, 16, false, false))
                    {
                        spawnLoc = new Point3D(x, y, z);
                        summoned = true;
                        break;
                    }
                     else
                    {
                        z = Map.GetAverageZ(x,y);
                        if (Map.CanFit(x, y, z, 16, false, false))
                        {
                            spawnLoc = new Point3D(x,y,z);
                            summoned = true;
                            break;
                        }
                    }
                }

                 if(summoned)
                 {
                     // Basic setup for the summoned creature
                    BaseCreature bc = experiment as BaseCreature;
                    if (bc != null)
                    {
                        bc.Team = this.Team;
                        bc.Summoned = true;
                        bc.SummonMaster = this;
                        // Optional: Slightly boost its stats or give it a unique hue?
                        // bc.Hue = UniqueHue;
                    }

                    experiment.MoveToWorld(spawnLoc, this.Map);
                    experiment.PlaySound(experiment.GetIdleSound()); // Announce arrival
                    Effects.SendLocationParticles( EffectItem.Create( spawnLoc, experiment.Map, EffectItem.DefaultDuration ), 0x3728, 1, 13, UniqueHue, 0, 9907, 0 ); // Summoning particles

                    if(Combatant != null && experiment is BaseCreature summonedBC)
                    {
                         summonedBC.Combatant = Combatant; // Set it to attack the Dean's target
                    }
                     m_SummonedExperiments.Add(experiment); // Track the summon
                 }
                 else
                 {
                     experiment.Delete(); // Delete if couldn't place
                 }
            }
        }


        // --- Unique Ability 4: Expulsion Bolt (High Damage Single Target + Knockback) ---
        public void ExpulsionBoltAttack()
        {
            if (Combatant == null || Map == null) return;

            IDamageable targetDamageable = Combatant;

            // Check if combatant is a mobile and can be harmed
            if (targetDamageable is Mobile targetMobile && CanBeHarmful(targetMobile, false) && SpellHelper.ValidIndirectTarget(this, targetMobile))
            {
                 DoHarmful(targetMobile);

                this.Say("*BEGONE!*");
                this.MovingEffect(targetMobile, 0x36E4, 5, 0, false, false, UniqueHue, 0); // Like Flame Strike bolt, but purple
                this.PlaySound(0x20A); // Energy Bolt sound

                // High single-target damage (mix of Cold/Energy)
                int damage = Utility.RandomMinMax(75, 100);
                AOS.Damage(targetMobile, this, damage, 0, 50, 0, 0, 50); // 50% Cold, 50% Energy

                 // Visual effect on impact
                 targetMobile.FixedParticles( 0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.LeftFoot ); // Impact explosion

                // Knockback effect
                if (Utility.RandomDouble() < 0.50) // 50% chance of knockback
                {
                    Point3D loc = targetMobile.Location;
                    Point3D frente = this.Location;
                    int dx = loc.X - frente.X;
                    int dy = loc.Y - frente.Y;
                    double distance = Math.Sqrt(dx*dx + dy*dy);

                    if (distance != 0)
                    {
                         int pushDist = Utility.RandomMinMax(2, 4); // Push back 2-4 tiles
                         // Normalize direction
                         double scaling = pushDist / distance;
                         int newX = (int)(frente.X + (dx * scaling));
                         int newY = (int)(frente.Y + (dy * scaling));

                         Point3D newLocation = new Point3D(newX, newY, targetMobile.Z);

                         // Check if the new location is valid before moving
                         if (Map.CanFit(newLocation.X, newLocation.Y, newLocation.Z, 16, false, false))
                         {
                              if(Map.GetAverageZ(newLocation.X, newLocation.Y) == newLocation.Z) // Basic ground check
                              {
                                  targetMobile.Location = newLocation;
                                  targetMobile.ProcessDelta();
                                  targetMobile.SendMessage(0x35, "You are violently thrown backwards!");
                                  targetMobile.PlaySound(0x11D); // Thump sound
                              }
                         }
                    }
                }
            }
            else if(targetDamageable != null) // If target isn't mobile or cant be harmed, just do effect at location
            {
                 this.Say("*BEGONE!*");
                 Effects.SendLocationEffect(targetDamageable.Location, targetDamageable.Map, 0x36E4, 20, 5, UniqueHue, 0); // Effect at location
                 this.PlaySound(0x20A); // Energy Bolt sound
            }
        }


        // --- Death Effect: Final Examination ---
        // Large explosion, leaves hazardous draining fields
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*Class... Dismissed... Forever...*");
            // Large central explosion effect
            Effects.PlaySound(this.Location, this.Map, 0x207); // Chain lightning sound? Or 0x211 explosion
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 80, UniqueHue, 0, 5052, 0); // Very large central explosion

            // Spawn damaging/draining hazards around the corpse
            int hazardsToDrop = Utility.RandomMinMax(5, 8);
            for (int i = 0; i < hazardsToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-5, 5);
                int yOffset = Utility.RandomMinMax(-5, 5);
                Point3D hazardLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                // Try to find valid ground Z coordinate
                if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                {
                    hazardLocation.Z = Map.GetAverageZ(hazardLocation.X, hazardLocation.Y);
                    if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                        continue; // Skip if we can't find a valid spot
                }

                // Use ManaDrainTile for the hazard effect
                ManaDrainTile drainTile = new ManaDrainTile();
                drainTile.Hue = UniqueHue; // Match Dean's hue
                drainTile.MoveToWorld(hazardLocation, this.Map);

                // Smaller visual effect at each hazard location
                Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration), 0x37CC, 10, 20, UniqueHue, 0, 5039, 0);
            }

            // Kill remaining summoned experiments
            foreach (Mobile m in new List<Mobile>(m_SummonedExperiments)) // Iterate copy
            {
                if (m != null && m.Alive)
                {
                    Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, UniqueHue, 0, 5049, 0 ); // Fizzle effect
                    m.Kill();
                }
            }
            m_SummonedExperiments.Clear();


            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool AlwaysMurderer { get { return true; } } // Always counts as a murderer
        public override TribeType Tribe { get { return TribeType.Undead; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } } // Immune to poison
        public override int TreasureMapLevel { get { return 5; } } // High level map drop

        // Increased Dispel difficulty
        public override double DispelDifficulty { get { return 135.0; } }
        public override double DispelFocus { get { return 60.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2); // Very good base loot
            AddLoot(LootPack.FilthyRich);   // More good loot
            AddLoot(LootPack.MedScrolls, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(2, 3)); // Good chance for 7th/8th scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15)); // Lots of gems

            // Unique / Rare Drops Themed to Malidor
            if (Utility.RandomDouble() < 0.25) // 25% chance
            {
                 // Placeholder: A common unique ingredient/lore item
                 PackItem( new DarkTallow( Utility.RandomMinMax(1, 3)) ); // Example: Dark Tallow
            }

            if (Utility.RandomDouble() < 0.05) // 5% chance
            {
                // Placeholder: A rarer unique item - e.g., Dean's Signet Ring, Corrupted Tome Fragment
                // Example: Use a known rare item temporarily
                PackItem( new DaemonBone(Utility.RandomMinMax(5,10)) );
            }

             if (Utility.RandomDouble() < 0.01) // 1% chance
            {
                // Placeholder: Very rare unique artifact - e.g., Malidor's Fractured Grimoire, Dean's Scepter of Corruption
                 // Example: Use a known artifact temporarily
                PackItem( new ArcaneShield() ); // Example Placeholder
            }
        }

        // --- Serialization ---
        public DeanOfMalidor(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            // Save summoned creature list (serials)
            writer.WriteMobileList(m_SummonedExperiments, true);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Load summoned creature list
            m_SummonedExperiments = reader.ReadStrongMobileList();
            if (m_SummonedExperiments == null) // Ensure list exists
                m_SummonedExperiments = new List<Mobile>();


            // Re-initialize timers on load/restart to prevent immediate ability spam
            m_NextRiftPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextLessonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextExpulsionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));

             // Clean up any invalid summons loaded
            Timer.DelayCall(TimeSpan.Zero, () => m_SummonedExperiments.RemoveAll(m => m == null || m.Deleted));
        }
    }

     // Example placeholder for a "rare resource" drop if needed later
    public class DarkTallow : Item
    {
        [Constructable]
        public DarkTallow() : this(1) { }

        [Constructable]
        public DarkTallow(int amount) : base(0x1BFB) // Candle geometry
        {
            Name = "Dark Tallow";
            Hue = 1109; // Dark Grey/Black
            Stackable = true;
            Amount = amount;
            Weight = 0.1;
        }

        public DarkTallow(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }
}