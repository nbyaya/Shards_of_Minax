using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a cliff goat corpse")]
    public class CliffGoat : BaseCreature
    {
        private DateTime m_NextCliffLeap;
        private DateTime m_NextRockArmor;
        private DateTime m_NextRockSlide;
        private DateTime m_NextStomp;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CliffGoat()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cliff goat";
            Body = 0xD1; // Using goat body
            Hue = 1918;  // Grayish-white hue
			BaseSoundID = 0x99;

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

        public CliffGoat(Serial serial)
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
                    m_NextCliffLeap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextRockArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextRockSlide = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_NextStomp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 120));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextCliffLeap)
                {
                    CliffLeap();
                }

                if (DateTime.UtcNow >= m_NextRockArmor)
                {
                    RockArmor();
                }

                if (DateTime.UtcNow >= m_NextRockSlide)
                {
                    RockSlide();
                }

                if (DateTime.UtcNow >= m_NextStomp)
                {
                    Stomp();
                }
            }
        }

        private void CliffLeap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mountain Goat leaps from the cliff, crushing anything below! *");
            PlaySound(0x5F9);
            FixedEffect(0x37C4, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(25, 35);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                }
            }

            m_NextCliffLeap = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void RockArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mountain Goat's hide hardens into rocky armor! *");
            PlaySound(0x5F8);

            // Temporarily increase physical resistance
            SetResistance(ResistanceType.Physical, 90, 100);

            Timer.DelayCall(TimeSpan.FromSeconds(30), () => SetResistance(ResistanceType.Physical, 60, 70));

            m_NextRockArmor = DateTime.UtcNow + TimeSpan.FromMinutes(1);
        }

        private void RockSlide()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mountain Goat triggers a rock slide, causing debris to fall! *");
            PlaySound(0x5F7);
            FixedEffect(0x37C4, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(15, 25);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are hit by falling debris!");
                }
            }

            m_NextRockSlide = DateTime.UtcNow + TimeSpan.FromMinutes(1.5);
        }

        private void Stomp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mountain Goat stomps the ground with a mighty force! *");
            PlaySound(0x5F6);
            FixedEffect(0x37C4, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    m.SendMessage("You are knocked down by the powerful stomp!");

                    // Knock down effect
                    m.Frozen = true;
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () => m.Frozen = false);
                }
            }

            m_NextStomp = DateTime.UtcNow + TimeSpan.FromMinutes(2);
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
