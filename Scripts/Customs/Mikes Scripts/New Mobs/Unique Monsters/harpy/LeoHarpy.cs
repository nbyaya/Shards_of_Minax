using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a leo harpy corpse")]
    public class LeoHarpy : BaseCreature
    {
        private DateTime m_NextRoarOfTheLion;
        private DateTime m_NextSunBurst;
        private DateTime m_NextRegalFlight;
        private bool m_AbilitiesActivated; // New flag to track initial ability activation

        [Constructable]
        public LeoHarpy()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Leo Harpy";
            Body = 30; // Harpy body
            Hue = 2075; // Golden hue for golden feathers
            BaseSoundID = 402;

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

            m_AbilitiesActivated = false; // Initialize flag
        }

        public LeoHarpy(Serial serial)
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
                if (!m_AbilitiesActivated)
                {
                    // Randomly set the initial activation times
                    Random rand = new Random();
                    m_NextRoarOfTheLion = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextSunBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextRegalFlight = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));

                    m_AbilitiesActivated = true; // Set the flag to prevent re-initializing the times
                }

                if (DateTime.UtcNow >= m_NextRoarOfTheLion)
                {
                    RoarOfTheLion();
                }

                if (DateTime.UtcNow >= m_NextSunBurst)
                {
                    SunBurst();
                }

                if (DateTime.UtcNow >= m_NextRegalFlight)
                {
                    RegalFlight();
                }
            }
        }

        private void RoarOfTheLion()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Leo Harpy roars powerfully, causing fear! *");
            PlaySound(0x5F5); // Roar sound effect
            FixedEffect(0x37B8, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are terrified by the Leo Harpy's roar and feel your strength waning!");
                    AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 100, 0, 0, 0);
                    m.Damage(Utility.RandomMinMax(5, 10));
                }
            }

            m_NextRoarOfTheLion = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for RoarOfTheLion
        }

        private void SunBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Leo Harpy unleashes a blinding burst of radiant light! *");
            FixedEffect(0x37B9, 10, 16);
            PlaySound(0x20E); // Radiant light sound effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are blinded by the Leo Harpy's brilliant light and take damage!");
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextSunBurst = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for SunBurst
        }

        private void RegalFlight()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Leo Harpy takes flight and swoops down with fury! *");
            PlaySound(0x1F2); // Swooping sound effect

            Point3D start = Location;
            Point3D end = Location;

            for (int i = 0; i < 10; i++)
            {
                end.X += Utility.RandomMinMax(-1, 1);
                end.Y += Utility.RandomMinMax(-1, 1);
                end.Z = Map.GetAverageZ(end.X, end.Y);

                if (Map.CanSpawnMobile(end))
                {
                    Effects.SendLocationParticles(EffectItem.Create(start, Map, EffectItem.DefaultDuration), 0x373A, 10, 20, 0);
                    Effects.SendLocationParticles(EffectItem.Create(end, Map, EffectItem.DefaultDuration), 0x373A, 10, 20, 0);

                    foreach (Mobile m in GetMobilesInRange(3))
                    {
                        if (m != this && m.Player)
                        {
                            m.SendMessage("The Leo Harpy swoops down, attacking with fierce talons!");
                            AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                            m.PlaySound(0x208);
                            m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                        }
                    }

                    break;
                }
            }

            m_NextRegalFlight = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for RegalFlight
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

            m_AbilitiesActivated = false; // Reset flag on deserialize
        }
    }
}
