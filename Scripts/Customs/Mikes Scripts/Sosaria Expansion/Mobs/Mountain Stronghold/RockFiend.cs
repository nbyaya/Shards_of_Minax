using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a rock fiend corpse")]
    public class RockFiend : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextTremorTime;
        private DateTime m_NextLavaFlowTime;
        private DateTime m_NextMagnetPulseTime;
        private DateTime m_NextBoulderBarrageTime;

        // Last position for movement‑based hazards
        private Point3D m_LastLocation;

        // A stout gray‑brown hue
        private const int UniqueHue = 1109;

        [Constructable]
        public RockFiend() 
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name            = "a Rock Fiend";
            Body            = 43;
            BaseSoundID     = 357;
            Hue             = UniqueHue;

            SetStr(700, 800);
            SetDex(200, 250);
            SetInt(150, 200);

            SetHits(2000, 2400);
            SetStam(200, 250);
            SetMana(200, 300);

            SetDamage(20, 25);

            SetDamageType(ResistanceType.Physical, 90);
            SetDamageType(ResistanceType.Fire,     10);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     60, 70);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   30, 40);

            SetSkill(SkillName.Wrestling,   110.1, 120.0);
            SetSkill(SkillName.Tactics,     110.1, 120.0);
            SetSkill(SkillName.MagicResist, 100.2, 110.0);
            SetSkill(SkillName.Anatomy,      90.0, 100.0);

            Fame         = 25000;
            Karma        = -25000;
            VirtualArmor = 90;
            ControlSlots = 6;

            m_NextTremorTime        = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextLavaFlowTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextMagnetPulseTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextBoulderBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            m_LastLocation = this.Location;

            PackItem(new Granite(Utility.RandomMinMax(20, 30)));
            PackItem(new IronOre(Utility.RandomMinMax(30, 40)));
            PackGold(1500, 2000);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                if (Map.CanFit(m_LastLocation, 16, false, false))
                {
                    var mine = new LandmineTile { Hue = UniqueHue };
                    mine.MoveToWorld(m_LastLocation, this.Map);
                }
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            if ( now >= m_NextBoulderBarrageTime && InRange(Combatant.Location, 12) )
            {
                BoulderBarrage();
                m_NextBoulderBarrageTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if ( now >= m_NextMagnetPulseTime && InRange(Combatant.Location, 10) )
            {
                MagnetPulse();
                m_NextMagnetPulseTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
            }
            else if ( now >= m_NextLavaFlowTime && InRange(Combatant.Location, 8) )
            {
                LavaFlow();
                m_NextLavaFlowTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            else if ( now >= m_NextTremorTime && InRange(Combatant.Location, 6) )
            {
                Tremor();
                m_NextTremorTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
        }

        // —— Ability: Massive Earthquake —— 
        public void Tremor()
        {
            Say("*The ground shatters!*");
            PlaySound(0x11D);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x376A, 20, 60, UniqueHue, 0, 5039, 0);

            var eq = new EarthquakeTile { Hue = UniqueHue };
            eq.MoveToWorld(this.Location, this.Map);
        }

        // —— Ability: Molten Vein —— 
        public void LavaFlow()
        {
            Say("*Feel the molten wrath!*");
            PlaySound(0x208);

            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                // Bresenham line — up to 6 steps toward the target
                var path = new List<Point3D>();
                int x0 = X, y0 = Y, x1 = target.X, y1 = target.Y;
                int dx = Math.Abs(x1 - x0), dy = Math.Abs(y1 - y0);
                int sx = x0 < x1 ? 1 : -1, sy = y0 < y1 ? 1 : -1;
                int err = dx - dy;
                int steps = 0;

                while (steps < 6)
                {
                    int z = Map.GetAverageZ(x0, y0);
                    path.Add(new Point3D(x0, y0, z));
                    if (x0 == x1 && y0 == y1)
                        break;

                    int e2 = err * 2;
                    if (e2 > -dy) { err -= dy; x0 += sx; }
                    if (e2 <  dx) { err += dx; y0 += sy; }
                    steps++;
                }

                foreach (Point3D pt in path)
                {
                    if (!Map.CanFit(pt, 16, false, false))
                        continue;

                    var lava = new HotLavaTile { Hue = UniqueHue };
                    lava.MoveToWorld(pt, this.Map);
                }
            }
        }

        // —— Ability: Magnetic Crushing Pulse —— 
        public void MagnetPulse()
        {
            Say("*The earth’s pull overpowers you!*");
            PlaySound(0x5C0);

            var mag = new MagnetTile { Hue = UniqueHue };
            mag.MoveToWorld(this.Location, this.Map);

            foreach (var m in Map.GetMobilesInRange(Location, 8))
            {
                if (m != this && CanBeHarmful(m, false))
                {
                    DoHarmful(m);
                    m.SendMessage("You are yanked toward the Rock Fiend!");
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 30), 100, 0, 0, 0, 0);

                    int dx = Math.Sign(X - m.X);
                    int dy = Math.Sign(Y - m.Y);
                    int nx = m.X + dx;
                    int ny = m.Y + dy;
                    int nz = Map.GetAverageZ(nx, ny);

                    // use a fixed height of 16 since Mobile.Height doesn't exist
                    if (Map.CanFit(nx, ny, nz, 16, false, false))
                        m.Location = new Point3D(nx, ny, nz);
                }
            }
        }

        // —— Ability: Boulder Barrage —— 
        public void BoulderBarrage()
        {
            Say("*Crush them under rock!*");
            PlaySound(0x2D6);

            var initial = Combatant as Mobile;
            if (initial == null || !CanBeHarmful(initial, false))
                return;

            var targets = new List<Mobile> { initial };
            int maxRange = 10, maxTargets = 4;

            foreach (var m in Map.GetMobilesInRange(initial.Location, maxRange))
            {
                if (targets.Count >= maxTargets) break;
                if (m != this && m != initial && CanBeHarmful(m, false))
                    targets.Add(m);
            }

            for (int i = 0; i < targets.Count; i++)
            {
                var src = (i == 0 ? this : targets[i - 1]);
                var dst = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x36D4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0x100);

                int dmg = Utility.RandomMinMax(25, 40);
                Timer.DelayCall(TimeSpan.FromSeconds(0.1 * i), () =>
                {
                    if (CanBeHarmful(dst, false))
                    {
                        DoHarmful(dst);
                        AOS.Damage(dst, this, dmg, 100, 0, 0, 0, 0);
                    }
                });
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

			if (this.Map == null)
				return;

            Say("*The mountain reclaims you!*");
            PlaySound(0x11E);

            for (int i = 0; i < 6; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Map.GetAverageZ(x, y);

                if (!Map.CanFit(x, y, z, 16, false, false))
                    continue;

                var qs = new QuicksandTile { Hue = UniqueHue };
                qs.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        public override bool BleedImmune    => true;
        public override Poison PoisonImmune => Poison.Greater;

        public override int TreasureMapLevel   => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus      => 80.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,        Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 3));
        }

        public RockFiend(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            var now = DateTime.UtcNow;
            m_NextTremorTime        = now + TimeSpan.FromSeconds(10);
            m_NextLavaFlowTime      = now + TimeSpan.FromSeconds(15);
            m_NextMagnetPulseTime   = now + TimeSpan.FromSeconds(20);
            m_NextBoulderBarrageTime = now + TimeSpan.FromSeconds(25);
        }
    }
}
