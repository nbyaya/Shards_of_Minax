using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a hellfire juggernaut corpse")]
    public class HellfireJuggernaut : BaseCreature
    {
        private DateTime m_NextHellfireSlam;
        private DateTime m_NextInfernalCharge;
        private DateTime m_NextMoltenArmor;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public HellfireJuggernaut()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a hellfire juggernaut";
            Body = 9; // Daemon body
            Hue = 1468; // Unique hue
			BaseSoundID = 357;

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

        public HellfireJuggernaut(Serial serial)
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
                    m_NextHellfireSlam = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextInfernalCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextMoltenArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextHellfireSlam)
                {
                    HellfireSlam();
                }

                if (DateTime.UtcNow >= m_NextInfernalCharge)
                {
                    InfernalCharge();
                }

                if (DateTime.UtcNow >= m_NextMoltenArmor)
                {
                    MoltenArmor();
                }
            }
        }

        private void HellfireSlam()
        {
            if (Combatant != null)
            {
                foreach (Mobile m in GetMobilesInRange(3))
                {
                    if (m != this && m != Combatant)
                    {
                        m.SendMessage("You are scorched by the Hellfire Juggernaut's slam!");
                        AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 0, 100, 0, 0);
                    }
                }
                
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Hellfire Juggernaut slams the ground with fiery fury! *");
                m_NextHellfireSlam = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        private void InfernalCharge()
        {
            if (Combatant is Mobile target) // Explicitly check if Combatant is Mobile
            {
                target.SendMessage("The Hellfire Juggernaut charges at you with immense force!");
                AOS.Damage(target, this, Utility.RandomMinMax(40, 50), 0, 0, 0, 0, 100);
                target.Freeze(TimeSpan.FromSeconds(2));

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Hellfire Juggernaut charges at its target! *");
                m_NextInfernalCharge = DateTime.UtcNow + TimeSpan.FromMinutes(1);
            }
        }

        private void MoltenArmor()
        {
            // This is a defensive ability, so it does not need a combat target
            SendMessage("The Hellfire Juggernaut's armor glows molten red, reflecting fire damage!");

            // Reflect fire damage by increasing resistance
            SetResistance(ResistanceType.Fire, 90, 100);

            // Increase virtual armor to simulate improved defense
            VirtualArmor = 100;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Hellfire Juggernaut's molten armor intensifies! *");
            m_NextMoltenArmor = DateTime.UtcNow + TimeSpan.FromMinutes(2);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
