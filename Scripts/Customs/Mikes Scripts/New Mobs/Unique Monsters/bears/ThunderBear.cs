using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a thunder bear corpse")]
    public class ThunderBear : BlackBear
    {
        private DateTime m_NextThunderclap;
        private DateTime m_NextElectrostaticField;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ThunderBear()
            : base()
        {
            Name = "a thunder bear";
            Hue = 1179; // Electric blue hue for a unique look

            // Set stats for Thunder Bear
            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			BaseSoundID = 0xA3;
			
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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public ThunderBear(Serial serial)
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
                    m_NextThunderclap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextElectrostaticField = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextThunderclap)
                {
                    Thunderclap();
                }

                if (DateTime.UtcNow >= m_NextElectrostaticField)
                {
                    ElectrostaticField();
                }
            }
        }

        private void Thunderclap()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Thunderclap! *");
            PlaySound(0x29F);
            FixedEffect(0x3779, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The Thunder Bear's thunderclap stuns you!");
                    m.Freeze(TimeSpan.FromSeconds(1));
                    m.Damage(Utility.RandomMinMax(5, 10), this);
                }
            }

            // Set the next ability interval after a random cooldown
            Random rand = new Random();
            m_NextThunderclap = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(20, 40));
        }

        private void ElectrostaticField()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Electrostatic Field! *");
            PlaySound(0x29F);
            FixedEffect(0x374A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The Thunder Bear's electric field shocks and paralyzes you!");
                    m.Damage(Utility.RandomMinMax(5, 10), this);

                    if (Utility.RandomDouble() < 0.2)
                        m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            // Set the next ability interval after a random cooldown
            Random rand = new Random();
            m_NextElectrostaticField = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(30, 60));
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

            // Reset ability intervals on deserialize
            m_AbilitiesInitialized = false;
        }
    }
}
