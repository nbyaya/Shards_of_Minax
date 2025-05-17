using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a sapphire dragon corpse")]
    public class SapphireDragon : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextIceShardStorm;
        private DateTime m_NextLightningCrescent;
        private DateTime m_NextGlacialRing;
        private DateTime m_NextAstralPulse;
        private DateTime m_NextLeviathanRoar;

        // Unique sapphire-blue hue
        private const int UniqueHue = 1158;

        [Constructable]
        public SapphireDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.4, 0.6)
        {
            Name = "a sapphire dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = UniqueHue;

            // Attributes
            SetStr(1400, 1600);
            SetDex(150, 200);
            SetInt(900, 1000);

            SetHits(1500, 1800);
            SetDamage(40, 50);
            SetDamageType(ResistanceType.Cold, 100);

            // Resistances
            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 60, 75);
            SetResistance(ResistanceType.Cold, 95, 100);
            SetResistance(ResistanceType.Poison, 60, 75);
            SetResistance(ResistanceType.Energy, 80, 90);

            // Skills
            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 140.0, 160.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 35000;
            Karma = -35000;
            VirtualArmor = 90;

            // Initialize cooldowns
            m_NextIceShardStorm      = DateTime.UtcNow;
            m_NextLightningCrescent  = DateTime.UtcNow;
            m_NextGlacialRing        = DateTime.UtcNow;
            m_NextAstralPulse        = DateTime.UtcNow;
            m_NextLeviathanRoar      = DateTime.UtcNow;
        }

        public SapphireDragon(Serial serial) : base(serial) { }

        // Overrides
        public override bool CanFly => true;
        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel => true;
        public override HideType HideType => HideType.Barbed;
        public override int Hides => 20;
        public override int Meat => 25;
        public override int Scales => 15;
        public override ScaleType ScaleType => ScaleType.Blue;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 6;

        // Loot
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 5);
            AddLoot(LootPack.Gems, 10);

            if (Utility.RandomDouble() < 0.02) // 2% chance
                PackItem(new TreasureMap(6, Map));
        }

        // Core AI loop
        public override void OnThink()
        {
            base.OnThink();

            if (!(Combatant is Mobile target) || Map == null)
                return;

            // Prioritize abilities by cooldown and context
            if (DateTime.UtcNow >= m_NextIceShardStorm)
                IceShardStorm();
            else if (DateTime.UtcNow >= m_NextLightningCrescent && InRange(target, 12))
                LightningCrescent();
            else if (DateTime.UtcNow >= m_NextGlacialRing)
                GlacialRing();
            else if (DateTime.UtcNow >= m_NextAstralPulse)
                AstralPulse();
            else if (DateTime.UtcNow >= m_NextLeviathanRoar && InRange(target, 15))
                LeviathanRoar();
        }

        // --- Special Abilities ---

        // 1. Ice Shard Storm: Radial circle of ice shards
        private void IceShardStorm()
        {
            var map = Map;
            Effects.PlaySound(Location, map, 0x1FE);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x375A, 10, 20, UniqueHue, 0, 2021, 0);

            int radius = 6, damage = 30;
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    var p = new Point3D(X + x, Y + y, Z);
                    if (!Utility.InRange(Location, p, radius)) continue;

                    // Correct Z if needed
                    int z = map.GetAverageZ(p.X, p.Y);
                    p = new Point3D(p.X, p.Y, z);

                    // Spawn tile and damage
                    var tile = new IceShardTile();
                    tile.MoveToWorld(p, map);

                    foreach (var m in map.GetMobilesInRange(p, 0))
                    {
                        if (m != this)
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                        }
                    }
                }
            }

            m_NextIceShardStorm = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // 2. Lightning Crescent: A swirling lightning cone
        private void LightningCrescent()
        {
            var map = Map;
            if (!(Combatant is Mobile target)) return;

            Effects.PlaySound(Location, map, 0x5A5);

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            int length = 10, width = 4, damage = 35;
            var locations = new List<Point3D>();

            // Build a rough cone
            for (int i = 1; i <= length; i++)
            {
                int span = (int)(i * (width / (double)length));
                for (int j = -span; j <= span; j++)
                {
                    int tx = X + dx * i;
                    int ty = Y + dy * i;

                    if (dx == 0) tx += j;
                    else if (dy == 0) ty += j;
                    else
                    {
                        // Approximate diagonal cone with diamond shape
                        if (Math.Abs(j) + i > length) continue;
                        if (dx * dy > 0) { tx += j; ty -= j; }
                        else { tx += j; ty += j; }
                    }

                    var p = new Point3D(tx, ty, Z);
                    int pz = map.GetAverageZ(tx, ty);
                    p = new Point3D(tx, ty, pz);

                    if (map.CanFit(p, 16, false, false))
                        locations.Add(p);
                }
            }

            // Strike each tile
            foreach (var p in locations)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3818, 10, 10, UniqueHue, 0, 2022, 0);

                var tile = new LightningStormTile();
                tile.MoveToWorld(p, map);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this)
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    }
                }
            }

            m_NextLightningCrescent = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 3. Glacial Ring: A ring of quicksand-like frost around self
        private void GlacialRing()
        {
            var map = Map;
            Effects.PlaySound(Location, map, 0x212);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3735, 20, 30, UniqueHue, 0, 2020, 0);

            int outer = 8, inner = 5, damage = 25;
            for (int angle = 0; angle < 360; angle += 10)
            {
                double rad = Math.PI * angle / 180.0;
                int rx = (int)(outer * Math.Cos(rad)), ry = (int)(outer * Math.Sin(rad));

                var p = new Point3D(X + rx, Y + ry, Z);
                int pz = map.GetAverageZ(p.X, p.Y);
                p = new Point3D(p.X, p.Y, pz);

                if (!map.CanFit(p, 16, false, false)) continue;

                var tile = new QuicksandTile();
                tile.MoveToWorld(p, map);

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x375A, 10, 10, UniqueHue, 0, 2020, 0);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this)
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 50, 0, 0, 50, 0);
                    }
                }
            }

            m_NextGlacialRing = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        // 4. Astral Pulse: Cross-shaped arcane pulses
        private void AstralPulse()
        {
            var map = Map;
            Effects.PlaySound(Location, map, 0x1F3);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3785, 5, 10, UniqueHue, 0, 2025, 0);

            int range = 5, damage = 20;
            var directions = new[] {
                new Point3D(1, 0, 0),
                new Point3D(-1, 0, 0),
                new Point3D(0, 1, 0),
                new Point3D(0, -1, 0)
            };

            foreach (var dir in directions)
            {
                for (int i = 1; i <= range; i++)
                {
                    int tx = X + dir.X * i, ty = Y + dir.Y * i;
                    var p = new Point3D(tx, ty, Z);
                    int pz = map.GetAverageZ(tx, ty);
                    p = new Point3D(tx, ty, pz);

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x3790, 5, 10, UniqueHue, 0, 2025, 0);

                    var tile = new VortexTile();
                    tile.MoveToWorld(p, map);

                    foreach (var m in map.GetMobilesInRange(p, 0))
                    {
                        if (m != this)
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 0, 50, 0, 50, 0);
                        }
                    }
                }
            }

            m_NextAstralPulse = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 5. Leviathan’s Roar: A backward thunderstorm line
        private void LeviathanRoar()
        {
            var map = Map;
            if (!(Combatant is Mobile target)) return;

            Effects.PlaySound(Location, map, 0x22B);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x371C, 10, 15, UniqueHue, 0, 2027, 0);

            Direction d = GetDirectionTo(target);
            Direction back = (Direction)(((int)d + 4) % 8);

            int dx = 0, dy = 0;
            Movement.Movement.Offset(back, ref dx, ref dy);

            int length = 12, damage = 45;
            for (int i = 1; i <= length; i++)
            {
                int tx = X + dx * i, ty = Y + dy * i;
                var p = new Point3D(tx, ty, Z);
                int pz = map.GetAverageZ(tx, ty);
                p = new Point3D(tx, ty, pz);

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3795, 10, 10, UniqueHue, 0, 2028, 0);

                var tile = new ThunderstormTile2();
                tile.MoveToWorld(p, map);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this)
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    }
                }
            }

            m_NextLeviathanRoar = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // Scatter icy and lightning tiles on death
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            base.OnDeath(c);

            Effects.PlaySound(Location, Map, 0x22D);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x372A, 10, 30, UniqueHue, 0, 2030, 0);

            for (int i = 0; i < 15; i++)
            {
                var p = GetRandomValidLocation(Location, 7, Map);
                if (p == Point3D.Zero) continue;

                if (Utility.RandomBool())
                {
                    new IceShardTile().MoveToWorld(p, Map);
                }
                else
                {
                    new LightningStormTile().MoveToWorld(p, Map);
                }

                Effects.SendLocationParticles(
                    EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                    0x375A, 5, 10, UniqueHue, 0, 2031, 0);
            }
        }

        // Helper for random valid point
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = center.X + Utility.RandomMinMax(-radius, radius);
                int y = center.Y + Utility.RandomMinMax(-radius, radius);
                int z = map.GetAverageZ(x, y);
                var p = new Point3D(x, y, z);

                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;
            }
            return Point3D.Zero;
        }

        // Serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
