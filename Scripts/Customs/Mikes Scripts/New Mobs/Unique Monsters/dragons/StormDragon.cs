using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a storm dragon corpse")]
    public class StormDragon : BaseCreature
    {
        private DateTime m_NextThunderclap;
        private DateTime m_NextLightningBreath;
        private DateTime m_NextStormShield;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public StormDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a storm dragon";
            Body = 12; // Dragon body
            Hue = 1481; // Blue hue representing storm
			BaseSoundID = 362;

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

        public StormDragon(Serial serial)
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
                    m_NextThunderclap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextLightningBreath = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextStormShield = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextThunderclap)
                {
                    Thunderclap();
                }

                if (DateTime.UtcNow >= m_NextLightningBreath)
                {
                    LightningBreath();
                }

                if (DateTime.UtcNow >= m_NextStormShield)
                {
                    StormShield();
                }
            }
        }

        private void Thunderclap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Storm Dragon emits a thunderous clap! *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The thunderclap disorients you!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                    m.Damage(20, this);
                }
            }

            m_NextThunderclap = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void LightningBreath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Storm Dragon breathes lightning! *");
            FixedEffect(0x37B9, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are struck by a bolt of lightning!");
                    m.Damage(25, this);

                    if (Utility.RandomBool())
                    {
                        m.SendMessage("You are stunned by the lightning!");
                        m.Freeze(TimeSpan.FromSeconds(2));
                    }
                }
            }

            m_NextLightningBreath = DateTime.UtcNow + TimeSpan.FromSeconds(40);
        }

        private void StormShield()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Storm Dragon summons a protective storm shield! *");
            FixedEffect(0x37C4, 10, 36);

            this.VirtualArmor += 30;

            Timer.DelayCall(TimeSpan.FromSeconds(30), () => 
            {
                this.VirtualArmor -= 30;
            });

            m_NextStormShield = DateTime.UtcNow + TimeSpan.FromMinutes(1);
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

            m_AbilitiesInitialized = false; // Reset flag to reinitialize abilities
        }
    }
}
