using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;        // for spell effects (optional)
using Server.Spells.Seventh; // for possible animations

namespace Server.Mobiles
{
    [CorpseName("a furnace drake corpse")]
    public class FurnaceDrakeMK1 : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextBreathTime;
        private DateTime m_NextMagmaBurstTime;
        private DateTime m_NextSpikesTime;
        private Point3D m_LastLocation;

        // Unique fiery orange hue
        private const int UniqueHue = 1350;

        [Constructable]
        public FurnaceDrakeMK1()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Furnace Drake MK-I";
            Body = Utility.RandomList(60, 61);
            BaseSoundID = 362;
            Hue = UniqueHue;

            // Stats
            SetStr(500, 550);
            SetDex(150, 180);
            SetInt(80, 100);

            SetHits(1200, 1300);
            SetStam(200, 250);
            SetMana(100, 150);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 40);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Wrestling, 100.1, 110.0);
            SetSkill(SkillName.Tactics,   100.1, 110.0);
            SetSkill(SkillName.MagicResist, 90.1, 100.0);
            SetSkill(SkillName.Meditation, 80.0,  90.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 100;
            ControlSlots = 5;

            // Schedule first uses
            m_NextBreathTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextMagmaBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextSpikesTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            m_LastLocation = this.Location;

            // Basic loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 25)));
            PackItem(new Granite(Utility.RandomMinMax(5, 10)));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems,      Utility.RandomMinMax(8, 12));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));

            // 5% chance to drop the unique Furnace Core
            if (Utility.RandomDouble() < 0.05)
                PackItem(new OceanclaspBands());
        }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 140.0;
        public override double DispelFocus     => 70.0;

        public FurnaceDrakeMK1(Serial serial) : base(serial) { }

        // --- Heat Aura on movement ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    // Light burn damage
                    AOS.Damage(target, this, Utility.RandomMinMax(8, 15), 0, 0, 0, 0, 100);
                    target.SendMessage(0x22, "The intense heat sears you as you move too close!");
                    target.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.Waist);
                }
            }
        }

        // --- Main AI Loop for special attacks & leaving lava pools ---
        public override void OnThink()
        {
            base.OnThink();

            // Leave a hot lava tile where it just was, 20% chance per move
            if (this.Location != m_LastLocation && this.Map != null && Utility.RandomDouble() < 0.20)
            {
                Point3D old = m_LastLocation;
                m_LastLocation = this.Location;

                if (this.Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                {
                    var tile = new HotLavaTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(old, this.Map);
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Flame Breath: short cone
            if (now >= m_NextBreathTime && this.InRange(Combatant.Location, 6))
            {
                FlameBreath();
                m_NextBreathTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Magma Burst: AoE around self
            else if (now >= m_NextMagmaBurstTime)
            {
                MagmaBurst();
                m_NextMagmaBurstTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Molten Spikes: hazard at target
            else if (now >= m_NextSpikesTime && this.InRange(Combatant.Location, 12))
            {
                MoltenSpikes();
                m_NextSpikesTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
            }
        }

        private void FlameBreath()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*Inferno Breath!*");
            PlaySound(0x66D);
            FixedParticles(0x370A, 1, 30, 9966, UniqueHue, 0, EffectLayer.Head);

            var cone = GetConeTargets(this, 5, Math.PI / 4); // 90° cone, 5 tiles
            foreach (var m in cone)
            {
                if (m is Mobile mTarget)
                {
                    DoHarmful(mTarget);
                    int dmg = Utility.RandomMinMax(40, 60);
                    AOS.Damage(mTarget, this, dmg, 0, 0, 0, 0, 100);
                    mTarget.FixedParticles(0x3709, 10, 20, 5052, UniqueHue, 0, EffectLayer.Waist);
                }
            }
        }

        private void MagmaBurst()
        {
            this.Say("*Magma Burst!*");
            PlaySound(0x208);
            FixedParticles(0x36BD, 20, 10, 5044, UniqueHue, 0, EffectLayer.CenterFeet);

            var targets = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in targets)
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile victim)
                {
                    DoHarmful(victim);
                    int dmg = Utility.RandomMinMax(30, 50);
                    AOS.Damage(victim, this, dmg, 0, 0, 0, 0, 100);

                    // scatter flamestrike hazard tiles
                    var tile = new FlamestrikeHazardTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(victim.Location, this.Map);
                }
            }
        }

        private void MoltenSpikes()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            this.Say("*Molten earth!*");
            PlaySound(0x654);

            var loc = target.Location;
            Effects.SendLocationParticles(EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration), 0x3818, 10, 30, UniqueHue, 0, 9502, 0);

            // delay spawning the spike
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (this.Map == null) return;

                var spike = new LandmineTile();
                spike.Hue = UniqueHue;
                spike.MoveToWorld(loc, this.Map);
            });
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            this.Say("*Ashes to ash...*");
            PlaySound(0x208);
            FixedParticles(0x3709, 20, 30, 5052, UniqueHue, 0, EffectLayer.Head);

            // Generate pools of lava around corpse
            int count = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < count; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var lava = new HotLavaTile();
                lava.Hue = UniqueHue;
                lava.MoveToWorld(new Point3D(x, y, z), this.Map);
            }
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

            // Reset cooldowns
            m_NextBreathTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextMagmaBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextSpikesTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }

        // Utility: get targets in a cone (for breath)
        private static IEnumerable<Mobile> GetConeTargets(BaseCreature from, double range, double halfAngle)
        {
            var list = new List<Mobile>();
            var all = from.Map.GetMobilesInRange(from.Location, (int)Math.Ceiling(range));

            double direction = ((int)from.Direction & 0x7) * (Math.PI / 4);
            foreach (Mobile m in all)
            {
                if (m == from || !from.CanBeHarmful(m, false) || !from.InLOS(m))
                    continue;

                double dx = m.X - from.X, dy = m.Y - from.Y;
                double dist = Math.Sqrt(dx * dx + dy * dy);
                if (dist > range) continue;

                double angleTo = Math.Atan2(dy, dx);
                double diff = Math.Abs(NormalizeAngle(angleTo - direction));
                if (diff <= halfAngle)
                    list.Add(m);
            }
            return list;
        }

        private static double NormalizeAngle(double a)
        {
            while (a < -Math.PI) a += 2 * Math.PI;
            while (a >  Math.PI) a -= 2 * Math.PI;
            return a;
        }
    }
}
