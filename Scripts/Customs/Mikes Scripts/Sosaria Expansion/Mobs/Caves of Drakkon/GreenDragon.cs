using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a green dragon corpse")]
    public class GreenDragon : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextEmeraldBreath;
        private DateTime m_NextSporeCloud;
        private DateTime m_NextRootLine;
        private DateTime m_NextToxicBarrage;
        private DateTime m_NextVerdantWard;

        // Unique emerald hue
        private const int EmeraldHue = 1153;

        [Constructable]
        public GreenDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.3, 0.6)
        {
            Name = "a green dragon";
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;
            Hue = EmeraldHue;

            // Stats
            SetStr(1500, 1800);
            SetDex(120, 180);
            SetInt(600, 800);

            SetHits(1200, 1600);
            SetDamage(30, 40);

            // Damage types
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison, 60);

            // Resistances
            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 80, 100);
            SetResistance(ResistanceType.Energy, 50, 70);

            // Skills
            SetSkill(SkillName.Magery, 110.0, 130.0);
            SetSkill(SkillName.EvalInt, 110.0, 130.0);
            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 25000;
            Karma = -20000;
            VirtualArmor = 85;

            // Cooldowns start now
            m_NextEmeraldBreath = m_NextSporeCloud = m_NextRootLine =
            m_NextToxicBarrage = m_NextVerdantWard = DateTime.UtcNow;
        }

        public GreenDragon(Serial serial)
            : base(serial)
        {
        }

        // --- Properties ---
        public override bool AutoDispel => true;
        public override bool ReacquireOnMovement => true;
        public override bool CanFly => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Poison.Deadly;
        public override int TreasureMapLevel => 6;
        public override int Hides => 40;
        public override int Meat => 20;
        public override int Scales => 12;
        public override ScaleType ScaleType => ScaleType.Green;

        // --- Loot ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.Gems, 6);
            if (Utility.RandomDouble() < 0.02) // 2% for a special green scale chest
                ; // PackItem(new GreenDragonScaleChestplate());
        }

        // --- Special Abilities ---

        // 1) Emerald Breath: A wide cone of toxic gas
        public void EmeraldBreath()
        {
            if (!(Combatant is Mobile target)) return;
            var map = Map;
            if (map == null || !this.InRange(target, 10)) return;

            Effects.PlaySound(Location, map, 0x227);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3709, 20, 30, EmeraldHue, 0, 5023, 0);

            // Cone parameters
            int range = 8, width = 4;
            Direction d = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);

            var coneTiles = new List<Point3D>();
            for (int i = 1; i <= range; i++)
            {
                int halfWidth = (i * width) / range;
                for (int j = -halfWidth; j <= halfWidth; j++)
                {
                    int x = X + dx * i + (dy == 0 ? j : 0);
                    int y = Y + dy * i + (dx == 0 ? j : 0);
                    var p = new Point3D(x, y, Z);
                    if (map.CanFit(p, 16, false, false))
                        coneTiles.Add(p);
                }
            }

            foreach (var p in coneTiles)
            {
                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x36B0, 10, 10, EmeraldHue, 0, 2031, 0);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 25, 0, 0, 100, 0, 0); // poison
                        m.ApplyPoison(this, Poison.Deadly);
                    }
                }
            }

            m_NextEmeraldBreath = DateTime.UtcNow + TimeSpan.FromSeconds(12);
        }

        // 2) Spore Cloud: Circular ring of PoisonTiles
        public void SporeCloud()
        {
            var map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x207);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x3728, 8, 20, EmeraldHue, 0, 5050, 0);

            int radius = 6;
            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                if (Utility.InRange(Location, new Point3D(X + x, Y + y, Z), radius))
                {
                    var p = new Point3D(X + x, Y + y, Z);
                    if (!map.CanFit(p, 16, false, false))
                        p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                    // Spawn PoisonTile
                    // var tile = new PoisonTile();
                    // tile.MoveToWorld(p, map);

                    foreach (var m in map.GetMobilesInRange(p, 0))
                    {
                        if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, 20, 0, 0, 100, 0, 0);
                        }
                    }
                }
            }

            m_NextSporeCloud = DateTime.UtcNow + TimeSpan.FromSeconds(18);
        }

        // 3) Root Entangle: Straight-line root & damage
        public void RootEntangleLine()
        {
            if (!(Combatant is Mobile target)) return;
            var map = Map;
            if (map == null || !Utility.InRange(Location, target.Location, 12)) return;

            Effects.PlaySound(Location, map, 0x249);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x379A, 12, 15, EmeraldHue, 0, 2015, 0);

            Direction d = GetDirectionTo(target);
			int dx = 0, dy = 0;
			Movement.Movement.Offset(d, ref dx, ref dy);

            for (int i = 1; i <= 12; i++)
            {
                var p = new Point3D(X + dx * i, Y + dy * i, Z);
                if (!map.CanFit(p, 16, false, false))
                    p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x376A, 8, 10, EmeraldHue, 0, 2033, 0);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 30, 100, 0, 0, 0, 0);
                        // m.Freeze(TimeSpan.FromSeconds(2)); // optional root
                    }
                }
            }

            m_NextRootLine = DateTime.UtcNow + TimeSpan.FromSeconds(10);
        }

        // 4) Toxic Barrage: Random splash of ToxicGasTiles
        public void ToxicBarrage()
        {
            var map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x21C);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37C9, 6, 25, EmeraldHue, 0, 5060, 0);

            for (int i = 0; i < 15; i++)
            {
                int rx = Utility.RandomMinMax(-8, 8), ry = Utility.RandomMinMax(-8, 8);
                var p = new Point3D(X + rx, Y + ry, Z);
                if (!map.CanFit(p, 16, false, false))
                    p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                // var gas = new ToxicGasTile();
                // gas.MoveToWorld(p, map);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m != this && (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team)))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 15, 0, 0, 100, 0, 0);
                    }
                }
            }

            m_NextToxicBarrage = DateTime.UtcNow + TimeSpan.FromSeconds(14);
        }

        // 5) Verdant Ward: Self-centered healing/damaging zone
        public void VerdantWard()
        {
            var map = Map;
            if (map == null) return;

            Effects.PlaySound(Location, map, 0x1F1);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                0x37AA, 4, 12, EmeraldHue, 0, 9901, 0);

            int radius = 5;
            for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
            {
                if (!Utility.InRange(Location, new Point3D(X + x, Y + y, Z), radius)) continue;
                var p = new Point3D(X + x, Y + y, Z);
                if (!map.CanFit(p, 16, false, false))
                    p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x37BA, 2, 8, EmeraldHue, 0, 9910, 0);

                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m == this) continue;
                    if (Utility.InRange(Location, p, 1))
                    {
                        // heal allies (including self)
                        if (m.Hits < m.HitsMax)
                            m.Heal(10);
                    }
                    else if (m is PlayerMobile || (m is BaseCreature bc && bc.Team != Team))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, 20, 0, 100, 0, 0, 0);
                    }
                }
            }

            m_NextVerdantWard = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        // --- AI Logic ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile)
            {
                var now = DateTime.UtcNow;

                if (now >= m_NextEmeraldBreath)       EmeraldBreath();
                else if (now >= m_NextSporeCloud)     SporeCloud();
                else if (now >= m_NextRootLine)       RootEntangleLine();
                else if (now >= m_NextToxicBarrage)   ToxicBarrage();
                else if (now >= m_NextVerdantWard)    VerdantWard();
            }
        }

        // --- Death Explosion ---
        public override void OnDeath(Container c)
        {
            var map = Map;
            if (map != null)
            {
                Effects.PlaySound(Location, map, 0x214);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, map, EffectItem.DefaultDuration),
                    0x3709, 12, 60, EmeraldHue, 0, 5055, 0);

                // Scatter PoisonTiles on death
                for (int i = 0; i < 20; i++)
                {
                    int rx = Utility.RandomMinMax(-6, 6),
                        ry = Utility.RandomMinMax(-6, 6);
                    var p = new Point3D(X + rx, Y + ry, Z);
                    if (!map.CanFit(p, 16, false, false))
                        p = new Point3D(p.X, p.Y, map.GetAverageZ(p.X, p.Y));

                    // var tile = new PoisonTile();
                    // tile.MoveToWorld(p, map);
                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x3728, 6, 12, EmeraldHue, 0, 5055, 0);
                }
            }

            base.OnDeath(c);
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
