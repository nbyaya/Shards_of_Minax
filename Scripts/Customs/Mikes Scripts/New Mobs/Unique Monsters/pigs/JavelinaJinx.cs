using System;
using Server;
using Server.Mobiles;
using Server.Network; // Ensure this is included

namespace Server.Mobiles
{
    [CorpseName("a javelina jinx corpse")]
    public class JavelinaJinx : BaseCreature
    {
        private DateTime m_NextSonicRoar;
        private DateTime m_NextQuickStrike;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public JavelinaJinx()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Javelina Jinx";
            Body = 0xCB; // Pig body
            Hue = 2190; // Brown hue
			BaseSoundID = 0xC4;

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

        public JavelinaJinx(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextSonicRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextQuickStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextSonicRoar)
                {
                    SonicRoar();
                }

                if (DateTime.UtcNow >= m_NextQuickStrike)
                {
                    QuickStrike();
                }
            }
        }

        private void SonicRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Sonic Roar! *");
            PlaySound(0x2D5); // Loud roar sound
            FixedEffect(0x373A, 10, 16); // Roar effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m is BaseCreature && !m.Player)
                {
                    BaseCreature bc = (BaseCreature)m;
                    bc.SendMessage("You are disoriented by the Javelina Jinx's roar!");
                    bc.VirtualArmor -= 10;
                }
            }

            Random rand = new Random();
            m_NextSonicRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 30)); // Random cooldown for SonicRoar
        }

        private void QuickStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Quick Strike! *");
            PlaySound(0x2F4); // Rapid attack sound

            foreach (Mobile m in GetMobilesInRange(2))
            {
                if (m != this && m.Alive)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(5, 10), 0, 0, 0, 0, 0);
                }
            }

            Random rand = new Random();
            m_NextQuickStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 25)); // Random cooldown for QuickStrike
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
