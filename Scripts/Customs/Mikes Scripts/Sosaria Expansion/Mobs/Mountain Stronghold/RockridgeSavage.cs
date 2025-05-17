using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a rockridge savage corpse")]
    public class RockridgeSavage : BaseCreature
    {
        private DateTime m_NextStompTime;
        private DateTime m_NextRockwaveTime;
        private DateTime m_NextCrushTime;
        private DateTime m_NextRegenerateTime;
        private Point3D m_LastLocation;

        // Unique stone‑gray hue
        private const int UniqueHue = 2217;

        [Constructable]
        public RockridgeSavage()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Rockridge Savage";
            Body = 188;              
            BaseSoundID = 0x3F3;     
            Hue = UniqueHue;

            SetStr(500, 600);
            SetDex(80, 120);
            SetInt(50, 100);

            SetHits(3000, 3500);
            SetStam(200, 300);
            SetMana(100, 150);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Wrestling, 120.0, 130.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 80;
            ControlSlots = 4;

            m_NextStompTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRockwaveTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextCrushTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextRegenerateTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            m_LastLocation       = this.Location;

            PackItem(new Granite(Utility.RandomMinMax(10, 20)));
            PackItem(new Ruby(Utility.RandomMinMax(3, 6)));
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                Point3D dropLoc = m_LastLocation;
                Map map = this.Map;

                if (map != null && map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                {
                    var qs = new QuicksandTile();
                    qs.Hue = UniqueHue;
                    qs.MoveToWorld(dropLoc, map);
                }
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == null || Combatant == null)
                return;

            var target = Combatant as Mobile;
            if (target == null)
                return;

            var now = DateTime.UtcNow;

            // ─── MANUAL DISTANCE CALCULATION ───────────────────────────────
            int dx = this.Location.X - target.Location.X;
            int dy = this.Location.Y - target.Location.Y;
            // Only XY matters for tile range; ignore Z
            int distance = (int)Math.Sqrt(dx * dx + dy * dy);
            // ────────────────────────────────────────────────────────────────

            if (now >= m_NextStompTime && distance <= 2)
            {
                EarthshatterStomp();
                m_NextStompTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            else if (now >= m_NextCrushTime && distance <= 8)
            {
                GravityCrush(target);
                m_NextCrushTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 28));
            }
            else if (now >= m_NextRockwaveTime && distance <= 12)
            {
                RockwaveAttack(target);
                m_NextRockwaveTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }

            if (now >= m_NextRegenerateTime)
            {
                RegenerationBurst();
                m_NextRegenerateTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 60));
            }
        }

        private void EarthshatterStomp()
        {
            PlaySound(0x2A2);
            FixedParticles(0x3709, 20, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(this.Location, 4))
            {
                if (m != this && CanBeHarmful(m, false))
                    list.Add(m);
            }

            foreach (var targ in list)
            {
                DoHarmful(targ);
                int dmg = Utility.RandomMinMax(100, 150);
                AOS.Damage(targ, this, dmg, 100, 0, 0, 0, 0);
                targ.SendMessage(0x22, "The ground rumbles violently beneath you!");
                targ.Freeze(TimeSpan.FromSeconds(2.0));
            }
        }

        private void GravityCrush(Mobile target)
        {
            if (!CanBeHarmful(target, false))
                return;

            Say("*Feel the weight of the mountains!*");
            PlaySound(0x23D);
            target.SendMessage(0x22, "A crushing force pins you to the earth!");
            target.FixedParticles(0x3789, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);

            target.Freeze(TimeSpan.FromSeconds(5.0));

            int stamDrain = Utility.RandomMinMax(30, 50);
            target.Stam = Math.Max(0, target.Stam - stamDrain);
        }

        private void RockwaveAttack(Mobile target)
        {
            if (!CanBeHarmful(target, false))
                return;

            Say("*The earth obeys my call!*");
            PlaySound(0x11D);
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, this.Map),
                0x36D4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

            DoHarmful(target);
            AOS.Damage(target, this, Utility.RandomMinMax(80, 120), 100, 0, 0, 0, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.3), () =>
            {
                if (Map == null) return;
                var loc = target.Location;
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var quake = new EarthquakeTile();
                quake.Hue = UniqueHue;
                quake.MoveToWorld(loc, Map);
            });
        }

        private void RegenerationBurst()
        {
            Say("*My stones renew!*");
            PlaySound(0x569);
            int heal = Utility.RandomMinMax(150, 250);
            Hits = Math.Min(HitsMax, Hits + heal);

            var loc = this.Location;
            if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
            {
                var pulse = new HealingPulseTile();
                pulse.Hue = UniqueHue;
                pulse.MoveToWorld(loc, Map);
            }
        }

        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                Say("*The mountain claims me!*");
                Effects.PlaySound(Location, Map, 0x2A2);

                for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
                {
                    int xOff = Utility.RandomMinMax(-3, 3), yOff = Utility.RandomMinMax(-3, 3);
                    var loc = new Point3D(X + xOff, Y + yOff, Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var quake = new EarthquakeTile();
                    quake.Hue = UniqueHue;
                    quake.MoveToWorld(loc, Map);
                }
            }

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new OathcarverOfTheSilentGuard());
        }

        public RockridgeSavage(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextStompTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRockwaveTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextCrushTime      = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextRegenerateTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            m_LastLocation       = this.Location;
        }
    }
}
