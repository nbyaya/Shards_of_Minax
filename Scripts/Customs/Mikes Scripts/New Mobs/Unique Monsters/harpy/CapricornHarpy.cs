using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a capricorn harpy corpse")]
    public class CapricornHarpy : Harpy
    {
        private DateTime m_NextMountainMight;
        private DateTime m_NextStalwartDefense;
        private DateTime m_NextSeismicRoar;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CapricornHarpy()
            : base()
        {
            Name = "a Capricorn Harpy";
            Body = 30; // Harpy body
            Hue = 2077; // Unique hue for the Capricorn Harpy
			BaseSoundID = 402; // Harpy sound
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

        public CapricornHarpy(Serial serial)
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
                    m_NextMountainMight = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextStalwartDefense = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextSeismicRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true; // Set flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMountainMight)
                {
                    MountainMight();
                }

                if (DateTime.UtcNow >= m_NextStalwartDefense)
                {
                    StalwartDefense();
                }

                if (DateTime.UtcNow >= m_NextSeismicRoar)
                {
                    SeismicRoar();
                }
            }
        }

        private void MountainMight()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Capricorn Harpy summons a massive rockslide! *");
            PlaySound(0x20F);
            Effects.SendLocationEffect(Location, Map, 0x3F9, 16, 1, 0x3B2, 0);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive)
                {
                    if (Utility.RandomDouble() < 0.3) // 30% chance to stun
                    {
                        m.SendMessage("You are struck by a rockslide and stunned!");
                        m.Freeze(TimeSpan.FromSeconds(2));
                    }
                    Effects.SendLocationEffect(m.Location, m.Map, 0x58, 16, 1, 0x3B2, 0);
                    AOS.Damage(m, this, Utility.RandomMinMax(25, 45), 0, 100, 0, 0, 0);
                }
            }

            m_NextMountainMight = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset interval
        }

        private void StalwartDefense()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Capricorn Harpy fortifies its defenses! *");
            PlaySound(0x1A1);

            this.VirtualArmor += 25;
            this.SetResistance(ResistanceType.Physical, this.PhysicalResistance + 25);

            Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(ResetDefense));

            m_NextStalwartDefense = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Reset interval
        }

        private void ResetDefense()
        {
            this.VirtualArmor -= 25;
            this.SetResistance(ResistanceType.Physical, this.PhysicalResistance - 25);
        }

        private void SeismicRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Capricorn Harpy lets out a thunderous roar! *");
            PlaySound(0x45D);
            Effects.SendLocationEffect(Location, Map, 0x3F8, 20, 1, 0x3B2, 0);

            foreach (Mobile m in GetMobilesInRange(6))
            {
                if (m != this && m.Alive)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 30), 0, 100, 0, 0, 0);
                    m.SendMessage("The seismic roar disorients you!");
                    m.AddToBackpack(new Apple()); // Dropping a rock as a minor effect
                }
            }

            m_NextSeismicRoar = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Reset interval
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
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
