using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Drakons Ostard corpse")]
    public class DrakonsOstard : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextEldritchCone;
        private DateTime m_NextHoofShockwave;
        private DateTime m_NextChaoticLine;
        private DateTime m_NextUmbralCircle;

        // Unique purple hue
        private const int UniqueHue = 1154;

        [Constructable]
        public DrakonsOstard()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Drakons Ostard";
            Body = 0xDA;
            BaseSoundID = 0x275;
            Hue = UniqueHue;

            // Supercharged stats
            SetStr(300, 350);
            SetDex(150, 175);
            SetInt(200, 225);

            SetHits(1200, 1400);
            SetMana(500, 600);

            SetDamage(25, 35);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Skills
            SetSkill(SkillName.EvalInt,    100.0, 120.0);
            SetSkill(SkillName.Magery,     100.0, 120.0);
            SetSkill(SkillName.MagicResist,120.0, 140.0);
            SetSkill(SkillName.Tactics,    90.0, 100.0);
            SetSkill(SkillName.Wrestling,  90.0, 100.0);

            Fame = 20000;
            Karma = -20000;
            VirtualArmor = 70;

            // Initialize cooldowns
            m_NextEldritchCone   = DateTime.UtcNow;
            m_NextHoofShockwave  = DateTime.UtcNow;
            m_NextChaoticLine    = DateTime.UtcNow;
            m_NextUmbralCircle   = DateTime.UtcNow;
        }

        public DrakonsOstard(Serial serial) : base(serial) { }

        // --- Behavior tweaks ---
        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel => false;

        public override HideType HideType => HideType.Barbed;
        public override int Hides => 25;
        public override int Meat => 10;
        public override int Scales => 10;
        public override ScaleType ScaleType => (ScaleType)Utility.Random(4);

        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Poison.Deadly;

        public override int TreasureMapLevel => 5;
        public override bool CanFly => false;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Gems, 6);

            if (Utility.RandomDouble() < 0.02) // 2% chance
                PackItem(new TreasureMap(5, Map));
        }

        public override int GetIdleSound()  => 0x275;
        public override int GetHurtSound()  => 0x276;
        public override int GetDeathSound() => 0x277;

        // --- Special Abilities ---

        // 1) Eldritch Cone Breath: a jagged triangular cone leaving ice shards
        public void EldritchConeBreath()
        {
            Map map = Map;
            if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            const int range = 10;
            const int damage = 30;

            // Play breath effect
            Effects.PlaySound(Location, map, 0x658);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x374A, 20, 10, UniqueHue, 0, 2035, 0
            );

            // Direction & perpendicular
            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);
            int pdx = -dy, pdy = dx;

            // Build triangular cone
            for (int i = 1; i <= range; i++)
            {
                int width = i; // expands with distance
                for (int j = -width; j <= width; j++)
                {
                    Point3D p = new Point3D(
                        X + i*dx + j*pdx,
                        Y + i*dy + j*pdy,
                        Z
                    );
                    // Adjust Z if blocked
                    if (!map.CanFit(p, 16, false, false))
                        p.Z = map.GetAverageZ(p.X, p.Y);

                    // Spawn ice shard tile
                    // IceShardTile tile = new IceShardTile();
                    // tile.MoveToWorld(p, map);

                    // Damage any mobile on that spot
                    foreach (Mobile m in map.GetMobilesInRange(p, 0))
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);
                        }
                    }
                }
            }

            m_NextEldritchCone = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Hoof Shockwave: circular stomp leaves earthquake tiles
        public void HoofShockwave()
        {
            Map map = Map;
            if (map == null) return;

            const int radius = 6;
            const int damage = 25;

            Effects.PlaySound(Location, map, 0x2F3);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3729, 10, 30, UniqueHue, 0, 5040, 0
            );

            for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
            {
                Point3D p = new Point3D(X + dx, Y + dy, Z);
                if (!Utility.InRange(Location, p, radius))
                    continue;

                if (!map.CanFit(p, 16, false, false))
                    p.Z = map.GetAverageZ(p.X, p.Y);

                // EarthquakeTile tile = new EarthquakeTile();
                // tile.MoveToWorld(p, map);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    }
                }
            }

            m_NextHoofShockwave = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 3) Chaotic Line: a straight AOE line of magnetic rifts
        public void ChaoticLine()
        {
            Map map = Map;
            if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            const int length = 15;
            const int damage = 20;

            Effects.PlaySound(Location, map, 0x5C3);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3710, 8, 5, UniqueHue, 0, 2040, 0
            );

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            for (int i = 1; i <= length; i++)
            {
                Point3D p = new Point3D(X + i*dx, Y + i*dy, Z);
                if (!map.CanFit(p, 16, false, false))
                    p.Z = map.GetAverageZ(p.X, p.Y);

                // MagnetTile tile = new MagnetTile();
                // tile.MoveToWorld(p, map);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                    }
                }
            }

            m_NextChaoticLine = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        // 4) Umbral Circle: dark ring spawning poison & mana-drain tiles
        public void UmbralCircle()
        {
            Map map = Map;
            if (map == null) return;

            const int radius = 5;

            Effects.PlaySound(Location, map, 0x5D4);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37B9, 6, 12, UniqueHue, 0, 5055, 0
            );

            for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
            {
                Point3D p = new Point3D(X + dx, Y + dy, Z);
                if (!Utility.InRange(Location, p, radius))
                    continue;

                if (!map.CanFit(p, 16, false, false))
                    p.Z = map.GetAverageZ(p.X, p.Y);

                // Randomly choose poison or mana-drain
                if (Utility.RandomBool())
                {
                    // PoisonTile t = new PoisonTile();
                    // t.MoveToWorld(p, map);
                }
                else
                {
                    // ManaDrainTile t = new ManaDrainTile();
                    // t.MoveToWorld(p, map);
                }
            }

            m_NextUmbralCircle = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target)
            {
                if (DateTime.UtcNow >= m_NextEldritchCone && InRange(target, 10))
                    EldritchConeBreath();
                else if (DateTime.UtcNow >= m_NextHoofShockwave)
                    HoofShockwave();
                else if (DateTime.UtcNow >= m_NextChaoticLine && InRange(target, 15))
                    ChaoticLine();
                else if (DateTime.UtcNow >= m_NextUmbralCircle)
                    UmbralCircle();
            }
        }

        // --- Death Explosion ---
        public override void OnDeath(Container c)
        {
            Map map = Map;
            if (map != null)
            {
                Effects.PlaySound(Location, map, 0x214);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                    0x3709, 15, 50, UniqueHue, 0, 5060, 0
                );

                for (int i = 0; i < 15; i++)
                {
                    Point3D p = GetRandomValidLocation(Location, 7, map);
                    if (p == Point3D.Zero) continue;

                    // ThunderstormTile t = new ThunderstormTile();
                    // t.MoveToWorld(p, map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x3728, 5, 20, UniqueHue, 0, 5018, 0
                    );
                }
            }

            base.OnDeath(c);
        }

        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                Point3D p = new Point3D(center.X + dx, center.Y + dy, center.Z);

                if (Utility.InRange(center, p, radius) && (map.CanFit(p, 16, false, false) ||
                    (p.Z = map.GetAverageZ(p.X, p.Y)) != center.Z))
                {
                    return p;
                }
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
