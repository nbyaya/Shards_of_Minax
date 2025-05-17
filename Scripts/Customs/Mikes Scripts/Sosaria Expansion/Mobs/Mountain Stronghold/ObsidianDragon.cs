using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("an obsidian dragon corpse")]
    public class ObsidianDragon : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextMoltenBreathTime;
        private DateTime m_NextShardStormTime;
        private DateTime m_NextEruptionTime;
        private Point3D m_LastLocation;
        // Unique Hue for Obsidian Dragon (dark, obsidian-like color)
        private const int UniqueHue = 1175;

        [Constructable]
        public ObsidianDragon() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Obsidian Dragon";
            Body = 718;         // Inherited body from FairyDragon
            BaseSoundID = 362;  // Inherited sound from FairyDragon
            Hue = UniqueHue;

            // Significantly boosted stats for a powerful monster
            SetStr(700, 800);
            SetDex(150, 200);
            SetInt(550, 600);

            SetHits(1800, 2000);
            SetStam(300, 350);
            SetMana(800, 900);

            SetDamage(20, 25);

            // Damage types: predominantly fire with a bit of energy
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 70);
            SetDamageType(ResistanceType.Energy, 10);

            // Resistances
            SetResistance(ResistanceType.Physical, 45, 60);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Skills configuration
            SetSkill(SkillName.EvalInt, 100.1, 115.0);
            SetSkill(SkillName.Magery, 105.1, 120.0);
            SetSkill(SkillName.MagicResist, 110.2, 125.0);
            SetSkill(SkillName.Tactics, 95.1, 105.0);
            SetSkill(SkillName.Wrestling, 95.1, 105.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize ability cooldown timers (randomized within ranges)
            m_NextMoltenBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextShardStormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;

            // Initial loot (example reagents; adjust as you see fit)
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 20)));
        }

        public ObsidianDragon(Serial serial) : base(serial)
        {
        }

        // --- OnMovement: Leaves behind a scorching trail that damages nearby foes ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive && InRange(this.Location, 2))
            {
                // Check if the target is a Mobile before accessing Mobile-specific properties
                if (m is Mobile target && SpellHelper.ValidIndirectTarget(this, target))
                {
                    DoHarmful(target);
                    // Minor fire damage (100% fire) as enemies brush against the trail
                    AOS.Damage(target, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0);

                    // Drain a little stamina to simulate scorched fatigue
                    int staminaDrain = Utility.RandomMinMax(5, 10);
                    if (target.Stam >= staminaDrain)
                    {
                        target.Stam -= staminaDrain;
                        target.SendMessage(0x22, "The scorching heat saps your strength!");
                        target.FixedParticles(0x36BD, 10, 15, 5032, EffectLayer.Waist);
                        target.PlaySound(0x5B);
                    }
                }
            }

            // Leave a hazardous hot lava trail using a HotLavaTile
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    HotLavaTile hazard = new HotLavaTile();
                    hazard.Hue = UniqueHue;
                    hazard.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        HotLavaTile hazard = new HotLavaTile();
                        hazard.Hue = UniqueHue;
                        hazard.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Check and trigger abilities based on cooldowns and range

            if (DateTime.UtcNow >= m_NextMoltenBreathTime && InRange(Combatant.Location, 8))
            {
                MoltenBreathAttack();
                m_NextMoltenBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (DateTime.UtcNow >= m_NextShardStormTime && InRange(Combatant.Location, 10))
            {
                ObsidianShardStormAttack();
                m_NextShardStormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            }
            else if (DateTime.UtcNow >= m_NextEruptionTime && InRange(Combatant.Location, 12))
            {
                EruptionAttack();
                m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(22, 28));
            }
        }

        // --- Unique Ability: Molten Breath Attack ---
        // Releases a cone of fire that damages and staggers nearby targets while leaving behind hazardous lava pools.
        public void MoltenBreathAttack()
        {
            if (Map == null)
                return;

            this.Say("*Feel the scorching blaze!*");
            PlaySound(0x228); // Chosen fire-breath sound

            // Visual effect: a molten burst around the dragon
            FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x376A, 10, 60, UniqueHue, 0, 5039, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(30, 50);
                    // Apply 100% fire damage
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

                    // Burning visual effect on target
                    target.FixedParticles(0x36BD, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);

                    // Chance to inflict additional stamina drain (burn effect)
                    if (Utility.RandomDouble() < 0.30)
                    {
                        if (target is Mobile targetMobile)
                        {
                            int extraDrain = Utility.RandomMinMax(10, 20);
                            if (targetMobile.Stam >= extraDrain)
                            {
                                targetMobile.Stam -= extraDrain;
                                targetMobile.SendMessage(0x22, "The burning flame leaves you weakened!");
                            }
                        }
                    }

                    // Drop a hazardous HotLavaTile at the target's location
                    if (Map != null)
                    {
                        Point3D loc = target.Location;
                        if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        {
                            HotLavaTile tile = new HotLavaTile();
                            tile.Hue = UniqueHue;
                            tile.MoveToWorld(loc, Map);
                        }
                    }
                }
            }
        }

        // --- Unique Ability: Obsidian Shard Storm ---
        // Launches a bouncing barrage of sharp, obsidian shards that chain to nearby foes.
        public void ObsidianShardStormAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Obsidian shards, rain down!*");
            PlaySound(0x20D); // Sound effect for shard storm

            List<Mobile> targets = new List<Mobile>();
            Mobile currentTarget = Combatant as Mobile;
            if (currentTarget == null || !CanBeHarmful(currentTarget, false) || !SpellHelper.ValidIndirectTarget(this, currentTarget))
                return;

            targets.Add(currentTarget);
            int maxBounces = 5;
            int bounceRange = 5;

            for (int i = 0; i < maxBounces; ++i)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;

                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, bounceRange);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false)
                        && SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
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
                    targets.Add(nextTarget);
                else
                    break;
            }

            // Process the bouncing damage along the chain
            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                // Visual effect: a shard moving from the previous target to the next
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x1B73, // Chosen graphic for obsidian shards
                    7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Mobile damageTarget = target; // Capture target for delayed damage
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(25, 40);
                        // Deal 100% fire damage with the shard
                        AOS.Damage(damageTarget, this, damage, 0, 100, 0, 0, 0);
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Unique Ability: Eruption Attack ---
        // Causes a violent eruption around a target location and spawns hazardous flames (using FlamestrikeHazardTile).
        public void EruptionAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*The earth trembles as magma erupts!*");
            PlaySound(0x228); // Eruption sound effect

            // Determine target location from Combatant—ensure we check for Mobile casting
            IDamageable targetDamageable = Combatant;
            Point3D targetLocation;
            if (targetDamageable is Mobile targetMobile && CanBeHarmful(targetMobile, false))
                targetLocation = targetMobile.Location;
            else
                targetLocation = targetDamageable.Location;

            // Particle explosion at the target location
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration),
                0x376A, 10, 10, UniqueHue, 0, 5039, 0);

            // Spawn several hazard tiles using FlamestrikeHazardTile
            int hazardCount = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < hazardCount; i++)
            {
                int xOffset = Utility.RandomMinMax(-4, 4);
                int yOffset = Utility.RandomMinMax(-4, 4);
                Point3D hazardLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                {
                    hazardLocation.Z = Map.GetAverageZ(hazardLocation.X, hazardLocation.Y);
                    if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                        continue;
                }

                FlamestrikeHazardTile hazardTile = new FlamestrikeHazardTile();
                hazardTile.Hue = UniqueHue;
                hazardTile.MoveToWorld(hazardLocation, this.Map);

                Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration),
                    0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }
        }

        // --- Death Effect: Volcanic Cataclysm ---
        // On death, the Obsidian Dragon unleashes a massive explosion and spawns additional hazard tiles.
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*Ashes... and embers...*");
            PlaySound(0x228); // Eruption sound effect

            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            // Spawn a number of hazard tiles (using HotLavaTile in this example)
            int hazardsToSpawn = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < hazardsToSpawn; i++)
            {
                int xOffset = Utility.RandomMinMax(-4, 4);
                int yOffset = Utility.RandomMinMax(-4, 4);
                Point3D hazardLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                {
                    hazardLocation.Z = Map.GetAverageZ(hazardLocation.X, hazardLocation.Y);
                    if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                        continue;
                }

                HotLavaTile tile = new HotLavaTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(hazardLocation, this.Map);

                Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration),
                    0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }

            base.OnDeath(c);
        }

        // --- Standard Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            if (Utility.RandomDouble() < 0.02)
            {
                PackItem(new MaxxiaScroll());
            }
            if (Utility.RandomDouble() < 0.05)
            {
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 3)));
            }
            if (Utility.RandomDouble() < 0.10)
            {
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 2)));
            }
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reinitialize timers when loading
            m_NextMoltenBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextShardStormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
