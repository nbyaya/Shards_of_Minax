using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a buried cow corpse")]
    public class BuriedCow : BaseCreature
    {
        private DateTime m_NextSonicTime;
        private DateTime m_NextFissureTime;
        private DateTime m_NextSummonTime;

        // A deep, earthen-brown tint
        private const int UniqueHue = 1909;

        [Constructable]
        public BuriedCow()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a buried cow";
            Body = Utility.RandomList(0xD8, 0xE7);
            BaseSoundID = 0x78;
            Hue = UniqueHue;

            // —— Stats ——
            SetStr(500, 600);
            SetDex(100, 150);
            SetInt(100, 120);

            SetHits(1800, 2000);
            SetStam(150, 200);
            SetMana(200, 300);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Wrestling, 120.1, 130.0);
            SetSkill(SkillName.Tactics,   120.1, 130.0);
            SetSkill(SkillName.MagicResist,100.1,110.0);
            SetSkill(SkillName.Anatomy,   100.1,110.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 65;
            ControlSlots = 4;

            // Staggered cooldowns
            m_NextSonicTime   = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextFissureTime = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextSummonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(20);

            // Basic loot
            PackGold(200, 300);
            PackItem(new BonePile(Utility.RandomMinMax(5, 10)));
            PackItem(new RawRibs());
        }

        public BuriedCow(Serial serial)
            : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Map == null || Combatant == null)
                return;

            var now = DateTime.UtcNow;

            // Only proceed if Combatant is a Mobile
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                double dist = GetDistanceToSqrt(target);

                // 1) Sonic Moo (stun AoE)
                if (now >= m_NextSonicTime && dist <= 10)
                {
                    SonicMoo();
                    m_NextSonicTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                }

                // 2) Ground Fissure (landmines in 3×3)
                if (now >= m_NextFissureTime && dist <= 8)
                {
                    GroundFissure(target.Location);
                    m_NextFissureTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 40));
                }

                // 3) Summon Buried Kin
                if (now >= m_NextSummonTime && dist <= 12)
                {
                    SummonKin();
                    m_NextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(60, 90));
                }
            }
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (!Alive || Map == null || m == this)
                return;

            // 30% chance to drop quicksand under any hostile that steps within 2 tiles
            if (m.InRange(Location, 2) && CanBeHarmful(m, false) && Utility.RandomDouble() < 0.30)
            {
                var loc = m.Location;
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var qs = new QuicksandTile();
                qs.Hue = UniqueHue;
                qs.MoveToWorld(loc, Map);
            }
        }

        private void SonicMoo()
        {
            Say("MooOOOoooo!");
            PlaySound(0x64);

            var toAffect = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && m.Alive && CanBeHarmful(m, false))
                    toAffect.Add(m);
            }

            foreach (var m in toAffect)
            {
                DoHarmful(m);
                m.FixedParticles(0x376A, 10, 20, 5025, UniqueHue, 0, EffectLayer.Waist);
                m.Freeze(TimeSpan.FromSeconds(2));
            }
        }

        private void GroundFissure(Point3D center)
        {
            Say("The earth trembles!");
            PlaySound(0x3F);

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    var loc = new Point3D(center.X + dx, center.Y + dy, center.Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    var mine = new LandmineTile();
                    mine.Hue = UniqueHue;
                    mine.MoveToWorld(loc, Map);
                }
            }
        }

        private void SummonKin()
        {
            Say("Rise, my buried kin!");
            int count = Utility.RandomMinMax(2, 4);

            for (int i = 0; i < count; i++)
            {
                var loc = new Point3D(
                    X + Utility.RandomMinMax(-2, 2),
                    Y + Utility.RandomMinMax(-2, 2),
                    Z
                );

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                // Summon a skeletal calf (use the standard Cow class, re-hued and renamed)
                var calf = new Cow
                {
                    Name = "a skeletal calf",
                    Hue  = UniqueHue
                };
                calf.MoveToWorld(loc, Map);
                calf.Combatant = Combatant; // immediately join the fight
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            // Big muddy explosion
            Effects.PlaySound(Location, Map, 0x3E9);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0
            );

            // Scatter poisonous puddles
            int drops = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < drops; i++)
            {
                var loc = new Point3D(
                    X + Utility.RandomMinMax(-4, 4),
                    Y + Utility.RandomMinMax(-4, 4),
                    Z
                );

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var puddle = new PoisonTile();
                puddle.Hue = UniqueHue;
                puddle.MoveToWorld(loc, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x376A, 10, 20, UniqueHue, 0, 5039, 0
                );
            }
        }

        // Standard properties
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 5;
        public override double DispelDifficulty => 140.0;
        public override double DispelFocus     => 70.0;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write( (int)0 ); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns
            m_NextSonicTime   = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextFissureTime = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextSummonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }
    }
}
