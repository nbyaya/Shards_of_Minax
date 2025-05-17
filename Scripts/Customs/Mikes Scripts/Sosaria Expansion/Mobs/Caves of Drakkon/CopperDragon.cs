using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a copper dragon corpse")]
    public class CopperDragon : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextShardStorm;
        private DateTime m_NextResonantPulse;
        private DateTime m_NextMagneticVortex;
        private DateTime m_NextCorrosiveCloud;
        private DateTime m_NextElectrifiedBreath;

        // Unique copper hue
        private const int UniqueHue = 1355;

        [Constructable]
        public CopperDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a copper dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = UniqueHue;

            // Stats
            SetStr(1300, 1500);
            SetDex(120, 160);
            SetInt(900, 1000);

            SetHits(1500, 1800);
            SetDamage(40, 55);

            // Resistances
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire,  50, 70);
            SetResistance(ResistanceType.Cold,  60, 75);
            SetResistance(ResistanceType.Poison,55, 70);
            SetResistance(ResistanceType.Energy,85, 95);

            // Skills
            SetSkill(SkillName.Magery,    120.0, 140.0);
            SetSkill(SkillName.EvalInt,   120.0, 140.0);
            SetSkill(SkillName.MagicResist,130.0,150.0);
            SetSkill(SkillName.Tactics,   120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);
            SetSkill(SkillName.Meditation,  80.0, 100.0);

            Fame = 35000;
            Karma = -35000;
            VirtualArmor = 100;

            Tamable = false; // Not meant to be tamed

            // Initialize cooldowns
            m_NextShardStorm      = DateTime.UtcNow;
            m_NextResonantPulse   = DateTime.UtcNow;
            m_NextMagneticVortex  = DateTime.UtcNow;
            m_NextCorrosiveCloud  = DateTime.UtcNow;
            m_NextElectrifiedBreath = DateTime.UtcNow;
        }

        public CopperDragon(Serial serial) : base(serial) { }

        // Basic properties
        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel        => true;
        public override HideType HideType     => HideType.Barbed;
        public override int Hides             => 60;
        public override int Meat              => 30;
        public override int Scales            => 20;
        public override ScaleType ScaleType   => (ScaleType)Utility.Random(7);
        public override Poison PoisonImmune   => Poison.Lethal;
        public override Poison HitPoison      => Poison.Deadly;
        public override int TreasureMapLevel  => 6;
        public override bool CanFly           => true;

        // Loot
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 5);
            AddLoot(LootPack.Gems,       12);

            if (Utility.RandomDouble() < 0.02)
                PackItem(new TreasureMap(6, Map));
        }

        // Sounds
        public override int GetIdleSound()   => BaseSoundID;
        public override int GetAttackSound() => 0x2D9;
        public override int GetHurtSound()   => 0x2D7;
        public override int GetDeathSound()  => 0x2DB;

        // --- Special Abilities ---

        // 1) Metallic Shard Storm: radial steel shards
        public void MetallicShardStorm()
        {
            Map map = Map;
            if (map == null || DateTime.UtcNow < m_NextShardStorm || !(Combatant is Mobile target))
                return;

            int radius = 6, damage = 25;
            Effects.PlaySound(Location, map, 0x1FE);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3709, 8, 20, UniqueHue, 0, 2032, 0
            );

            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                Point3D p = new Point3D(X + x, Y + y, Z);
                if (map.CanFit(p, 16, false, false) && Utility.InRange(Location, p, radius))
                {
                    Effects.SendLocationEffect(p, map, 0x36B0, 10, UniqueHue, 0);
                    var eable = map.GetMobilesInRange(p, 0);
                    foreach (Mobile m in eable)
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                        }
                    }
                    eable.Free();
                }
            }

            m_NextShardStorm = DateTime.UtcNow.AddSeconds(12);
        }

        // 2) Resonant Pulse: plus‑shaped sonic wave
        public void ResonantPulse()
        {
            Map map = Map;
            if (map == null || DateTime.UtcNow < m_NextResonantPulse || !(Combatant is Mobile target))
                return;

            int length = 8, damage = 30;
            Effects.PlaySound(Location, map, 0x665);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3728, 12, 12, UniqueHue, 0, 4047, 0
            );

            var locs = new List<Point3D>();
            for (int i = -length; i <= length; i++)
            {
                locs.Add(new Point3D(X + i, Y, Z));
                locs.Add(new Point3D(X, Y + i, Z));
            }

            foreach (Point3D p in locs)
            {
                if (!map.CanFit(p, 16, false, false)) continue;
                Effects.SendLocationEffect(p, map, 0x3818, 16, UniqueHue, 0);

                var eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);
                    }
                }
                eable.Free();
            }

            m_NextResonantPulse = DateTime.UtcNow.AddSeconds(15);
        }

        // 3) Magnetic Vortex: ring of pulling metal shards
        public void MagneticVortex()
        {
            Map map = Map;
            if (map == null || DateTime.UtcNow < m_NextMagneticVortex || !(Combatant is Mobile target))
                return;

            int radius = 5;
            Effects.PlaySound(Location, map, 0x210);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x2104, 6, 30, UniqueHue, 0, 5050, 0
            );

            // 12‑point ring
            for (int i = 0; i < 12; i++)
            {
                double ang = i * (Math.PI * 2 / 12);
                int dx = (int)Math.Round(Math.Cos(ang) * radius);
                int dy = (int)Math.Round(Math.Sin(ang) * radius);
                Point3D p = new Point3D(X + dx, Y + dy, Z);

                if (!map.CanFit(p, 16, false, false)) continue;
                Effects.SendLocationEffect(p, map, 0x36B0, 10, UniqueHue, 0);

                var eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 35, 50, 0, 0, 0, 50);
                        // Optional pull/push: m.Location = Location;
                    }
                }
                eable.Free();
            }

            m_NextMagneticVortex = DateTime.UtcNow.AddSeconds(18);
        }

        // 4) Corrosive Cloud: random poison‑tile barrage
        public void CorrosiveCloud()
        {
            Map map = Map;
            if (map == null || DateTime.UtcNow < m_NextCorrosiveCloud || !(Combatant is Mobile target))
                return;

            int cloudRadius = 4;
            Effects.PlaySound(Location, map, 0x21E);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3709, 8, 40, UniqueHue, 0, 5051, 0
            );

            for (int i = 0; i < 20; i++)
            {
                int x = Utility.RandomMinMax(-cloudRadius, cloudRadius);
                int y = Utility.RandomMinMax(-cloudRadius, cloudRadius);
                Point3D p = new Point3D(X + x, Y + y, Z);

                if (!map.CanFit(p, 16, false, false)) continue;
                // new PoisonTile().MoveToWorld(p, map);

                var eable = map.GetMobilesInRange(p, 0);
                foreach (Mobile m in eable)
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 15, 0, 0, 100, 0, 0);
                        // m.ApplyPoison(this, Poison.Deadly);
                    }
                }
                eable.Free();
            }

            m_NextCorrosiveCloud = DateTime.UtcNow.AddSeconds(20);
        }

		private static void DirectionOffset(Direction d, out int dx, out int dy)
		{
			dx = 0; dy = 0;
			switch (d)
			{
				case Direction.North: dy = -1; break;
				case Direction.South: dy = 1; break;
				case Direction.West: dx = -1; break;
				case Direction.East: dx = 1; break;
				case Direction.Up: dx = -1; dy = -1; break;
				case Direction.Down: dx = 1; dy = 1; break;
				case Direction.Left: dx = -1; dy = 1; break;
				case Direction.Right: dx = 1; dy = -1; break;
			}
		}


        // 5) Electrified Breath: cone‐shaped lightning breath
        public void ElectrifiedBreath()
        {
            Map map = Map;
            if (map == null || DateTime.UtcNow < m_NextElectrifiedBreath || !(Combatant is Mobile target))
                return;

            int range = 10, width = 4;
            Effects.PlaySound(Location, map, 0x65B);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3728, 12, 12, UniqueHue, 0, 2024, 0
            );

            Direction d = GetDirectionTo(target);
            DirectionOffset(d, out int dx, out int dy);

            for (int i = 1; i <= range; i++)
            {
                int w = (int)Math.Ceiling(width * i / (double)range);
                for (int j = -w; j <= w; j++)
                {
                    int tx = X + i*dx, ty = Y + i*dy;
                    if (dx == 0) tx += j;
                    else if (dy == 0) ty += j;
                    else { /* Intercardinal adjustments if desired */ }

                    Point3D p = new Point3D(tx, ty, Z);
                    if (!map.CanFit(p, 16, false, false)) continue;

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x3818, 8, 15, UniqueHue, 0, 2030, 0
                    );

                    var eable = map.GetMobilesInRange(p, 0);
                    foreach (Mobile m in eable)
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, 20, 0, 0, 0, 0, 100);
                            // Optional stun: m.Stam -= 20;
                        }
                    }
                    eable.Free();
                }
            }

            m_NextElectrifiedBreath = DateTime.UtcNow.AddSeconds(10);
        }

        // AI chooses when to trigger each ability
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null) return;

            var m = Combatant as Mobile;
            if (m == null) return;

            if (DateTime.UtcNow >= m_NextElectrifiedBreath && InRange(m, 10))
                ElectrifiedBreath();
            else if (DateTime.UtcNow >= m_NextShardStorm)
                MetallicShardStorm();
            else if (DateTime.UtcNow >= m_NextResonantPulse)
                ResonantPulse();
            else if (DateTime.UtcNow >= m_NextMagneticVortex)
                MagneticVortex();
            else if (DateTime.UtcNow >= m_NextCorrosiveCloud)
                CorrosiveCloud();
        }

        // Death effect: scatter random hazard tiles
        public override void OnDeath(Container c)
        {
            Map map = Map;
            if (map != null)
            {
                Effects.PlaySound(Location, map, 0x213);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                    0x3709, 12, 60, UniqueHue, 0, 4049, 0
                );

                for (int i = 0; i < 25; i++)
                {
                    Point3D p = GetRandomValidLocation(Location, 8, map);
                    if (p == Point3D.Zero) continue;

                    if (Utility.RandomBool())
                    {
                        // new LightningStormTile().MoveToWorld(p, map);
                    }
                    else
                    {
                        // new LandmineTile().MoveToWorld(p, map);
                    }

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x3728, 6, 20, UniqueHue, 0, 2030, 0
                    );
                }
            }

            base.OnDeath(c);
        }

        // Helper for OnDeath
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + dx, center.Y + dy, center.Z);

                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;

                int z = map.GetAverageZ(p.X, p.Y);
                var p2 = new Point3D(p.X, p.Y, z);
                if (map.CanFit(p2, 16, false, false) && Utility.InRange(center, p2, radius))
                    return p2;
            }
            return Point3D.Zero;
        }

        // Serialization of cooldowns
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_NextShardStorm);
            writer.Write(m_NextResonantPulse);
            writer.Write(m_NextMagneticVortex);
            writer.Write(m_NextCorrosiveCloud);
            writer.Write(m_NextElectrifiedBreath);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextShardStorm       = reader.ReadDateTime();
            m_NextResonantPulse    = reader.ReadDateTime();
            m_NextMagneticVortex   = reader.ReadDateTime();
            m_NextCorrosiveCloud   = reader.ReadDateTime();
            m_NextElectrifiedBreath = reader.ReadDateTime();
        }
    }
}
