using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a crag fiddler corpse")]
    public class CragFiddler : BaseCreature
    {
        private DateTime m_NextQuakeTime;
        private DateTime m_NextChordTime;
        private Point3D m_LastLocation;

        private const int UniqueHue = 1250;

        [Constructable]
        public CragFiddler()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Crag Fiddler";
            Body = 724;
            BaseSoundID = 0x29;
            Hue = UniqueHue;

            SetStr(300, 350);
            SetDex(200, 240);
            SetInt(150, 180);

            SetHits(1200, 1400);
            SetStam(200, 240);
            SetMana(150, 200);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Cold, 30);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Cold,     60, 70);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   40, 50);

            SetSkill(SkillName.Wrestling,    90.0, 100.0);
            SetSkill(SkillName.Tactics,      95.0, 105.0);
            SetSkill(SkillName.MagicResist,  80.0,  90.0);
            SetSkill(SkillName.Anatomy,      85.0,  95.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 75;
            ControlSlots = 4;

            m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextChordTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_LastLocation = this.Location;

            AddItem(new Bow());
            PackItem(new Arrow(Utility.RandomMinMax(50, 70)));
            PackItem(new IronOre(Utility.RandomMinMax(10, 20)));
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && CanBeHarmful(m, false))
            {
                Mobile target = m as Mobile;
                if (target != null)
                {
                    DoHarmful(target);

                    // *** FIX #1: now using the 13‑arg overload, adding a final "0" ***
                    Effects.SendMovingParticles(
                        new Entity(Serial.Zero, this.Location, this.Map),
                        new Entity(Serial.Zero, target.Location, target.Map),
                        0x374A, 5, 10,
                        false, true,
                        UniqueHue,      // hue
                        0,              // renderMode
                        9502,           // effect
                        0,              // explodeEffect
                        0,              // explodeSound
                        0               // unknown
                    );

                    AOS.Damage(target, this, Utility.RandomMinMax(8, 15), 100, 0, 0, 0, 0);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (this.Location != m_LastLocation)
                m_LastLocation = this.Location;

            if (Combatant == null || !Alive || Map == Map.Internal)
                return;

            if (DateTime.UtcNow >= m_NextQuakeTime && InRange(Combatant.Location, 10))
            {
                EarthquakeSlam();
                m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextChordTime && InRange(Combatant.Location, 6))
            {
                SonicChord();
                m_NextChordTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
        }

        private void EarthquakeSlam()
        {
            PlaySound(0x1F1);
            Say("*The earth trembles beneath your feet!*");

            for (int dx = -3; dx <= 3; dx++)
            {
                for (int dy = -3; dy <= 3; dy++)
                {
                    Point3D loc = new Point3D(X + dx, Y + dy, Z);
                    if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    {
                        var tile = new EarthquakeTile { Hue = UniqueHue };
                        tile.MoveToWorld(loc, Map);
                    }
                }
            }

            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 3);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false))
                {
                    Mobile target = m as Mobile;
                    DoHarmful(target);

                    AOS.Damage(target, this, Utility.RandomMinMax(30, 50), 100, 0, 0, 0, 0);
                    target.Freeze(TimeSpan.FromSeconds(2.0));
                    target.FixedParticles(0x376A, 10, 20, UniqueHue, EffectLayer.Waist);
                }
            }
            eable.Free();
        }

        private void SonicChord()
        {
            if (!(Combatant is Mobile target)) return;

            PlaySound(0x20B);
            Say("*Screech of the stones!*");

            var coneTargets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && InLOS(m))
                {
                    double dx = m.X - X, dy = m.Y - Y;
                    double angTo   = Math.Atan2(dy, dx),
                           angDir  = Math.Atan2(Combatant.Y - Y, Combatant.X - X);

                    if (Math.Abs(angTo - angDir) < (Math.PI / 6))
                        coneTargets.Add(m);
                }
            }
            eable.Free();

            foreach (var mob in coneTargets)
            {
                DoHarmful(mob);
                AOS.Damage(mob, this, Utility.RandomMinMax(25, 40), 100, 0, 0, 0, 0);

                mob.FixedParticles(0x3709, 10, 15, UniqueHue, EffectLayer.Head);
                mob.SendMessage(0x22, "Your bones resonate painfully!");

                // *** FIX #2: capture the mod, then remove by that same object ***
                var mod = new ResistanceMod(ResistanceType.Physical, -10);
                mob.AddResistanceMod(mod);
                Timer.DelayCall(TimeSpan.FromSeconds(10), () => mob.RemoveResistanceMod(mod));
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new MaxxiaScroll());
        }

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			if (Map == null || Map == Map.Internal)
				return;

			PlaySound(0x1F1);
			Effects.SendLocationEffect(Location, Map, 0x3728, 30, UniqueHue, 0, 5023);

			for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
			{
				int dx = Utility.RandomMinMax(-2, 2), dy = Utility.RandomMinMax(-2, 2);
				Point3D loc = new Point3D(X + dx, Y + dy, Z);
				if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
				{
					var tile = new LandmineTile { Hue = UniqueHue };
					tile.MoveToWorld(loc, Map);
				}
			}
		}


        public CragFiddler(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextQuakeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextChordTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }
    }
}
