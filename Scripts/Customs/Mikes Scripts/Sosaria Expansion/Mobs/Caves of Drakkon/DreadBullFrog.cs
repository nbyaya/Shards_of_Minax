using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a dread bullfrog corpse")]
    public class DreadBullFrog : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextTongueWhip;
        private DateTime m_NextVenomSpray;
        private DateTime m_NextQuagmireSlam;
        private DateTime m_NextThunderCroak;
        private DateTime m_NextSummonSpawn;

        // Unique dark-emerald hue for visual distinction
        private const int UniqueHue = 2218;

        [Constructable]
        public DreadBullFrog()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.5)
        {
            Name = "a dread bullfrog";
            Body = 81;                   // Bullfrog body
            BaseSoundID = 0x266;         // Bullfrog sounds
            Hue = UniqueHue;

            // Attributes
            SetStr(400, 450);
            SetDex(100, 120);
            SetInt(200, 240);

            SetHits(600, 650);
            SetMana(300, 350);

            SetDamage(20, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   80, 90);
            SetResistance(ResistanceType.Energy,   30, 40);

            // Skills
            SetSkill(SkillName.EvalInt,    80.0, 100.0);
            SetSkill(SkillName.Magery,     75.0,  95.0);
            SetSkill(SkillName.MagicResist,100.0, 120.0);
            SetSkill(SkillName.Tactics,   100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 70;

            Tamable = false;

            // Initialize cooldowns so it can use abilities immediately
            var now = DateTime.UtcNow;
            m_NextTongueWhip   = now;
            m_NextVenomSpray   = now;
            m_NextQuagmireSlam = now;
            m_NextThunderCroak = now;
            m_NextSummonSpawn  = now;
        }

        public DreadBullFrog(Serial serial)
            : base(serial)
        {
        }

        // Drops
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            PackItem(new Quillfang()); // Rare trophy item
        }

        // Hide, meat, poison properties
        public override HideType HideType => HideType.Barbed;
        public override int Hides => 10;
        public override int Meat => 5;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Poison.Deadly;

        // AI Think loop
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target && target.Map == Map)
            {
                var now = DateTime.UtcNow;

                if (now >= m_NextTongueWhip && InRange(target, 10))
                {
                    TongueWhipAttack(target);
                }
                else if (now >= m_NextVenomSpray && InRange(target, 12))
                {
                    VenomSprayCone(target);
                }
                else if (now >= m_NextQuagmireSlam && Utility.InRange(Location, target.Location, 5))
                {
                    QuagmireSlam();
                }
                else if (now >= m_NextThunderCroak)
                {
                    ThunderCroak();
                }
                else if (now >= m_NextSummonSpawn)
                {
                    SummonSpawnlings();
                }
            }
        }

        // --- Special Abilities ---

        // 1) Tongue Whip: a straight-line physical/force attack
        public void TongueWhipAttack(Mobile target)
        {
            var map = Map;
            if (map == null) return;

            const int range = 10, damage = 25;
            var dir = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(dir, ref dx, ref dy);

            // Collect line of tiles
            var lineTiles = new List<Point3D>();
            for (int i = 1; i <= range; i++)
            {
                var p = new Point3D(X + dx * i, Y + dy * i, Z);
                if (!map.CanFit(p, 16, false, false))
                {
                    var z2 = map.GetAverageZ(p.X, p.Y);
                    p = new Point3D(p.X, p.Y, z2);
                    if (!map.CanFit(p, 16, false, false))
                        break;
                }
                lineTiles.Add(p);
            }

            // Effects
            Effects.PlaySound(Location, map, 0x627);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x3728, 8, 8, UniqueHue, 0, 2023, 0);

            // Damage along the line
            foreach (var p in lineTiles)
            {
                Effects.SendLocationEffect(p, map, 0x3818, 16, UniqueHue, 0);
                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                    }
                }
            }

            m_NextTongueWhip = DateTime.UtcNow.AddSeconds(8);
        }

        // 2) Venom Spray: a cone of poison, spawns PoisonTile
        public void VenomSprayCone(Mobile target)
        {
            var map = Map;
            if (map == null) return;

            const int coneRange = 8, coneWidth = 4, damage = 20;
            var dir = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(dir, ref dx, ref dy);

            Effects.PlaySound(Location, map, BaseSoundID);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x370A, 10, 30, UniqueHue, 0, 5052, 0);

            var coneTiles = new List<Point3D>();
            for (int i = 1; i <= coneRange; i++)
            {
                int spread = (i * coneWidth) / coneRange;
                for (int j = -spread; j <= spread; j++)
                {
                    int tx = X + dx * i, ty = Y + dy * i;
                    if (dx == 0) tx += j;
                    else if (dy == 0) ty += j;
                    else
                    {
                        tx += j;
                        ty += (dx * dy > 0) ? -j : j;
                    }

                    var p = new Point3D(tx, ty, Z);
                    if (!map.CanFit(p, 16, false, false))
                    {
                        var z2 = map.GetAverageZ(tx, ty);
                        p = new Point3D(tx, ty, z2);
                    }
                    coneTiles.Add(p);
                }
            }

            foreach (var p in coneTiles)
            {
                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                              0x36B0, 8, 8, UniqueHue, 0, 2023, 0);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                        // Spawn poison ground tile
                        if (Utility.RandomDouble() < 0.3)
                        {
                            PoisonTile tile = new PoisonTile();
                            tile.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextVenomSpray = DateTime.UtcNow.AddSeconds(12);
        }

        // 3) Quagmire Slam: radial slam spawning QuicksandTile
        public void QuagmireSlam()
        {
            var map = Map;
            if (map == null) return;

            const int radius = 5, damage = 30;
            Effects.PlaySound(Location, map, 0x2F3);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x3709, 10, 30, UniqueHue, 0, 5029, 0);

            for (int xOff = -radius; xOff <= radius; xOff++)
            {
                for (int yOff = -radius; yOff <= radius; yOff++)
                {
                    var p = new Point3D(X + xOff, Y + yOff, Z);
                    if (Utility.InRange(Location, p, radius))
                    {
                        Effects.SendLocationEffect(p, map, 0x3709, 16, UniqueHue, 0);
                        foreach (var m in map.GetMobilesInRange(p, 0))
                        {
                            if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                            {
                                DoHarmful(m);
                                AOS.Damage(m, this, damage, 100, 0, 0, 0, 0);
                                m.Paralyze(TimeSpan.FromSeconds(2.0));
                            }
                        }
                        if (Utility.RandomDouble() < 0.4)
                        {
                            QuicksandTile qs = new QuicksandTile();
                            qs.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextQuagmireSlam = DateTime.UtcNow.AddSeconds(15);
        }

        // 4) Thunder Croak: wide-area lightning with stun & VortexTile
        public void ThunderCroak()
        {
            var map = Map;
            if (map == null) return;

            const int radius = 8, damage = 20;
            Effects.PlaySound(Location, map, 0x5BB);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                                          0x37CB, 1, 10, UniqueHue, 0, 9909, 0);

            for (int xOff = -radius; xOff <= radius; xOff++)
            {
                for (int yOff = -radius; yOff <= radius; yOff++)
                {
                    var p = new Point3D(X + xOff, Y + yOff, Z);
                    if (Utility.InRange(Location, p, radius))
                    {
                        Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration),
                                                      0x36F4, 8, 8, UniqueHue, 0, 2023, 0);
                        foreach (var m in map.GetMobilesInRange(p, 0))
                        {
                            if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                            {
                                DoHarmful(m);
                                AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                                if (Utility.RandomBool())
                                    m.Freeze(TimeSpan.FromSeconds(1.5));
                            }
                        }
                        if (Utility.RandomDouble() < 0.2)
                        {
                            VortexTile vt = new VortexTile();
                            vt.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextThunderCroak = DateTime.UtcNow.AddSeconds(20);
        }

        // 5) Summon Spawnlings: calls small bullfrogs to aid
        public void SummonSpawnlings()
        {
            var map = Map;
            if (map == null) return;

            int count = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < count; i++)
            {
                var xOff = Utility.RandomMinMax(-3, 3);
                var yOff = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + xOff, Y + yOff, Z);
                var spawn = new BullFrog(); // base bullfrog
                spawn.MoveToWorld(loc, map);
                spawn.Combatant = Combatant;
            }

            Effects.PlaySound(Location, map, 0x557);
            m_NextSummonSpawn = DateTime.UtcNow.AddSeconds(30);
        }

        // On death: final poisonous eruption
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Effects.PlaySound(Location, Map, 0x214);
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                              0x3709, 10, 60, UniqueHue, 0, 5052, 0);

                for (int i = 0; i < 15; i++)
                {
                    var p = GetRandomLocation(Location, 5, Map);
                    if (p != Point3D.Zero)
                    {
                        PoisonTile pt = new PoisonTile();
                        pt.MoveToWorld(p, Map);
                    }
                }
            }
            base.OnDeath(c);
        }

        // Helper: random valid ground location
        private Point3D GetRandomLocation(Point3D center, int radius, Map map)
        {
            for (int i = 0; i < 10; i++)
            {
                var xOff = Utility.RandomMinMax(-radius, radius);
                var yOff = Utility.RandomMinMax(-radius, radius);
                var p = new Point3D(center.X + xOff, center.Y + yOff, center.Z);
                if (Utility.InRange(center, p, radius) && map.CanFit(p, 16, false, false))
                    return p;

                var z2 = map.GetAverageZ(p.X, p.Y);
                var p2 = new Point3D(p.X, p.Y, z2);
                if (Utility.InRange(center, p2, radius) && map.CanFit(p2, 16, false, false))
                    return p2;
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
