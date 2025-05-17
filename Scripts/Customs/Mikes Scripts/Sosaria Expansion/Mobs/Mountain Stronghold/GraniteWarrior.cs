using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network; // For visual and sound effects
using Server.Spells;


namespace Server.Mobiles
{
    [CorpseName("a granite warrior corpse")]
    public class GraniteWarrior : BaseCreature
    {
        // Cooldowns for special abilities
        private DateTime m_NextTremorSlamTime;
        private DateTime m_NextBoulderTossTime;
        private DateTime m_NextStoneBarrageTime;

        // Used to drop ground hazards when moving
        private Point3D m_LastLocation;

        // Unique hue for this monster (a granite-toned color)
        private const int UniqueHue = 0x480;

        [Constructable]
        public GraniteWarrior()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Granite Warrior";
            Body = 806;            // Same body as the BlackSolenWarrior example
            BaseSoundID = 959;     // Same base sound as the example
            Hue = UniqueHue;

            // Advanced stats for a boss-level creature
            SetStr(300, 350);
            SetDex(150, 180);
            SetInt(80, 100);

            SetHits(1200, 1400);
            SetDamage(20, 30);

            // 100% Physical Damage output
            SetDamageType(ResistanceType.Physical, 100);

            // Resistance values (adjust as necessary)
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Skills are geared toward melee combat
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;
            ControlSlots = 4;

