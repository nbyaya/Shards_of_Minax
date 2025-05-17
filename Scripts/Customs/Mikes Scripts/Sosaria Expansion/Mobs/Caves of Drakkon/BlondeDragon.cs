using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a blue dragon corpse")]
    public class BlondeDragon : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextFrostBreath;
        private DateTime m_NextShardStorm;
        private DateTime m_NextArcLightning;
        private DateTime m_NextQuicksandTrap;
        private DateTime m_NextVortexPull;

        // Unique icy‑blue hue
        private const int UniqueHue = 1150;

        [Constructable]
        public BlondeDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a blonde dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = UniqueHue;

            // Stats
            SetStr(1400, 1600);
            SetDex(150, 200);
            SetInt(900, 1000);

            SetHits(1200, 1500);
            SetDamage(40, 55);

            // Resistances
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Cold, 90, 100);
            SetResistance(ResistanceType.Fire, 50, 65);
            SetResistance(ResistanceType.Poison, 55, 65);
            SetResistance(ResistanceType.Energy, 70, 80);

            // Skills
            SetSkill(SkillName.EvalInt, 110.0, 130.0);
            SetSkill(SkillName.Magery, 110.0, 130.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 120.0, 160.0);
            SetSkill(SkillName.Tactics, 110.0, 130.0);
            SetSkill(SkillName.Wrestling, 110.0, 130.0);

            Fame = 35000;
            Karma = -35000;
            VirtualArmor = 90;
            Tamable = false;

            // Initialize cooldowns
            m_NextFrostBreath   = DateTime.UtcNow;
            m_NextShardStorm    = DateTime.UtcNow;
            m_NextArcLightning  = DateTime.UtcNow;
            m_NextQuicksandTrap = DateTime.UtcNow;
            m_NextVortexPull    = DateTime.UtcNow;
        }

        public BlondeDragon(Serial serial) : base(serial) { }

        // Basic overrides
        public override bool ReacquireOnMovement => true;
        public override bool AutoDispel          => true;
        public override HideType HideType       => HideType.Barbed;
        public override int Hides               => 60;
        public override int Meat                => 30;
        public override int Scales              => 20;
        public override ScaleType ScaleType     => ScaleType.Blue;
        public override Poison PoisonImmune     => Poison.Lethal;
        public override Poison HitPoison        => Poison.Deadly;
        public override int TreasureMapLevel    => 7;
        public override bool CanFly             => true;

        // Loot
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
            AddLoot(LootPack.Gems,       10);

            if (Utility.RandomDouble() < 0.02) // 2% chance
                PackItem(new KabutoOfIronTempests()); // your custom item
        }

        // Sounds
        public override int GetIdleSound()  => 362;
        public override int GetAngerSound() => 362;
        public override int GetHurtSound()  => 362;
        public override int GetDeathSound() => 362;

        // --- Special Abilities ---

        // 1) FrostBreath: an 8×3‑cone of chilling breath that leaves IceShardTiles
        public void FrostBreath()
        {
            if (!(Combatant is Mobile target)) return;
            if (!this.InRange(target, 8)) return;

            Map map = Map;
            if (map == null) return;

            // Cone effect
            Effects.PlaySound(Location, map, 0x5C4);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37B3, 9, 20, UniqueHue, 0, 5023, 0);

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            int range = 8, width = 3, damage = 30;
            var conePoints = new List<Point3D>();

            for (int i = 1; i <= range; i++)
            {
                int w = (int)(i * (width / (double)range));
                for (int j = -w; j <= w; j++)
                {
                    int x = X + i*dx;
                    int y = Y + i*dy;
                    if (dx == 0) x += j;
                    else if (dy == 0) y += j;
                    else
                    {
                        // Diamond fallback
                        if (Math.Abs(j) + i > range) continue;
                        x += j;
                        y += (dx*dy > 0 ? -j : j);
                    }

                    var p = new Point3D(x, y, Z);
                    if (!map.CanFit(p,16,false,false))
                    {
                        int z2 = map.GetAverageZ(x,y);
                        p = new Point3D(x,y,z2);
                        if (!map.CanFit(p,16,false,false)) continue;
                    }
                    conePoints.Add(p);
                }
            }

            // Apply
            foreach (var p in conePoints)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x37B3, 5, 10, UniqueHue, 0, 5023, 0);

                foreach (Mobile m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || 
                       (m is BaseCreature bc && bc.Team != this.Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    }
                }

                // Ice shards remain briefly
                if (Utility.RandomDouble() < 0.3)
                {
                    // var tile = new IceShardTile();
                    // tile.MoveToWorld(p, map);
                }
            }

            m_NextFrostBreath = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) ShardStorm: 6‑tile radius burst of cold + scattered IceShardTiles
        public void ShardStorm()
        {
            Map map = Map;
            if (map == null) return;

            int radius = 6, damage = 35;
            Effects.PlaySound(Location, map, 0x658);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x379A, 15, 30, UniqueHue, 0, 5030, 0);

            var points = new List<Point3D>();
            for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
            {
                var p = new Point3D(X+dx, Y+dy, Z);
                if (!Utility.InRange(Location, p, radius)) continue;
                if (!map.CanFit(p,16,false,false))
                {
                    int z2 = map.GetAverageZ(p.X,p.Y);
                    p = new Point3D(p.X,p.Y,z2);
                    if (!map.CanFit(p,16,false,false)) continue;
                }
                points.Add(p);
            }

            foreach (var p in points)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x379A, 8, 16, UniqueHue, 0, 5030, 0);

                foreach (Mobile m in map.GetMobilesInRange(p,0))
                {
                    if (m != this && (m is PlayerMobile ||
                       (m is BaseCreature bc && bc.Team != this.Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    }
                }

                if (Utility.RandomDouble() < 0.2)
                {
                    // var tile = new IceShardTile();
                    // tile.MoveToWorld(p, map);
                }
            }

            m_NextShardStorm = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 3) ArcLightning: 12‑tile straight beam + chance to spawn LightningStormTile
        public void ArcLightning()
        {
            if (!(Combatant is Mobile target)) return;
            if (!Utility.InRange(Location, target.Location, 12)) return;

            Map map = Map;
            if (map == null) return;

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            Effects.PlaySound(Location, map, 0x5C5);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3728, 12, 12, UniqueHue, 0, 2032, 0);

            int length = 12, damage = 45;
            for (int i = 1; i <= length; i++)
            {
                var p = new Point3D(X + i*dx, Y + i*dy, Z);
                if (!map.CanFit(p,16,false,false))
                {
                    int z2 = map.GetAverageZ(p.X,p.Y);
                    p = new Point3D(p.X,p.Y,z2);
                    if (!map.CanFit(p,16,false,false)) break;
                }

                Effects.SendLocationEffect(p, map, 0x3818, 16, UniqueHue, 0);

                foreach (Mobile m in map.GetMobilesInRange(p,0))
                {
                    if (m != this && (m is PlayerMobile ||
                       (m is BaseCreature bc && bc.Team != this.Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    }
                }

                if (Utility.RandomDouble() < 0.2)
                {
                    // var tile = new LightningStormTile();
                    // tile.MoveToWorld(p, map);
                }
            }

            m_NextArcLightning = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // 4) QuicksandTrap: randomly drop QuicksandTiles within 8 tiles
        public void QuicksandTrap()
        {
            Map map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x027);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x38E2, 10, 20, UniqueHue, 0, 5061, 0);

            int radius = 8;
            for (int i = 0; i < 12; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(X+dx, Y+dy, Z);

                if (!Utility.InRange(Location, p, radius)) continue;
                if (!map.CanFit(p,16,false,false))
                {
                    int z2 = map.GetAverageZ(p.X,p.Y);
                    p = new Point3D(p.X,p.Y,z2);
                    if (!map.CanFit(p,16,false,false)) continue;
                }

                if (Utility.RandomDouble() < 0.15)
                {
                    // var tile = new QuicksandTile();
                    // tile.MoveToWorld(p, map);
                }
            }

            m_NextQuicksandTrap = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // 5) VortexPull: 5‑tile radius pull + spawn VortexTile
        public void VortexPull()
        {
            Map map = Map;
            if (map == null) return;

            int radius = 5, damage = 25;
            Effects.PlaySound(Location, map, 0x209);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3779, 8, 16, UniqueHue, 0, 9909, 0);

            for (int dx = -radius; dx <= radius; dx++)
            for (int dy = -radius; dy <= radius; dy++)
            {
                var p = new Point3D(X+dx, Y+dy, Z);
                if (!Utility.InRange(Location, p, radius)) continue;
                if (!map.CanFit(p,16,false,false))
                {
                    int z2 = map.GetAverageZ(p.X,p.Y);
                    p = new Point3D(p.X,p.Y,z2);
                    if (!map.CanFit(p,16,false,false)) continue;
                }

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3779, 4, 8, UniqueHue, 0, 9909, 0);

                foreach (Mobile m in map.GetMobilesInRange(p,0))
                {
                    if (m != this && (m is PlayerMobile ||
                       (m is BaseCreature bc && bc.Team != this.Team)))
                    {
                        DoHarmful(m);
                        // Pull one step toward center
                        int mx = m.X + Math.Sign(X - m.X);
                        int my = m.Y + Math.Sign(Y - m.Y);
                        m.MoveToWorld(new Point3D(mx, my, m.Z), map);

                        AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                    }
                }

                if (Utility.RandomDouble() < 0.2)
                {
                    // var tile = new VortexTile();
                    // tile.MoveToWorld(p, map);
                }
            }

            m_NextVortexPull = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        // AI dispatch
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile tgt)
            {
                if (DateTime.UtcNow >= m_NextFrostBreath &&
                    this.InRange(tgt, 8))
                {
                    FrostBreath();
                }
                else if (DateTime.UtcNow >= m_NextShardStorm)
                {
                    ShardStorm();
                }
                else if (DateTime.UtcNow >= m_NextArcLightning &&
                         Utility.InRange(Location, tgt.Location, 12))
                {
                    ArcLightning();
                }
                else if (DateTime.UtcNow >= m_NextVortexPull)
                {
                    VortexPull();
                }
                else if (DateTime.UtcNow >= m_NextQuicksandTrap)
                {
                    QuicksandTrap();
                }
            }
        }

        // On‑death scatter hazards
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Effects.PlaySound(Location, Map, 0x2E5);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 12, 60, UniqueHue, 0, 5052, 0);

                for (int i = 0; i < 15; i++)
                {
                    var p = GetRandomValidLocation(Location, 7, Map);
                    if (p == Point3D.Zero) continue;

                    switch (Utility.Random(3))
                    {
                        case 0:
                            // new IceShardTile().MoveToWorld(p, Map);
                            break;
                        case 1:
                            // new QuicksandTile().MoveToWorld(p, Map);
                            break;
                        default:
                            // new ThunderstormTile2().MoveToWorld(p, Map);
                            break;
                    }

                    Effects.SendLocationParticles(
                        EffectItem.Create(p, Map, EffectItem.DefaultDuration),
                        0x3709, 8, 20, UniqueHue, 0, 5016, 0);
                }
            }

            base.OnDeath(c);
        }

        // Helper: find a valid point
        private Point3D GetRandomValidLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                int dx = Utility.RandomMinMax(-radius, radius);
                int dy = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + dx, center.Y + dy, center.Z);

                if (!Utility.InRange(center, p, radius)) continue;
                if (!map.CanFit(p,16,false,false))
                {
                    int z2 = map.GetAverageZ(p.X,p.Y);
                    p = new Point3D(p.X,p.Y,z2);
                    if (!map.CanFit(p,16,false,false)) continue;
                }
                return p;
            }
            return Point3D.Zero;
        }

        // Serialization
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
