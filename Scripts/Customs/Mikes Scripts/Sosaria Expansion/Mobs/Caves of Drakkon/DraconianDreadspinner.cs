using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a draconian dreadspinner corpse")]
    public class DraconianDreadspinner : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextArcaneWebBloom;
        private DateTime m_NextDraconicWebBreath;
        private DateTime m_NextVenomousSpiral;
        private DateTime m_NextSonicScreech;
        private DateTime m_NextGrasp;

        // Unique hue (deep violet)
        private const int UniqueHue = 1254;

        [Constructable]
        public DraconianDreadspinner()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "Draconian Dreadspinner";
            Body = 11;               // same as DreadSpider
            BaseSoundID = 1170;      // same as DreadSpider
            Hue = UniqueHue;

            // Stats
            SetStr(500, 600);
            SetDex(200, 300);
            SetInt(400, 500);

            SetHits(400, 500);
            SetDamage(20, 30);

            // Damage types
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Poison, 70);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 40, 50);

            // Skills
            SetSkill(SkillName.EvalInt, 80.0, 100.0);
            SetSkill(SkillName.Magery, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);
            SetSkill(SkillName.Poisoning, 100.0);
            SetSkill(SkillName.DetectHidden, 80.0);
            SetSkill(SkillName.Necromancy, 40.0);
            SetSkill(SkillName.SpiritSpeak, 40.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;

            Tamable = false;
            ControlSlots = 0;

            // Initialize cooldowns
            m_NextArcaneWebBloom    = DateTime.UtcNow;
            m_NextDraconicWebBreath = DateTime.UtcNow;
            m_NextVenomousSpiral    = DateTime.UtcNow;
            m_NextSonicScreech      = DateTime.UtcNow;
            m_NextGrasp             = DateTime.UtcNow;
        }

        public DraconianDreadspinner(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => true;
        public override Poison PoisonImmune  => Poison.Lethal;
        public override Poison HitPoison     => Poison.Lethal;
        public override int   TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,       5);

            if (Utility.RandomDouble() < 0.02) // 2% chance
                PackItem(new SpidersSilk(10));
        }

        public override int GetIdleSound() { return BaseSoundID; }
        public override int GetHurtSound() { return 0x1171; } // slight variation

        // --- Special Abilities ---

        // 1) Arcane Web Bloom: spawns a ring of trap webs in radius 5
        public void ArcaneWebBloom()
        {
            var map = this.Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x2E4);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration), 0x370A, 8, 30, UniqueHue, 0, 5032, 0);

            int radius = 5;
            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                var p = new Point3D(X + x, Y + y, Z);
                if (Utility.InRange(Location, p, radius) && map.CanFit(p, 16, false, false))
                {
                    // spawn a ring (only at approximate distance)
                    double dist = Math.Sqrt(x * x + y * y);
                    if (dist >= radius - 1 && dist <= radius + 0.5)
                    {
                        var tile = new TrapWeb();
                        tile.MoveToWorld(p, map);
                    }
                }
            }

            m_NextArcaneWebBloom = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 2) Draconic Web Breath: cone of sticky webs and poison
        public void DraconicWebBreath()
        {
            var map = this.Map;
            if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            int coneRange = 8, coneWidth = 4, damage = 25;
            Effects.PlaySound(Location, map, BaseSoundID);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration), 0x3728, 10, 30, UniqueHue, 0, 2025, 0);

            Direction d = GetDirectionTo(target);
            int dx = 0, dy = 0;
            Movement.Movement.Offset(d, ref dx, ref dy);

            var coneTiles = new List<Point3D>();
            for (int i = 1; i <= coneRange; i++)
            {
                int spread = (int)(coneWidth * (i / (double)coneRange));
                for (int j = -spread; j <= spread; j++)
                {
                    int tx = X + i * dx, ty = Y + i * dy;
                    if (dx == 0) tx += j;
                    else if (dy == 0) ty += j;
                    else { tx += j; ty += j; }

                    var p = new Point3D(tx, ty, Z);
                    if (!map.CanFit(p, 16, false, false))
                    {
                        int z2 = map.GetAverageZ(tx, ty);
                        p = new Point3D(tx, ty, z2);
                    }
                    coneTiles.Add(p);
                }
            }

            foreach (var p in coneTiles)
            {
                Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration), 0x36B0, 8, 8, UniqueHue, 0, 2026, 0);
                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || m is BaseCreature && ((BaseCreature)m).Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                        if (m is Mobile mm) mm.ApplyPoison(this, Poison.Deadly);
                    }
                }
            }

            m_NextDraconicWebBreath = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 3) Venomous Spiral: swirling poison-tile spiral
        public void VenomousSpiral()
        {
            var map = this.Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x22F);
            int radius = 6, points = radius * 8, damage = 30;
            var spiral = new List<Point3D>();

            for (int i = 0; i < points; i++)
            {
                double angle = i * (2 * Math.PI / points);
                double r = radius * (i / (double)points);
                int dx = (int)Math.Round(r * Math.Cos(angle));
                int dy = (int)Math.Round(r * Math.Sin(angle));
                var p = new Point3D(X + dx, Y + dy, Z);
                if (Utility.InRange(Location, p, radius) && map.CanFit(p, 16, false, false) && !spiral.Contains(p))
                    spiral.Add(p);
            }

            foreach (var p in spiral)
            {
                var tile = new PoisonTile();
                tile.MoveToWorld(p, map);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || m is BaseCreature && ((BaseCreature)m).Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                    }
                }
            }

            m_NextVenomousSpiral = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // 4) Terrifying Screech: donut-shaped mana & stam drain
        public void TerrifyingScreech()
        {
            var map = this.Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x213);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration), 0x37C4, 1, 12, UniqueHue, 0, 9909, 0);

            int inner = 3, outer = 6, drain = 50;
            for (int x = -outer; x <= outer; x++)
            for (int y = -outer; y <= outer; y++)
            {
                double dist = Math.Sqrt(x * x + y * y);
                if (dist >= inner && dist <= outer)
                {
                    var p = new Point3D(X + x, Y + y, Z);
                    if (!map.CanFit(p, 16, false, false))
                    {
                        int z2 = map.GetAverageZ(p.X, p.Y);
                        p = new Point3D(p.X, p.Y, z2);
                    }

                    Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration), 0x3709, 6, 20, UniqueHue, 0, 5033, 0);
                    foreach (var m in map.GetMobilesInRange(p, 0))
                    {
                        if (m != this && m is Mobile mm && (mm is PlayerMobile || mm is BaseCreature && ((BaseCreature)mm).Team != Team))
                        {
                            DoHarmful(mm);
                            mm.Mana        = Math.Max(0, mm.Mana - drain);
                            mm.Stam        = Math.Max(0, mm.Stam - drain);
                            var tile = new ManaDrainTile();
                            tile.MoveToWorld(p, map);
                        }
                    }
                }
            }

            m_NextSonicScreech = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        // 5) Dreadspinner's Grasp: roots target in a 3×3 web
        public void DreadspinnersGrasp()
        {
            var map = this.Map;
            if (map == null || Combatant == null) return;
            if (!(Combatant is Mobile target)) return;

            Effects.PlaySound(target.Location, map, 0x1F7);
            Effects.SendLocationParticles(EffectItem.Create(target.Location, map, EffectItem.DefaultDuration), 0x3709, 8, 30, UniqueHue, 0, 5016, 0);

            for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                var p = new Point3D(target.X + x, target.Y + y, target.Z);
                if (!map.CanFit(p, 16, false, false))
                {
                    int z2 = map.GetAverageZ(p.X, p.Y);
                    p = new Point3D(p.X, p.Y, z2);
                }

                var web = new TrapWeb();
                web.MoveToWorld(p, map);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || m is BaseCreature && ((BaseCreature)m).Team != Team))
                    {
                        DoHarmful(m);
                        m.SendMessage("You are ensnared by Draconian dreadspinner's webs!");
                        m.Paralyze(TimeSpan.FromSeconds(4));
                    }
                }
            }

            m_NextGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && Combatant is Mobile target)
            {
                // Use the highest priority ability whose cooldown is up
                if (DateTime.UtcNow >= m_NextDraconicWebBreath && InRange(target, 8))
                    DraconicWebBreath();
                else if (DateTime.UtcNow >= m_NextVenomousSpiral && InRange(target, 12))
                    VenomousSpiral();
                else if (DateTime.UtcNow >= m_NextArcaneWebBloom)
                    ArcaneWebBloom();
                else if (DateTime.UtcNow >= m_NextSonicScreech && InRange(target, 10))
                    TerrifyingScreech();
                else if (DateTime.UtcNow >= m_NextGrasp && InRange(target, 6))
                    DreadspinnersGrasp();
            }
        }

        // --- OnDeath Effect ---
        public override void OnDeath(Container c)
        {
            var map = this.Map;
            if (map == null)
            {
                base.OnDeath(c);
                return;
            }

            Effects.PlaySound(Location, map, 0x2A4);
            Effects.SendLocationParticles(EffectItem.Create(Location, map, EffectItem.DefaultDuration), 0x3709, 10, 50, UniqueHue, 0, 5054, 0);

            int count = 15, radius = 5;
            for (int i = 0; i < count; i++)
            {
                var p = GetRandomValidLocation(Location, radius, map);
                if (p != Point3D.Zero)
                {
                    var tile = Utility.RandomBool() ? (Item)new LandmineTile() : new VortexTile();
                    tile.MoveToWorld(p, map);
                    Effects.SendLocationParticles(EffectItem.Create(p, map, EffectItem.DefaultDuration), 0x3709, 6, 20, UniqueHue, 0, 5018, 0);
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
                var p = new Point3D(center.X + dx, center.Y + dy, center.Z);
                if (map.CanFit(p, 16, false, false) && Utility.InRange(center, p, radius))
                    return p;

                int z2 = map.GetAverageZ(p.X, p.Y);
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
