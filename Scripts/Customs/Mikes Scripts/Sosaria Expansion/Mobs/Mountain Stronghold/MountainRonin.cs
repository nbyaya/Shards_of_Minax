using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a mountain ronin corpse")]
    public class MountainRonin : BaseCreature
    {
        private DateTime m_NextQuakeTime;
        private DateTime m_NextSnareTime;
        private DateTime m_NextAvalancheTime;
        private Point3D m_LastLocation;

        // A cold, steely‑stone gray
        private const int UniqueHue = 2309;

        [Constructable]
        public MountainRonin()
            : base(AIType.AI_Samurai, FightMode.Closest, 10, 1, 0.3, 0.6)
        {
            Name = "a Mountain Ronin";
            Body = (Female = Utility.RandomBool()) ? 0x191 : 0x190;
            BaseSoundID = 0x5A; // human combat sounds
            Hue = UniqueHue;

            // —— Enhanced Stats ——
            SetStr(400, 450);
            SetDex(50, 70);
            SetInt(120, 140);

            SetHits(800, 1000);
            SetStam(200, 250);
            SetMana(100, 150);

            SetDamage(25, 35);

            // —— Damage Types ——
            SetDamageType(ResistanceType.Physical, 100);

            // —— Resistances ——
            SetResistance(ResistanceType.Physical, 60, 80);
            SetResistance(ResistanceType.Fire,     50, 60);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   50, 60);

            // —— Skills ——
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics,     120.0, 140.0);
            SetSkill(SkillName.Wrestling,   110.0, 125.0);
            SetSkill(SkillName.Swords,      115.0, 130.0);
            SetSkill(SkillName.Fencing,     100.0, 115.0);
            SetSkill(SkillName.Bushido,     120.0, 140.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 80;
            ControlSlots = 4;

            // —— Cooldowns ——
            m_NextQuakeTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSnareTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextAvalancheTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));

            m_LastLocation = this.Location;

            // —— Loot ——
            PackGold(200, 400);
            PackItem(new Granite(Utility.RandomMinMax(5, 10))); // thematic “ore”
        }

        // —— Aura: Quicksand Trap while moving ——
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Drop a quicksand hazard 1 in 5 moves
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                Point3D drop = m_LastLocation;
                if (Map.CanFit(drop.X, drop.Y, drop.Z, 16, false, false))
                {
                    QuicksandTile qs = new QuicksandTile
                    {
                        Hue = UniqueHue
                    };
                    qs.MoveToWorld(drop, this.Map);
                }
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now    = DateTime.UtcNow;
            var target = Combatant as Mobile;

            // Earthquake Slam (close‑range AoE)
            if (now >= m_NextQuakeTime && InRange(Combatant.Location, 2))
            {
                EarthquakeSlam();
                m_NextQuakeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // Stone Snare (roots target in place)
            else if (now >= m_NextSnareTime && InRange(Combatant.Location, 8))
            {
                StoneSnare(target);
                m_NextSnareTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Avalanche Throw (medium‑range projectile)
            else if (now >= m_NextAvalancheTime && InRange(Combatant.Location, 12))
            {
                AvalancheThrow(target);
                m_NextAvalancheTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        // —— Close-range AoE knock‑down + damage ——
        private void EarthquakeSlam()
        {
            PlaySound(0x2A3);
            // replace Shoes with LeftFoot
            FixedParticles(0x375A, 20, 15, 5046, UniqueHue, 0, EffectLayer.LeftFoot);

            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 3);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false))
                {
                    DoHarmful(m);

                    int dmg = Utility.RandomMinMax(40, 60);
                    AOS.Damage(m, this, dmg, 100, 0, 0, 0, 0);

                    // Knockback particles
                    Effects.SendLocationParticles(
                        EffectItem.Create(m.Location, Map, EffectItem.DefaultDuration),
                        0x3728, 1, 15, UniqueHue, 0, 5031, 0
                    );
                    m.PlaySound(0x22F);

                    // Push one tile away
                    Direction dir      = GetDirectionTo(m);
                    Direction opposite = (Direction)(((int)dir + 4) & 7);
                    // Move only needs the direction now
                    m.Move(opposite);
                }
            }
            eable.Free();
        }

        // —— Roots the target by planting a LandmineTile under them ——
        private void StoneSnare(Mobile target)
        {
            if (target == null || !CanBeHarmful(target, false)) return;

            Say("*Roots of the mountain bind you!*");
            PlaySound(0x228);

            // Visual impact
            target.FixedParticles(0x378A, 10, 20, 5022, UniqueHue, 0, EffectLayer.Head);

            // Drop a landmine tile at their feet
            Point3D loc = target.Location;
            if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
            {
                LandmineTile snare = new LandmineTile
                {
                    Hue = UniqueHue
                };
                snare.MoveToWorld(loc, Map);
            }
        }

        // —— Ranged rock projectile ——
        private void AvalancheThrow(Mobile target)
        {
            if (target == null || !CanBeHarmful(target, false)) return;

            Say("*Feel the crushing weight!*");
            PlaySound(0x2A0);

            // Bolt‑style visual
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, this.Map),
                0x36E4, 7, 0, false, true, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0
            );

            Timer.DelayCall(TimeSpan.FromSeconds(0.3), () =>
            {
                if (CanBeHarmful(target, false))
                {
                    DoHarmful(target);
                    int dmg = Utility.RandomMinMax(50, 75);
                    AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0);

                    target.FixedParticles(0x3779, 10, 15, 5022, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x227);
                }
            });
        }

        // —— Final tremor on death ——
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

			if (this.Map == null)
				return;

            Say("*The mountain… endures…*");
            Effects.PlaySound(Location, Map, 0x2A3);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x375A, 20, 15, UniqueHue, 0, 5046, 0
            );

            // Spawn 3–5 quake tiles around corpse
            for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
            {
                Point3D off = new Point3D(
                    X + Utility.RandomMinMax(-3, 3),
                    Y + Utility.RandomMinMax(-3, 3),
                    Z
                );

                if (!Map.CanFit(off.X, off.Y, off.Z, 16, false, false))
                    off.Z = Map.GetAverageZ(off.X, off.Y);

                EarthquakeTile quake = new EarthquakeTile
                {
                    Hue = UniqueHue
                };
                quake.MoveToWorld(off, Map);
            }
        }

        public override bool BleedImmune    => true;
        public override bool BardImmune     => true;
        public override bool AlwaysMurderer => true;
        public override int  TreasureMapLevel => 5;

        public MountainRonin(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on reload
            m_NextQuakeTime     = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextSnareTime     = DateTime.UtcNow + TimeSpan.FromSeconds(8);
            m_NextAvalancheTime = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }
    }
}
