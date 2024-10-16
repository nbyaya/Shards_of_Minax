using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a leo sunbear corpse")]
    public class LeoSunBear : BaseCreature
    {
        private DateTime m_NextSolarRoar;
        private DateTime m_NextRoyalPresence;
        private DateTime m_NextFlareStrike;
        private DateTime m_NextMeteorShower;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public LeoSunBear()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Leo SunBear";
            Body = 212; // GrizzlyBear body
            Hue = 2000; // Golden hue for the mane
			BaseSoundID = 0xA3;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public LeoSunBear(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextSolarRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextRoyalPresence = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextFlareStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextMeteorShower = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSolarRoar)
                {
                    SolarRoar();
                }

                if (DateTime.UtcNow >= m_NextRoyalPresence)
                {
                    RoyalPresence();
                }

                if (DateTime.UtcNow >= m_NextFlareStrike)
                {
                    FlareStrike();
                }

                if (DateTime.UtcNow >= m_NextMeteorShower)
                {
                    MeteorShower();
                }
            }
        }

        private void SolarRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Leo SunBear roars with the power of the sun!*");
            FixedEffect(0x3709, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The Leo SunBear's roar burns you with solar fire!");
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextSolarRoar = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void RoyalPresence()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Leo SunBear exudes an aura of royal authority!*");

            Fame += 500;
            Karma += 500;

            m_NextRoyalPresence = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void FlareStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Leo SunBear channels the sun's energy for a Flare Strike!*");
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(7))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The Leo SunBear's Flare Strike burns you with intense heat!");
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
                    m.PlaySound(0x208);
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                }
            }

            m_NextFlareStrike = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void MeteorShower()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Leo SunBear calls down a Meteor Shower!*");
            FixedEffect(0x373A, 10, 16);

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.5), () =>
                {
                    Point3D loc = new Point3D(X + Utility.RandomMinMax(-3, 3), Y + Utility.RandomMinMax(-3, 3), Z);
                    Effects.SendLocationEffect(loc, Map, 0x36BD, 20, 10);

                    foreach (Mobile m in GetMobilesInRange(3))
                    {
                        if (m != this && m.Player)
                        {
                            m.SendMessage("Meteors rain down from the sky!");
                            AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                            m.PlaySound(0x1DD);
                            m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                        }
                    }
                });
            }

            m_NextMeteorShower = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
