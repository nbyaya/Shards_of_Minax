using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;         // For potential spell effects
using Server.Network;        // For visual and sound effects
using System.Collections.Generic; // For AoE targeting lists

namespace Server.Mobiles
{
    [CorpseName("an obsidian worm corpse")]
    public class ObsidianWorm : BaseCreature
    {
        // Ability cooldown timers and movement tracking
        private DateTime m_NextMoltenEruptionTime;
        private DateTime m_NextObsidianRiftTime;
        private DateTime m_NextShardBarrageTime;
        private Point3D m_LastLocation;

        // Unique Obsidian Hue: choose an evocative dark hue value
        private const int UniqueHue = 1175;

        [Constructable]
        public ObsidianWorm() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "an Obsidian Worm";
            Body = 89;             // Same as GiantIceWorm
            BaseSoundID = 0xDC;      // Same as GiantIceWorm
            Hue = UniqueHue;         // Unique obsidian sheen

            // --- Stronger, Boss-Level Stats ---
            SetStr(500, 600);
            SetDex(200, 250);
            SetInt(300, 350);

            SetHits(2000, 2500);
            SetStam(300, 350);
            SetMana(800, 1000);

            // Base physical damage with a mix of fire damage
            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Fire, 30);

            // Resistances: Highly resistant to physical and fire, but vulnerable to cold
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Skills ---
            SetSkill(SkillName.EvalInt, 80.1, 90.0);
            SetSkill(SkillName.Magery, 80.1, 90.0);
            SetSkill(SkillName.MagicResist, 90.1, 100.0);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 110.1, 120.0);
            SetSkill(SkillName.Meditation, 80.0, 90.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 100;
            ControlSlots = 5; // Boss-level creature

            // Initialize special ability cooldowns (in seconds)
            m_NextMoltenEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextObsidianRiftTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            m_NextShardBarrageTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            // Initialize last location for leaving lava trails
            m_LastLocation = this.Location;

            // --- Loot Setup (adjust as desired) ---
            PackItem(new Gold(Utility.RandomMinMax(250, 350)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
        }

        public ObsidianWorm(Serial serial) : base(serial)
        {
        }

        // --- Movement: Leave a trail of HotLavaTile behind when moving ---
        public override void OnThink()
        {
            base.OnThink();

            // Leave a lava trail on old tiles with some chance
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.25)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    HotLavaTile lavaTile = new HotLavaTile();
                    lavaTile.Hue = UniqueHue;
                    lavaTile.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        HotLavaTile lavaTile = new HotLavaTile();
                        lavaTile.Hue = UniqueHue;
                        lavaTile.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            // Check to see if we have a valid combatant target before doing abilities
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Priority: Shard Barrage > Obsidian Rift > Molten Eruption based on cooldowns and distance
            if (DateTime.UtcNow >= m_NextShardBarrageTime && this.InRange(Combatant.Location, 10))
            {
                ObsidianShardBarrageAttack();
                m_NextShardBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }
            else if (DateTime.UtcNow >= m_NextObsidianRiftTime && this.InRange(Combatant.Location, 12))
            {
                ObsidianRiftAttack();
                m_NextObsidianRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
            else if (DateTime.UtcNow >= m_NextMoltenEruptionTime && this.InRange(Combatant.Location, 8))
            {
                MoltenEruptionAttack();
                m_NextMoltenEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
        }

        // --- Special Ability: Molten Eruption ---
        // An AoE burst that deals fire damage and may ignite targets
        public void MoltenEruptionAttack()
        {
            if (Map == null)
                return;

            // Visual and sound effects
            PlaySound(0x208); // Deep rumbling eruption sound
            FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet); 

            // Get targets in a 6-tile radius
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

                    // Deal significant fire damage (70% fire)
                    int damage = Utility.RandomMinMax(40, 60);
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

                    // Visual effect on target
                    target.FixedParticles(0x374A, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);

                    // Chance to “ignite” (send a message); you can tie this to a DoT effect if desired
                    if (Utility.RandomDouble() < 0.35)
                    {
                        if (target is Mobile targetMobile)
                        {
                            targetMobile.SendMessage(0x22, "Flames scorch your flesh!");
                        }
                    }
                }
            }
        }

        // --- Special Ability: Obsidian Rift ---
        // A targeted hazard attack that spawns a burst of molten lava at the enemy's location.
        public void ObsidianRiftAttack()
        {
            if (Combatant == null || Map == null)
                return;

            Point3D targetLocation;

            // Ensure we are dealing with a Mobile target before accessing its Location
            if (Combatant is Mobile targetMobile && CanBeHarmful(targetMobile, false))
            {
                targetLocation = targetMobile.Location;
            }
            else
            {
                targetLocation = Combatant.Location;
            }

            this.Say("*The earth cracks!*");
            PlaySound(0x22F); // Rift sound effect

            // Effect: A small explosion effect before the hazard emerges
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            // Delay then spawn the hazard
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (this.Map == null)
                    return;

                // Adjust spawn location if necessary
                Point3D spawnLoc = targetLocation;
                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                {
                    spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                        return;
                }

                HotLavaTile lavaTile = new HotLavaTile();
                lavaTile.Hue = UniqueHue;
                lavaTile.MoveToWorld(spawnLoc, this.Map);

                Effects.PlaySound(spawnLoc, this.Map, 0x1F6);
            });
        }

        // --- Special Ability: Obsidian Shard Barrage ---
        // A chain attack that bounces between targets with physical and fiery damage.
        public void ObsidianShardBarrageAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Feel the piercing shards!*");
            PlaySound(0x20A); // Shard impact sound

            List<Mobile> targets = new List<Mobile>();
            Mobile currentTarget = Combatant as Mobile;
            if (currentTarget == null || !CanBeHarmful(currentTarget, false) || !SpellHelper.ValidIndirectTarget(this, currentTarget))
                return;
            targets.Add(currentTarget);

            int maxTargets = 5;
            int range = 5;

            for (int i = 0; i < maxTargets; ++i)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;

                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, range);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) 
                        && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(lastTarget, m) 
                        && lastTarget.InLOS(m))
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

            // Loop through and apply damage effects for each target hit
            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                // Visual: Create an obsidian shard effect from source to target
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Delay damage slightly per bounce for visual impact
                Mobile damageTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(35, 50);
                        // Deal damage as mixed physical and fire (e.g., 70% physical, 30% fire)
                        AOS.Damage(damageTarget, this, damage, 70, 30, 0, 0, 0);
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Death Effect: Volcanic Detonation ---
        // On death, the Obsidian Worm erupts violently, spawning additional hazardous HotLavaTiles
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*Ash and flame...*");
            Effects.PlaySound(this.Location, this.Map, 0x211);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            int hazardsToDrop = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < hazardsToDrop; i++)
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

                HotLavaTile lavaTile = new HotLavaTile();
                lavaTile.Hue = UniqueHue;
                lavaTile.MoveToWorld(hazardLocation, this.Map);

                Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration),
                    0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }

            base.OnDeath(c);
        }

        // --- Standard Creature Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } } // For high-level map drop
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            // Chance for a unique magical artifact
            if (Utility.RandomDouble() < 0.02)
            {
                PackItem(new MaxxiaScroll()); // Placeholder artifact
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

        // --- Serialization ---
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reinitialize ability timers on load
            m_NextMoltenEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextObsidianRiftTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            m_NextShardBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
        }
    }
}
