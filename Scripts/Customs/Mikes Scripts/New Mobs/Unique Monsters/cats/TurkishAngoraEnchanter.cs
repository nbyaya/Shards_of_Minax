using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Turkish Angora Enchanter corpse")]
    public class TurkishAngoraEnchanter : BaseCreature
    {
        private DateTime m_NextArcaneBurst;
        private DateTime m_NextCharm;
        private DateTime m_NextMysticShield;
        private bool m_HasShield;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public TurkishAngoraEnchanter()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Turkish Angora Enchanter";
            Body = 0xC9; // Cat body
            Hue = 1292;  // Unique hue for the Enchanter
			BaseSoundID = 0x69;

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
            SetResistance(ResistanceType.Poison, 100);
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

            m_NextArcaneBurst = DateTime.UtcNow;
            m_NextCharm = DateTime.UtcNow;
            m_NextMysticShield = DateTime.UtcNow;
            m_HasShield = false;
            m_AbilitiesInitialized = false; // Set flag to false
        }

        public TurkishAngoraEnchanter(Serial serial)
            : base(serial)
        {
        }

		public override bool ReacquireOnMovement
		{
			get
			{
				return !Controlled;
			}
		}
		public override bool AutoDispel
		{
			get
			{
				return !Controlled;
			}
		}

		public override int TreasureMapLevel
		{
			get
			{
				return 5;
			}
		}
		
		public override bool CanAngerOnTame
		{
			get
			{
				return true;
			}
		}

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
                    m_NextArcaneBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextCharm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextMysticShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_AbilitiesInitialized = true; // Set flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextArcaneBurst)
                {
                    ArcaneBurst();
                }

                if (DateTime.UtcNow >= m_NextCharm)
                {
                    Charm();
                }

                if (DateTime.UtcNow >= m_NextMysticShield)
                {
                    MysticShield();
                }
            }
        }

        private void ArcaneBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Turkish Angora Enchanter conjures a dazzling arcane blast! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    m.Damage(damage, this);
                }
            }

            m_NextArcaneBurst = DateTime.UtcNow + TimeSpan.FromSeconds(25); // Cooldown
        }

        private void Charm()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                if (target != null && target.Alive)
                {
                    target.Say("* You are now under my charm! *");
                    target.Kill(); // Apply charm effect (replace this line with actual charm effect)
                }

                m_NextCharm = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown
            }
        }

        private void MysticShield()
        {
            if (!m_HasShield)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Turkish Angora Enchanter is enveloped in a mystical shield! *");
                FixedEffect(0x376A, 10, 16);

                // Set shield effect (actual shield logic would need to be implemented)
                m_HasShield = true;
                Timer.DelayCall(TimeSpan.FromSeconds(10), () => { m_HasShield = false; });

                m_NextMysticShield = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
            }
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

            // Reset the initialization flag to ensure proper reinitialization
            m_AbilitiesInitialized = false;
            m_NextArcaneBurst = DateTime.UtcNow;
            m_NextCharm = DateTime.UtcNow;
            m_NextMysticShield = DateTime.UtcNow;
            m_HasShield = false;
        }
    }
}
