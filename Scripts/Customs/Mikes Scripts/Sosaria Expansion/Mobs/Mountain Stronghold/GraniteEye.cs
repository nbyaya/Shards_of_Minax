using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // For potential spell effects
using Server.Network; // For visual/sound effects
using System.Collections.Generic; // For lists used in AoE targeting

namespace Server.Mobiles
{
    [CorpseName("a shattered granite corpse")]
    public class GraniteEye : BaseCreature
    {
        // Timers for unique ability cooldowns
        private DateTime m_NextPetrifyTime;
        private DateTime m_NextSeismicPulseTime;
        private DateTime m_NextStonyBarrageTime;
        private Point3D m_LastLocation;

        // Unique Hue (a distinctive stone-gray color)
        private const int UniqueHue = 1175;

        [Constructable]
        public GraniteEye() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Granite Eye";
            Body = 22;           // Based on the Elder Gazer’s body
            BaseSoundID = 377;   // Same base sound as Elder Gazer
            Hue = UniqueHue;

            // --- Advanced Stats ---
            SetStr(500, 600);
            SetDex(150, 200);
            SetInt(500, 600);

            SetHits(2200, 2500);
            SetStam(200, 250);
            SetMana(800, 900);

            SetDamage(20, 30);

            // Mainly physical damage with some magical (energy) bonus.
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Energy, 30);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 60, 70);

            // --- Skill Set ---
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5; // Boss-level creature

            // --- Ability cooldown timers ---
            m_NextPetrifyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextSeismicPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextStonyBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            // Basic loot (example items)
            PackItem(new IronIngot(Utility.RandomMinMax(10, 20)));
            PackItem(new Gold(Utility.RandomMinMax(500, 700)));

            m_LastLocation = this.Location;
        }

        // --- Unique Ability: Petrifying Gaze Attack ---
        // Fixes the target with a deadly gaze, dealing heavy physical damage and potentially paralyzing them.
        public void PetrifyingGazeAttack()
        {
            if (Combatant == null || Map == null)
                return;

            // Ensure target is a Mobile before accessing specific properties
            IDamageable targetDamageable = Combatant;
            if (targetDamageable is Mobile target && CanBeHarmful(target, false))
            {
                this.Say("*The Granite Eye fixes its gaze upon you!*");
                PlaySound(0x223); // Unique sound for petrification

                // Visual particle effect on target’s head
                target.FixedParticles(0x3709, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);

                int damage = Utility.RandomMinMax(40, 60);
                // Deal 100% physical damage
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);

                // 30% chance to "petrify" (simulate with temporary paralysis)
                if (Utility.RandomDouble() < 0.3)
                {
                    target.SendMessage(0x22, "You feel your body turning to stone!");
                    if (!target.Paralyzed)
                    {
                        target.Paralyzed = true;
                        // Unparalyze after 3 seconds using a timer
                        Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
                        {
                            if (target != null && target.Alive)
                                target.Paralyzed = false;
                        });
                    }
                }
            }
        }

        // --- Unique Ability: Seismic Pulse Attack ---
        // Sends a shock wave to the target's location and spawns an EarthquakeTile.
        public void SeismicPulseAttack()
        {
            if (Combatant == null || Map == null)
                return;

            IDamageable targetDamageable = Combatant;
            Point3D targetLocation;
            if (targetDamageable is Mobile target && CanBeHarmful(target, false))
                targetLocation = target.Location;
            else
                targetLocation = targetDamageable.Location;

            this.Say("*The ground trembles beneath your feet!*");
            PlaySound(0x20F); // Seismic pulse sound
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 20, UniqueHue, 0, 5032, 0);

            // Brief delay then spawn a hazard at the target location
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (this.Map == null)
                    return;

                Point3D spawnLoc = targetLocation;
                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                {
                    spawnLoc = new Point3D(spawnLoc.X, spawnLoc.Y, Map.GetAverageZ(spawnLoc.X, spawnLoc.Y));
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                        return;
                }
                EarthquakeTile tile = new EarthquakeTile(); // A pre-defined hazard tile type
                tile.Hue = UniqueHue;
                tile.MoveToWorld(spawnLoc, this.Map);
                Effects.PlaySound(spawnLoc, this.Map, 0x1F6); // Additional tremor sound
            });
        }

        // --- Unique Ability: Stony Barrage Attack ---
        // Hurls a series of stone shards that chain between targets.
        public void StonyBarrageAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*The Granite Eye unleashes a barrage of stone shards!*");
            PlaySound(0x256); // Stone impact sound

            List<Mobile> targets = new List<Mobile>();
            Mobile initialTarget = Combatant as Mobile;
            if (initialTarget == null || !CanBeHarmful(initialTarget, false) || !SpellHelper.ValidIndirectTarget(this, initialTarget))
                return;
            targets.Add(initialTarget);

            int maxTargets = 5;
            int range = 5;

            // Find additional nearby targets to bounce the attack to.
            for (int i = 0; i < maxTargets; ++i)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;

                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, range);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false) &&
                        SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
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

            // For each target in the chain, send a visual projectile and apply damage.
            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x36D, // Stone shard graphic
                    7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Mobile damageTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(30, 50);
                        AOS.Damage(damageTarget, this, damage, 100, 0, 0, 0, 0);
                        damageTarget.FixedParticles(0x36D, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- OnMovement Override ---
        // As Granite Eye moves, it may leave behind a hazard tile (using EarthquakeTile) that damages nearby foes.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target && SpellHelper.ValidIndirectTarget(this, target))
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(5, 10);
                    AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                    target.FixedParticles(0x3709, 5, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x1F8);
                }
            }

            base.OnMovement(m, oldLocation);

            // Drop a hazard on previous location when moving to a new tile (20% chance).
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.2)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    EarthquakeTile tile = new EarthquakeTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        EarthquakeTile tile = new EarthquakeTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        // --- OnThink Override ---
        // Checks ability cooldowns and the distance to the combatant to choose which special attack to use.
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            if (DateTime.UtcNow >= m_NextStonyBarrageTime && this.InRange(Combatant.Location, 10))
            {
                StonyBarrageAttack();
                m_NextStonyBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextSeismicPulseTime && this.InRange(Combatant.Location, 12))
            {
                SeismicPulseAttack();
                m_NextSeismicPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
            }
            else if (DateTime.UtcNow >= m_NextPetrifyTime && this.InRange(Combatant.Location, 8))
            {
                PetrifyingGazeAttack();
                m_NextPetrifyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
        }

        // --- OnDeath Override ---
        // On death, Granite Eye shatters—playing a sound, showing particles, and spawning a few hazard tiles.
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The Granite Eye shatters into countless fragments!*");
                Effects.PlaySound(this.Location, this.Map, 0x220); // Shattering sound
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5032, 0);

                int hazardsToDrop = Utility.RandomMinMax(4, 7);
                for (int i = 0; i < hazardsToDrop; i++)
                {
                    int xOffset = Utility.RandomMinMax(-4, 4);
                    int yOffset = Utility.RandomMinMax(-4, 4);
                    Point3D hazardLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                    if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                    {
                        hazardLocation = new Point3D(hazardLocation.X, hazardLocation.Y, Map.GetAverageZ(hazardLocation.X, hazardLocation.Y));
                        if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                            continue;
                    }

                    EarthquakeTile drainTile = new EarthquakeTile(); // Using EarthquakeTile for the death hazard
                    drainTile.Hue = UniqueHue;
                    drainTile.MoveToWorld(hazardLocation, this.Map);
                    Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration),
                        0x3709, 10, 20, UniqueHue, 0, 5032, 0);
                }
            }
            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            if (Utility.RandomDouble() < 0.02)
                PackItem(new MaxxiaScroll());
            if (Utility.RandomDouble() < 0.05)
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 3)));
            if (Utility.RandomDouble() < 0.10)
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 2)));
        }

        // --- Serialization ---
        public GraniteEye(Serial serial) : base(serial)
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
            m_NextPetrifyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextSeismicPulseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextStonyBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }

    // --- Optional Helper: Petrify Timer ---
    // This timer releases a paralyzed Mobile after a set duration to simulate petrification fading away.
    public class PetrifyTimer : Timer
    {
        private Mobile m_Target;
        public PetrifyTimer(Mobile target) : base(TimeSpan.FromSeconds(3))
        {
            m_Target = target;
            Priority = TimerPriority.TwoFiftyMS;
        }

        protected override void OnTick()
        {
            if (m_Target != null && m_Target.Alive)
            {
                m_Target.Paralyzed = false;
                m_Target.SendMessage(0x22, "You feel the petrification fade away.");
            }
        }
    }
}
