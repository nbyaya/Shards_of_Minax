using System;
using Server;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a bengal storm corpse")]
    public class BengalStorm : BaseCreature
    {
        private DateTime m_NextLightningPaw;
        private DateTime m_NextThunderclap;
        private DateTime m_NextStormCloak;
        private bool m_IsStormCloakActive;
        private bool m_AbilitiesInitialized; // Flag to track if random intervals have been initialized

        [Constructable]
        public BengalStorm()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Bengal Storm";
            Body = 0xC9; // Cat body
            Hue = 1301; // Unique hue for Bengal Storm (lightning-themed)
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

            // Initialize the abilities
            m_AbilitiesInitialized = false; // Set the flag to false
        }

        public BengalStorm(Serial serial)
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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextLightningPaw = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextThunderclap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextStormCloak = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextLightningPaw)
                {
                    LightningPaw();
                }

                if (DateTime.UtcNow >= m_NextThunderclap)
                {
                    Thunderclap();
                }

                if (DateTime.UtcNow >= m_NextStormCloak && !m_IsStormCloakActive)
                {
                    StormCloak();
                }
            }
        }

        private void LightningPaw()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Bengal Storm unleashes a crackling lightning strike! *");
            PlaySound(0x29F);
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m != Combatant)
                {
                    m.Damage(20, this);
                    m.SendMessage("You are struck by a lightning bolt from the Bengal Storm!");
                }
            }

            m_NextLightningPaw = DateTime.UtcNow + TimeSpan.FromSeconds(25);
        }

        private void Thunderclap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Bengal Storm lets out a thunderous clap! *");
            PlaySound(0x30F);
            FixedEffect(0x37B9, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this)
                {
                    m.Damage(15, this);
                    m.SendMessage("The thunderclap stuns you!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextThunderclap = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void StormCloak()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Bengal Storm envelops itself in a stormy cloak! *");
            PlaySound(0x2D9);
            FixedEffect(0x376A, 10, 16);

            m_IsStormCloakActive = true;
            this.SetResistance(ResistanceType.Energy, 100); // Full resistance to energy while active

            Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
            {
                m_IsStormCloakActive = false;
                this.SetResistance(ResistanceType.Energy, 60); // Restore resistance after cloak expires
            });

            m_NextStormCloak = DateTime.UtcNow + TimeSpan.FromSeconds(35);
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

            // Reset the initialization flag on deserialization
            m_AbilitiesInitialized = false;
        }
    }
}
