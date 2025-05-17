using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a white dragon corpse")]
    public class WhiteDragon : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextFrostBreath;
        private DateTime m_NextGlacialBeam;
        private DateTime m_NextHailstorm;
        private DateTime m_NextIcePrison;

        // Unique icy‑white hue
        private const int UniqueHue = 1153;

        [Constructable]
        public WhiteDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a white dragon";
            Body = 12;               // Standard dragon body
            BaseSoundID = 362;       // Dragon sounds
            Hue = UniqueHue;         // Icy white

            // Enhanced stats
            SetStr(1400, 1600);
            SetDex(200, 250);
            SetInt(900, 1000);

            SetHits(2000, 2500);
            SetDamage(40, 55);

            // Damage types
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 90, 100);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 60, 80);

            // Skills
            SetSkill(SkillName.EvalInt,   120.0, 140.0);
            SetSkill(SkillName.Magery,    120.0, 140.0);
            SetSkill(SkillName.MagicResist, 140.0, 170.0);
            SetSkill(SkillName.Tactics,   120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 35000;
            Karma = -35000;
            VirtualArmor = 100;

            // Initialize ability cooldowns
            m_NextFrostBreath = m_NextGlacialBeam = m_NextHailstorm = m_NextIcePrison = DateTime.UtcNow;
        }

        public WhiteDragon(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel         => true;
        public override HideType HideType      => HideType.Barbed;
        public override int Hides              => 75;
        public override int Meat               => 30;
        public override int Scales             => 20;
        public override ScaleType ScaleType    => ScaleType.White;
        public override Poison PoisonImmune    => Poison.Lethal;
        public override Poison HitPoison       => Poison.Deadly;
        public override int TreasureMapLevel   => 7;
        public override bool CanFly            => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.Gems, 10);

            if (Utility.RandomDouble() < 0.02) // 2% chance
                PackItem(new MoonpetalFang());

            if (Utility.RandomDouble() < 0.05) // 5% chance
                PackItem(new TreasureMap(7, Map));
        }

        // --- Frost Breath: cone of chilling cold ---
        public void FrostBreath()
        {
            if (!(Combatant is Mobile target)) return;

            Map map = Map;
            if (map == null || !InRange(target, 8)) return;

            // Play breath effect
            Effects.PlaySound(Location, map, 0x658);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x376A, 10, 30, UniqueHue, 0, 5024, 0);

            // Determine cone shape
            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            var coneLocs = new List<Point3D>();
            int range = 8, width = 4;
            for (int i = 1; i <= range; i++)
            {
                int half = (int)(i * (width / (double)range));
                for (int j = -half; j <= half; j++)
                {
                    int x = X + i*dx, y = Y + i*dy;
                    if (dx == 0) x += j; else if (dy == 0) y += j;
                    var p = new Point3D(x, y, Z);
                    if (!map.CanFit(p, 16, false, false))
                    {
                        int z2 = map.GetAverageZ(x, y);
                        p = new Point3D(x, y, z2);
                        if (!map.CanFit(p, 16, false, false)) continue;
                    }
                    coneLocs.Add(p);
                }
            }

            // Apply damage & spawn IceShardTile
            foreach (var p in coneLocs)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x36BD, 8, 8, UniqueHue, 0, 5024, 0);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this || (m is BaseCreature bc && bc.Team == Team)) continue;
                    DoHarmful(m);
                    AOS.Damage(m, this, 25, 0, 0, 100, 0, 0);
                }

                if (Utility.RandomDouble() < 0.3)
                {
                    // var ice = new IceShardTile();
                    // ice.MoveToWorld(p, map);
                }
            }

            m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // --- Glacial Beam: straight line of crushing ice shards ---
        public void GlacialBeam()
        {
            if (!(Combatant is Mobile target)) return;

            Map map = Map;
            if (map == null || !InRange(target, 15)) return;

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            var lineLocs = new List<Point3D>();
            int length = 15;
            for (int i = 1; i <= length; i++)
            {
                int x = X + i*dx, y = Y + i*dy;
                var p = new Point3D(x, y, Z);
                if (!map.CanFit(p, 16, false, false))
                {
                    int z2 = map.GetAverageZ(x, y);
                    p = new Point3D(x, y, z2);
                    if (!map.CanFit(p, 16, false, false)) break;
                }
                lineLocs.Add(p);
            }

            Effects.PlaySound(Location, map, 0x65F);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3788, 12, 12, UniqueHue, 0, 2031, 0);

            foreach (var p in lineLocs)
            {
                Effects.SendLocationEffect(p, map, 0x3836, 18, UniqueHue, 0);
                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this || (m is BaseCreature bc && bc.Team == Team)) continue;
                    DoHarmful(m);
                    AOS.Damage(m, this, 30, 0, 0, 100, 0, 0);
                }
            }

            m_NextGlacialBeam = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // --- Hailstorm: radial blizzard around the dragon ---
        public void Hailstorm()
        {
            Map map = Map;
            if (map == null) return;

            const int radius = 6;
            const int damage = 20;

            Effects.PlaySound(Location, map, 0x2F4);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37AE, 14, 50, UniqueHue, 0, 5033, 0);

            var tiles = new List<Point3D>();
            for (int dx = -radius; dx <= radius; dx++)
            {
                for (int dy = -radius; dy <= radius; dy++)
                {
                    var p = new Point3D(X + dx, Y + dy, Z);
                    if (!map.CanFit(p, 16, false, false) || !Utility.InRange(Location, p, radius))
                    {
                        int z2 = map.GetAverageZ(p.X, p.Y);
                        p = new Point3D(p.X, p.Y, z2);
                        if (!map.CanFit(p, 16, false, false) || !Utility.InRange(Location, p, radius))
                            continue;
                    }
                    tiles.Add(p);
                }
            }

            foreach (var p in tiles)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x37AE, 8, 8, UniqueHue, 0, 5033, 0);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this || (m is BaseCreature bc && bc.Team == Team)) continue;
                    DoHarmful(m);
                    AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                }

                if (Utility.RandomDouble() < 0.2)
                {
                    // var ice = new IceShardTile();
                    // ice.MoveToWorld(p, map);
                }
            }

            m_NextHailstorm = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // --- Ice Prison: encase the target in freezing ice ---
        public void IcePrison()
        {
            if (!(Combatant is Mobile target)) return;

            Map map = Map;
            if (map == null || !InRange(target, 4)) return;

            Effects.PlaySound(target.Location, map, 0x227);
            Effects.SendLocationParticles(
                EffectItem.Create(target.Location, map, EffectItem.DefaultDuration),
                0x37C1, 5, 20, UniqueHue, 0, 5039, 0);

            // Apply damage and slow
            DoHarmful(target);
            AOS.Damage(target, this, 50, 0, 0, 100, 0, 0);
            // target.SendMessage("You are encased in ice!"); // optional

            // Spawn a small ring of ice tiles
            for (int i = 0; i < 8; i++)
            {
                var p = target.Location;
                p.X += Utility.RandomMinMax(-1, 1);
                p.Y += Utility.RandomMinMax(-1, 1);
                // var prison = new IceShardTile();
                // prison.MoveToWorld(p, map);
            }

            m_NextIcePrison = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile t)
            {
                if (DateTime.UtcNow >= m_NextGlacialBeam && InRange(t, 15))
                    GlacialBeam();
                else if (DateTime.UtcNow >= m_NextFrostBreath && InRange(t, 8))
                    FrostBreath();
                else if (DateTime.UtcNow >= m_NextHailstorm)
                    Hailstorm();
                else if (DateTime.UtcNow >= m_NextIcePrison && InRange(t, 4))
                    IcePrison();
            }
        }

        // --- OnDeath: icy shrapnel explosion ---
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Effects.PlaySound(Location, Map, 0x213);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x37AE, 12, 60, UniqueHue, 0, 5033, 0);

                for (int i = 0; i < 25; i++)
                {
                    var p = GetRandomSpawnLocation(Location, 5, Map);
                    if (p != Point3D.Zero)
                    {
                        // var ice = new IceShardTile();
                        // ice.MoveToWorld(p, Map);
                        Effects.SendLocationParticles(
                            EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                            0x37AE, 6, 20, UniqueHue, 0, 5033, 0);
                    }
                }
            }
            base.OnDeath(c);
        }

        private Point3D GetRandomSpawnLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + dx, center.Y + dy, center.Z);
                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;
                var z2 = map.GetAverageZ(p.X, p.Y);
                var p2 = new Point3D(p.X, p.Y, z2);
                if (map.CanFit(p2, 16, false, false) && Utility.InRange(center, p2, radius))
                    return p2;
            }
            return Point3D.Zero;
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
        }
    }
}