            // Initialize ability timers with randomized cooldowns
            m_NextTremorSlamTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            m_NextBoulderTossTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextStoneBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 22));

            m_LastLocation = this.Location;

            // Basic loot
            PackItem(new IronIngot(Utility.RandomMinMax(1, 3)));
            PackItem(new Granite(Utility.RandomMinMax(5, 10)));
        }

        public GraniteWarrior(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            // --- Ground Hazard on Movement ---
            // As the Granite Warrior moves, drop an EarthquakeTile (hazard) behind if possible.
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.3)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;
                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    EarthquakeTile tile = new EarthquakeTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(oldLoc, this.Map);
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            // --- Ability Checks ---
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on cooldown and range
            if (DateTime.UtcNow >= m_NextStoneBarrageTime && this.InRange(Combatant.Location, 10))
            {
                ChainStoneBarrage();
                m_NextStoneBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
            else if (DateTime.UtcNow >= m_NextBoulderTossTime && this.InRange(Combatant.Location, 12))
            {
                BoulderToss();
                m_NextBoulderTossTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (DateTime.UtcNow >= m_NextTremorSlamTime && this.InRange(Combatant.Location, 3))
            {
                TremorSlam();
                m_NextTremorSlamTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
        }

        // --- Ability: Tremor Slam --- 
        // A ground pound that deals AoE physical damage to all nearby enemies.
        public void TremorSlam()
        {
            PlaySound(0x20C); // Ground-shaking sound effect
            FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.Waist);

            // Affect all mobiles in a 3-tile radius
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 3);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(30, 45);
                    AOS.Damage(m, this, damage, 100, 0, 0, 0, 0); // 100% physical damage
                    m.SendMessage("The force of the tremor slams you to the ground!");
                }
            }
            eable.Free();
        }

        // --- Ability: Boulder Toss ---
        // Throws a massive boulder at a targeted Mobile, dealing physical damage and knocking the target back.
        public void BoulderToss()
        {
            // Ensure the Combatant is a Mobile before accessing its properties.
            if (!(Combatant is Mobile target))
                return;
            if (!CanBeHarmful(target, false) || !SpellHelper.ValidIndirectTarget(this, target))
                return;

            PlaySound(0x20A); // Boulder toss sound effect
            MovingEffect(target, 0x36D4, 1, 0, false, false, UniqueHue, 0);

            // Calculate delay based on distance
            TimeSpan delay = TimeSpan.FromSeconds(GetDistanceToSqrt(target) / 5.0);
            Timer.DelayCall<Mobile>(delay, EndBoulderToss, target);
        }

        public void EndBoulderToss(Mobile m)
        {
            if (m == null || m.Deleted || !m.Alive || !this.Alive)
                return;

            DoHarmful(m);
            int damage = Utility.RandomMinMax(40, 60);
            AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);

            // Attempt to apply a knockback effect
            int xOffset = m.X - this.X;
            int yOffset = m.Y - this.Y;
            if (xOffset != 0 || yOffset != 0)
            {
                int newX = m.X + (xOffset > 0 ? 1 : -1);
                int newY = m.Y + (yOffset > 0 ? 1 : -1);
                Point3D newLocation = new Point3D(newX, newY, m.Z);
                if (m.Map.CanFit(newLocation.X, newLocation.Y, newLocation.Z, 16, false, false))
                {
                    m.Location = newLocation;
                    m.SendMessage("You are knocked back by the impact!");
                }
            }
        }

        // --- Ability: Chain Stone Barrage ---
        // Bounces rock projectiles between multiple targets within a 5-tile range.
        public void ChainStoneBarrage()
        {
            Say("*Crushing rocks rain down!*");
            PlaySound(0x1FB); // Appropriate stone barrage sound

            List<Mobile> targets = new List<Mobile>();

            // Ensure Combatant is Mobile before proceeding
            if (!(Combatant is Mobile initialTarget))
                return;
            if (!CanBeHarmful(initialTarget, false) || !SpellHelper.ValidIndirectTarget(this, initialTarget))
                return;
            targets.Add(initialTarget);

            int maxTargets = 5;
            int range = 5;

            for (int i = 0; i < maxTargets; i++)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closest = double.MaxValue;
                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, range);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false) &&
                        SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
                    {
                        double dist = lastTarget.GetDistanceToSqrt(m);
                        if (dist < closest)
                        {
                            closest = dist;
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
                    break;
                }
            }

            // Apply damage and effects for each bounce
            for (int i = 0; i < targets.Count; i++)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x36D4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Mobile damageTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(25, 40);
                        AOS.Damage(damageTarget, this, damage, 100, 0, 0, 0, 0);
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        public override int GetAngerSound() { return 0xB5; }
        public override int GetIdleSound() { return 0xB5; }
        public override int GetAttackSound() { return 0x289; }
        public override int GetHurtSound() { return 0xBC; }
        public override int GetDeathSound() { return 0xE4; }

        // Optionally, on taking significant damage, auto-trigger a defensive Tremor Slam.
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (!willKill && this.Hits < (this.HitsMax * 0.25) && DateTime.UtcNow >= m_NextTremorSlamTime)
            {
                TremorSlam();
                m_NextTremorSlamTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
        }

        // --- Hazardous Death Effect ---
        // Upon death, spawn several EarthquakeTile hazards around the corpse.
        public override bool OnBeforeDeath()
        {
            if (Map != null)
            {
                int hazardsToDrop = Utility.RandomMinMax(3, 6);
                for (int i = 0; i < hazardsToDrop; i++)
                {
                    int xOffset = Utility.RandomMinMax(-3, 3);
                    int yOffset = Utility.RandomMinMax(-3, 3);
                    Point3D hazardLoc = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                    if (!Map.CanFit(hazardLoc.X, hazardLoc.Y, hazardLoc.Z, 16, false, false))
                    {
                        hazardLoc.Z = Map.GetAverageZ(hazardLoc.X, hazardLoc.Y);
                        if (!Map.CanFit(hazardLoc.X, hazardLoc.Y, hazardLoc.Z, 16, false, false))
                            continue;
                    }
                    EarthquakeTile tile = new EarthquakeTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(hazardLoc, this.Map);
                }
            }
            return base.OnBeforeDeath();
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 3));
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reinitialize cooldown timers
            m_NextTremorSlamTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            m_NextBoulderTossTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextStoneBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 22));
        }
    }
}
