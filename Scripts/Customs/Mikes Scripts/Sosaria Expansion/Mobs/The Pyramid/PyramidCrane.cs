using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the crushed remains of a pyramid crane")]
    public class PyramidCrane : BaseCreature
    {
        private DateTime m_NextSandWarp;
        private DateTime m_NextQuicksand;
        private DateTime m_NextToxicCloud;
        private Point3D m_LastLocation;
        private const int UniqueHue = 2405; // Sandy-gold coloration

        [Constructable]
        public PyramidCrane()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name           = "a pyramid crane";
            Body           = 254;
            BaseSoundID    = 0x4D7;
            Hue            = UniqueHue;

            // — Stats —
            SetStr(200, 230);
            SetDex(150, 180);
            SetInt(300, 340);

            SetHits(1000, 1150);
            SetStam(200, 240);
            SetMana(400, 480);

            SetDamage(12, 18);
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold,     20);
            SetDamageType(ResistanceType.Energy,   50);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Fire,     20, 30);
            SetResistance(ResistanceType.Energy,   60, 70);
            SetResistance(ResistanceType.Poison,   30, 40);

            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.EvalInt,      90.0, 100.0);
            SetSkill(SkillName.Magery,      100.0, 110.0);
            SetSkill(SkillName.Tactics,      90.0, 100.0);
            SetSkill(SkillName.Wrestling,    90.0, 100.0);

            Fame          = 18000;
            Karma        = -18000;
            VirtualArmor = 70;
            ControlSlots = 5;

            // Initialize ability cooldowns
            var now = DateTime.UtcNow;
            m_NextSandWarp    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextQuicksand   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextToxicCloud  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

            // Loot
            PackItem(new MaxxiaScroll(Utility.RandomMinMax(5, 10)));
            PackGold(800, 1200);
        }

        public PyramidCrane(Serial serial) : base(serial)
        {
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Leave a swirling dust hazard occasionally
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.15 && Map != null)
            {
                var dustLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(dustLoc.X, dustLoc.Y, dustLoc.Z, 16, false, false))
                {
                    PoisonTile dust = new PoisonTile();
                    dust.Hue = UniqueHue;
                    dust.MoveToWorld(dustLoc, Map);
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Sand Warp: teleport near target
            if (now >= m_NextSandWarp && Combatant is Mobile target && CanBeHarmful(target, false))
            {
                SandWarp(target);
                m_NextSandWarp = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 22));
            }
            // Quicksand Trap
            else if (now >= m_NextQuicksand && Combatant is Mobile qtarget && CanBeHarmful(qtarget, false))
            {
                DeployQuicksand(qtarget.Location);
                m_NextQuicksand = now + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 26));
            }
            // Toxic Cloud: area PoisonTile barrage
            else if (now >= m_NextToxicCloud && Combatant is Mobile ctarget && CanBeHarmful(ctarget, false))
            {
                ToxicCloud(ctarget);
                m_NextToxicCloud = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        private void SandWarp(Mobile target)
        {
            // Teleport to a random position within 3 tiles of the target
            var rndAngle = Utility.RandomDouble() * Math.PI * 2;
            var dx = (int)(Math.Cos(rndAngle) * Utility.RandomMinMax(1, 3));
            var dy = (int)(Math.Sin(rndAngle) * Utility.RandomMinMax(1, 3));
            var dest = new Point3D(target.X + dx, target.Y + dy, target.Z);

            // Play departure effect
            PlaySound(0x22F);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(0.5)),
                                          0x3728, 10, 15, UniqueHue, 0, 5039, 0);

            // Actually move the creature
            Teleport(dest, Map);

            Say("*The sands shift!*");
        }

        private void DeployQuicksand(Point3D center)
        {
            Say("*The ground gives way!*");
            PlaySound(0x29B);

            for (int i = 0; i < 3; i++)
            {
                int rx = Utility.RandomMinMax(-2, 2);
                int ry = Utility.RandomMinMax(-2, 2);
                var loc = new Point3D(center.X + rx, center.Y + ry, center.Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                QuicksandTile qs = new QuicksandTile();
                qs.Hue = UniqueHue;
                qs.MoveToWorld(loc, Map);

                Effects.SendLocationParticles(EffectItem.Create(loc, Map, TimeSpan.FromSeconds(0.5)),
                                              0x3779, 8, 15, UniqueHue, 0, 9502, 0);
            }
        }

        private void ToxicCloud(Mobile target)
        {
            Say("*Breathe the miasma!*");
            PlaySound(0x2C6);

            var pts = new List<Point3D> { target.Location };

            // Add a few random adjacent positions too
            for (int i = 0; i < 4; i++)
            {
                int rx = Utility.RandomMinMax(-1, 1);
                int ry = Utility.RandomMinMax(-1, 1);
                pts.Add(new Point3D(target.X + rx, target.Y + ry, target.Z));
            }

            foreach (var loc in pts)
            {
                var z = loc.Z;
                if (!Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                    z = Map.GetAverageZ(loc.X, loc.Y);

                PoisonTile cloud = new PoisonTile();
                cloud.Hue = UniqueHue;
                cloud.MoveToWorld(new Point3D(loc.X, loc.Y, z), Map);

                Effects.SendLocationParticles(EffectItem.Create(new Point3D(loc.X, loc.Y, z), Map, TimeSpan.FromSeconds(0.5)),
                                              0x374A, 5, 10, UniqueHue, 0, 5032, 0);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null)
                return;

            Say("*The sands reclaim me…*");
            PlaySound(0x22F);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(0.7)),
                                          0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            // Scatter quicksand around the corpse
            for (int i = 0; i < 5; i++)
            {
                int rx = Utility.RandomMinMax(-3, 3);
                int ry = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + rx, Y + ry, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                QuicksandTile qs = new QuicksandTile();
                qs.Hue = UniqueHue;
                qs.MoveToWorld(loc, Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems);

            // Chance for a rare pyramid-themed artifact
            if (Utility.RandomDouble() < 0.03)
                PackItem(new BlessingOfTheSilverSun());  // example unique item
        }

        public override bool BleedImmune   => true;
        public override int TreasureMapLevel => 5;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reinitialize timers in case of server restart
            var now = DateTime.UtcNow;
            m_NextSandWarp   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextQuicksand  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextToxicCloud = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation   = this.Location;
        }

        //
        // Custom Teleport helper so that the call in SandWarp() compiles.
        //
        private void Teleport(Point3D dest, Map map)
        {
            if (map == null)
                return;

            // If the spot is obstructed, find a valid Z
            if (!map.CanFit(dest.X, dest.Y, dest.Z, 16, false, false))
                dest.Z = map.GetAverageZ(dest.X, dest.Y);

            // Move the mobile
            this.Location = dest;
            this.Map      = map;

            // (Optional) arrival effect
            Effects.SendLocationParticles(
                EffectItem.Create(dest, map, TimeSpan.FromSeconds(0.5)),
                0x3728, 10, 15, UniqueHue, 0, 5039, 0);

            PlaySound(0x22F);
        }
    }
}
