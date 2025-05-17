using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // Needed for spell effects potentially
using Server.Network; // Needed for effects
using System.Collections.Generic; // Needed for lists in AoE targeting
using Server.Spells.Seventh; // Needed for Chain Lightning (basis for Barrage)

namespace Server.Mobiles
{
    [CorpseName("an elemental confluence corpse")] // More evocative corpse name
    public class SpellElemental : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextDischargeTime;
        private DateTime m_NextRiftTime;
        private DateTime m_NextBarrageTime;
        private Point3D m_LastLocation;

        // Unique Hue - Example: 1159 is a deep arcane blue/purple.
        private const int UniqueHue = 1159;

        [Constructable]
        public SpellElemental() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2) // Quick reaction
        {
            Name = "a Spell Elemental";
            Body = 162; // Base body from GreaterPoisonElemental
            BaseSoundID = 263; // Base sound from GreaterPoisonElemental
            Hue = UniqueHue;

            // --- Significantly Boosted Stats - Magic Focused ---
            SetStr(400, 500);    // Decent strength
            SetDex(250, 300);    // Agile for casting/movement
            SetInt(550, 650);    // Very High Intelligence

            SetHits(1400, 1700); // High survivability
            SetStam(250, 300);   // Good stamina pool
            SetMana(600, 750);   // Large mana pool for abilities

            SetDamage(15, 20);   // Moderate physical damage, abilities are key

            // Primarily Energy damage, low physical
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Energy, 90); // Arcane energy focus

            // --- Adjusted Resistances - Strong vs Magic ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 30, 40); // Slightly weaker vs Poison
            SetResistance(ResistanceType.Energy, 85, 95); // Very high energy resist

            // --- Enhanced Skills - Focus on Magic ---
            SetSkill(SkillName.EvalInt, 110.1, 125.0);
            SetSkill(SkillName.Magery, 110.1, 125.0);
            SetSkill(SkillName.MagicResist, 115.2, 130.0); // Very high resist skill
            SetSkill(SkillName.Meditation, 100.0, 110.0); // Good mana regen
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 20000; // High fame boss
            Karma = -20000; // Very evil

            VirtualArmor = 80; // High passive defense
            ControlSlots = 5; // Boss-level creature

            // Initialize ability cooldowns
            m_NextDischargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            // Loot: Magic reagents and potential unique drop
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(10, 15)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));

            m_LastLocation = this.Location;

        }

        // --- Unique Ability: Arcane Instability Aura ---
        // Affects those moving nearby with mana drain and minor energy damage
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                // Ensure the target is a Mobile before accessing Mobile properties
                if (m is Mobile targetMobile && SpellHelper.ValidIndirectTarget(this, targetMobile))
                {
                    DoHarmful(targetMobile);

                    // Mana Drain Effect
                    int manaDrained = Utility.RandomMinMax(10, 20);
                    if (targetMobile.Mana >= manaDrained)
                    {
                        targetMobile.Mana -= manaDrained;
                        targetMobile.SendMessage(0x22, "You feel your magical energy drained!"); // Feedback message
                        targetMobile.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head); // Mana drain particle effect
                        targetMobile.PlaySound(0x1F8); // Mana drain sound
                    }

                    // Minor Energy Damage
                    AOS.Damage(targetMobile, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 100); // 100% Energy
                }
            }

            base.OnMovement(m, oldLocation);
        }


        // --- Thinking Process for Special Attacks & Movement Effect ---
        public override void OnThink()
        {
            base.OnThink();

            // --- Movement Effect: Leave Unstable Energy ---
            // Leaves short-lived VortexTiles behind as it moves
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25) // 25% chance per move tick
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                // Check if the tile can be placed
                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    VortexTile vortex = new VortexTile(); // Use existing VortexTile
                    vortex.Hue = UniqueHue; // Match elemental's hue
                    vortex.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                     if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                     {
                        VortexTile vortex = new VortexTile();
                        vortex.Hue = UniqueHue;
                        vortex.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                     }
                }
            }
            else
            {
                 m_LastLocation = this.Location; // Update location even if no tile dropped
            }

            // --- Ability Checks ---
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on cooldown and range
            if (DateTime.UtcNow >= m_NextBarrageTime && this.InRange(Combatant.Location, 10))
            {
                ChainSpellBarrageAttack();
                m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextRiftTime && this.InRange(Combatant.Location, 12))
            {
                 UnstableRiftAttack();
                 m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
            }
            else if (DateTime.UtcNow >= m_NextDischargeTime && this.InRange(Combatant.Location, 8))
            {
                ChaoticDischargeAttack();
                m_NextDischargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
        }


        // --- Unique Ability: Chaotic Discharge (AoE Energy Burst + Mana Drain) ---
        public void ChaoticDischargeAttack()
        {
            if (Map == null) return;

            this.PlaySound(0x211); // Magic explosion sound
            this.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet); // Large arcane explosion effect on self

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
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 60, UniqueHue, 0, 5039, 0); // Magic arrow explosion style

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(35, 55); // Significant AoE damage
                    // Deal 100% energy damage
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                    // Add a visual effect on the target
                    target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head); // Arcane hit effect

                    // Chance to drain mana
                    if (Utility.RandomDouble() < 0.40) // 40% chance
                    {
                        if (target is Mobile targetMobile) // Check if it's a Mobile
                        {
                             int manaDrained = Utility.RandomMinMax(20, 40);
                            if (targetMobile.Mana >= manaDrained)
                            {
                                targetMobile.Mana -= manaDrained;
                                targetMobile.SendMessage(0x22, "The arcane discharge saps your focus!");
                                targetMobile.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head); // Mana drain particle effect
                                targetMobile.PlaySound(0x1F8); // Mana drain sound
                            }
                        }
                    }
                }
            }
        }


        // --- Unique Ability: Unstable Rift (Targeted AoE Hazard) ---
        public void UnstableRiftAttack()
        {
            if (Combatant == null || Map == null) return;

            IDamageable targetDamageable = Combatant;
            Point3D targetLocation;

            if(targetDamageable is Mobile targetMobile && CanBeHarmful(targetMobile, false))
            {
                 targetLocation = targetMobile.Location;
            }
            else // If combatant isn't a mobile or can't be harmed, target its location directly
            {
                targetLocation = targetDamageable.Location;
            }

            this.Say("*Reality tears!*"); // Flavor text
            PlaySound(0x22F); // Rift / Teleport sound effect?

            // Effect: Small explosion at the target location before rift appears
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 10, UniqueHue, 0, 5039, 0); // Mana vampire effect style

            // Create the hazardous tile
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (this.Map == null) return; // Re-check map validity

                // Check if the tile can be placed
                Point3D spawnLoc = targetLocation;
                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                {
                    spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                       return; // Give up if we can't find a valid spot
                }

                VortexTile vortex = new VortexTile(); // Use VortexTile for damaging hazard
                vortex.Hue = UniqueHue; // Match elemental's hue
                vortex.MoveToWorld(spawnLoc, this.Map);

                // Play sound at rift location
                Effects.PlaySound(spawnLoc, this.Map, 0x1F6); // Magic trap sound?
            });
        }


         // --- Unique Ability: Chain Spell Barrage ---
        public void ChainSpellBarrageAttack()
        {
            if (Combatant == null || Map == null) return;

            this.Say("*Feel the surge!*");
            PlaySound(0x20A); // Energy Bolt sound

            List<Mobile> targets = new List<Mobile>();
            Mobile currentTarget = Combatant as Mobile; // Initial target

            if (currentTarget == null || !CanBeHarmful(currentTarget, false) || !SpellHelper.ValidIndirectTarget(this, currentTarget))
                return; // Exit if initial target is invalid

            targets.Add(currentTarget);

            int maxTargets = 5; // Max number of bounces
            int range = 5; // Range for bouncing

            for (int i = 0; i < maxTargets; ++i)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;

                // Find the closest valid target not already hit
                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, range);
                foreach (Mobile m in eable)
                {
                    // Ensure it's a valid target, not self, not already hit, and within LoS
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
                    {
                        double dist = lastTarget.GetDistanceToSqrt(m);
                        if (closestDist == -1.0 || dist < closestDist)
                        {
                            closestDist = dist;
                            nextTarget = m;
                        }
                    }
                }
                eable.Free();

                if (nextTarget != null)
                {
                    targets.Add(nextTarget);
                }
                else
                {
                    break; // No more valid targets found
                }
            }

            // Damage and effects
            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile source = (i == 0) ? this : targets[i - 1]; // Damage comes from previous target (or self for first hit)
                Mobile target = targets[i];

                // Visual effect: Bolt from source to target
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map), // From point
                    new Entity(Serial.Zero, target.Location, target.Map), // To point
                    0x3818, // Energy Bolt graphic ID
                    7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Apply damage after a slight delay for the visual
                Mobile damageTarget = target; // Need to capture for the lambda
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                         DoHarmful(damageTarget);
                         int damage = Utility.RandomMinMax(25, 40); // Damage per hit
                         AOS.Damage(damageTarget, this, damage, 0, 0, 0, 0, 100); // Pure energy damage
                         damageTarget.FixedParticles(0x374A, 1, 15, 9502, EffectLayer.Waist); // Small energy impact effect
                    }
                });
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

            this.Say("*Energy... unbound!*");
            // Large central explosion effect
            Effects.PlaySound(this.Location, this.Map, 0x211); // Magic explosion sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0); // Big central explosion

            // Spawn damaging/draining hazards around the corpse
            int hazardsToDrop = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < hazardsToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-4, 4);
                int yOffset = Utility.RandomMinMax(-4, 4);
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
                drainTile.Hue = UniqueHue; // Match elemental's hue
                drainTile.MoveToWorld(hazardLocation, this.Map);

                // Smaller visual effect at each hazard location
                Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }


            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        // Keep Poison resistance normal unless specified otherwise
        // public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 6; } } // High level map drop

        // Increased Dispel difficulty
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2); // Very good base loot
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2)); // Chance for 7th/8th scrolls
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12)); // Lots of gems

            // Chance for a unique magical artifact
            if (Utility.RandomDouble() < 0.02) // 2% chance
            {
                // Example artifact - replace with an actual item defined elsewhere
                // PackItem(new MalidorsUnstableFocus()); // e.g., a necklace or spellbook
                PackItem( new MaxxiaScroll() ); // Placeholder: Pack a Power Crystal
            }
             if (Utility.RandomDouble() < 0.05) // 5% chance for another rare resource
            {
                 PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 3))); // Example rare resource
            }
             if (Utility.RandomDouble() < 0.10) // 10% chance for Void Core
            {
                 PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 2))); // Example rare resource
            }
        }

        // --- Serialization ---
        public SpellElemental(Serial serial) : base(serial)
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
            m_NextDischargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

        }
    }
}