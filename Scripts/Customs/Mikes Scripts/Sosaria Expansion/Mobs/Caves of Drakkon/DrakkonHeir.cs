using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a drakkon heir corpse")]
    public class DrakkonHeir : BaseCreature
    {
        private DateTime m_NextBreath;
        private DateTime m_NextVortex;
        private DateTime m_NextLightning;
        private DateTime m_NextPoisonBloom;
        private DateTime m_NextNecroFlame;

        [Constructable]
        public DrakkonHeir() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Drakkon Heir";
            Body = 46;
            BaseSoundID = 362;
            Hue = 1155;              // Unique fiery‐arcane hue


            SetStr(1200, 1300);
            SetDex(150, 200);
            SetInt(900, 1000);

            SetHits(800, 900);
            SetDamage(35, 45);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire,     30);
            SetDamageType(ResistanceType.Energy,   20);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire,     85, 95);
            SetResistance(ResistanceType.Cold,     75, 85);
            SetResistance(ResistanceType.Poison,   65, 75);
            SetResistance(ResistanceType.Energy,   80, 90);

            SetSkill(SkillName.EvalInt,    100.1, 120.0);
            SetSkill(SkillName.Magery,     100.1, 120.0);
            SetSkill(SkillName.MagicResist,120.5, 140.0);
            SetSkill(SkillName.Tactics,    100.0, 110.0);
            SetSkill(SkillName.Wrestling,  100.0, 110.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 80;

            // Initialize all timers to now
            m_NextBreath      = DateTime.UtcNow;
            m_NextVortex      = DateTime.UtcNow;
            m_NextLightning   = DateTime.UtcNow;
            m_NextPoisonBloom = DateTime.UtcNow;
            m_NextNecroFlame  = DateTime.UtcNow;
        }

        public override void OnThink()
        {
            base.OnThink();

            var now = DateTime.UtcNow;
            var map = this.Map;
            if (map == null || Combatant == null) return;

            if (now >= m_NextBreath)
            {
                BreathArcAttack();
                m_NextBreath = now + TimeSpan.FromSeconds(10);
            }

            if (now >= m_NextVortex)
            {
                VortexMaelstrom();
                m_NextVortex = now + TimeSpan.FromSeconds(18);
            }

            if (now >= m_NextLightning)
            {
                LightningStorm();
                m_NextLightning = now + TimeSpan.FromSeconds(14);
            }

            if (now >= m_NextPoisonBloom)
            {
                PoisonBloom();
                m_NextPoisonBloom = now + TimeSpan.FromSeconds(12);
            }

            if (now >= m_NextNecroFlame)
            {
                NecroFlamestrike();
                m_NextNecroFlame = now + TimeSpan.FromSeconds(20);
            }
        }

        // 1) Fan‑shaped fire breath in front of the heir
        private void BreathArcAttack()
        {
            if (this.Map == null) return;
            Direction d = this.Direction;
            int range = 12, damage = 40, hue = this.Hue;
            List<Point3D> area = new List<Point3D>();

            // Compute base direction vector
            int dx = 0, dy = 0;
            switch (d & Direction.Mask)
            {
                case Direction.North: dy = -1; break;
                case Direction.South: dy =  1; break;
                case Direction.West:  dx = -1; break;
                case Direction.East:  dx =  1; break;
                // intercards
                case Direction.Up:    dx = -1; dy = -1; break;
                case Direction.Right: dx =  1; dy = -1; break;
                case Direction.Down:  dx =  1; dy =  1; break;
                case Direction.Left:  dx = -1; dy =  1; break;
            }

            for (int i = 1; i <= range; i++)
            {
                // triangular width
                int width = i / 3 + 1;
                int baseX = this.X + dx * i;
                int baseY = this.Y + dy * i;

                for (int off = -width; off <= width; off++)
                {
                    int tx = baseX, ty = baseY;
                    if (dx == 0) tx += off;
                    else if (dy == 0) ty += off;
                    else if (dx * dy > 0) { tx += off; ty -= off; }
                    else { tx += off; ty += off; }

                    int tz = this.Z;
                    if (!Map.CanFit(tx, ty, tz, 16, false, false))
                    {
                        tz = Map.GetAverageZ(tx, ty);
                        if (!Map.CanFit(tx, ty, tz, 16, false, false))
                            continue;
                    }

                    area.Add(new Point3D(tx, ty, tz));
                }
            }

            foreach (var p in area)
            {
                Effects.SendLocationEffect(p, this.Map, 0x36BD, 16, 10, hue, 0);
                foreach (var m in this.Map.GetMobilesInRange(p, 0))
                {
                    if (m is PlayerMobile || m is BaseCreature)
                        m.Damage(damage, this);
                }
            }
        }

        // 2) Central vortex that pulls in and damages nearby foes
        private void VortexMaelstrom()
        {
            if (this.Map == null) return;
            var map = this.Map;
            var center = this.Location;
            int radius = 6, damage = 20, hue = this.Hue;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (x*x + y*y > radius*radius) continue;
                    var p = new Point3D(center.X + x, center.Y + y, center.Z);
                    Effects.SendLocationParticles(
                        EffectItem.Create(p, map, EffectItem.DefaultDuration),
                        0x376A, 8, 20, hue, 0, 5021, 0);

                    foreach (var m in map.GetMobilesInRange(p, 0))
                    {
                        if (m is Mobile mb && mb != this && mb.Alive)
                        {
                            // pull 1 tile closer
                            int mx = center.X - mb.X;
                            int my = center.Y - mb.Y;
                            mb.MoveToWorld(new Point3D(mb.X + Math.Sign(mx), mb.Y + Math.Sign(my), mb.Z), map);
                            mb.Damage(damage, this);
                        }
                    }
                }
            }
        }

        // 3) Random lightning strikes around combatant
        private void LightningStorm()
        {
            if (!(Combatant is Mobile target) || this.Map == null) return;
            var map = this.Map;
            int strikes = 6, damage = 25;

            for (int i = 0; i < strikes; i++)
            {
                int rx = Utility.RandomMinMax(-8, 8);
                int ry = Utility.RandomMinMax(-8, 8);
                var p = new Point3D(target.X + rx, target.Y + ry, target.Z);



                foreach (var m in map.GetMobilesInRange(p, 0))
                {
                    if (m is PlayerMobile || m is BaseCreature)
                        m.Damage(damage, this);
                }
            }
        }

        // 4) Poison tiles bloom in a ring around Drakkon Heir
        private void PoisonBloom()
        {
            if (this.Map == null) return;
            var map = this.Map;
            var center = this.Location;
            int radius = 5;

            for (int angle = 0; angle < 360; angle += 30)
            {
                double rad = angle * Math.PI / 180;
                int x = center.X + (int)(radius * Math.Cos(rad));
                int y = center.Y + (int)(radius * Math.Sin(rad));
                var p = new Point3D(x, y, center.Z);

                if (!map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                    p.Z = map.GetAverageZ(p.X, p.Y);

                // spawn a poison hazard tile
                var tile = new PoisonTile();
                tile.Hue = this.Hue;
                tile.MoveToWorld(p, map);

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x373A, 12, 30, this.Hue, 0, 2032, 0);
            }
        }

        // 5) Necromantic flame pillars
        private void NecroFlamestrike()
        {
            if (!(Combatant is Mobile target) || this.Map == null) return;
            var map = this.Map;
            var p = target.Location;

            // scatter 4 pillars around the target
            foreach (var off in new[]{ new Point3D(2,2,0), new Point3D(-2,2,0), new Point3D(2,-2,0), new Point3D(-2,-2,0) })
            {
                var loc = new Point3D(p.X + off.X, p.Y + off.Y, p.Z);
                if (!map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = map.GetAverageZ(loc.X, loc.Y);

                var tile = new NecromanticFlamestrikeTile();
                tile.Hue = this.Hue;
                tile.MoveToWorld(loc, map);

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, map, EffectItem.DefaultDuration),
                    0x3709, 8, 20, this.Hue, 0, 5050, 0);
            }
        }

        // Death spawns landmines in a wide area
        public override void OnDeath(Container c)
        {
            var map = this.Map;
            if (map == null) { base.OnDeath(c); return; }

            Point3D loc = this.Location;
            int mines = 12;
            for (int i = 0; i < mines; i++)
            {
                int dx = Utility.RandomMinMax(-4, 4);
                int dy = Utility.RandomMinMax(-4, 4);
                var p = new Point3D(loc.X + dx, loc.Y + dy, loc.Z);

                if (!map.CanFit(p.X, p.Y, p.Z, 16, false, false))
                    p.Z = map.GetAverageZ(p.X, p.Y);

                var mine = new LandmineTile();
                mine.Hue = this.Hue;
                mine.MoveToWorld(p, map);

                Effects.SendLocationParticles(
                    EffectItem.Create(p, map, EffectItem.DefaultDuration),
                    0x3728, 10, 30, this.Hue, 0, 2031, 0);
            }

            Effects.PlaySound(loc, map, 0x214);
            base.OnDeath(c);
        }

        public DrakkonHeir(Serial serial) : base(serial) { }

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
