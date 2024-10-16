using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an infernal incinerator corpse")]
    public class InfernalIncinerator : BaseCreature
    {
        private DateTime m_NextFlamingAura;
        private DateTime m_NextBlast;
        private DateTime m_NextImmolation;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public InfernalIncinerator()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an infernal incinerator";
            Body = 9; // Daemon body
            Hue = 1466; // Unique fire-themed hue
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

            m_AbilitiesInitialized = false; // Initialize the flag
            ControlSlots = Core.SE ? 4 : 5;
        }

        public InfernalIncinerator(Serial serial)
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
                    m_NextFlamingAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextImmolation = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFlamingAura)
                {
                    FlamingAura();
                }

                if (DateTime.UtcNow >= m_NextBlast)
                {
                    Blast();
                }

                if (DateTime.UtcNow >= m_NextImmolation)
                {
                    Immolation();
                }
            }
        }

        private void FlamingAura()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are engulfed in flames from the infernal aura!");
                    m.Damage(10, this);
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Infernal Incinerator's flaming aura burns all around it! *");
            m_NextFlamingAura = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Update the cooldown
        }

        private void Blast()
        {
            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("A fiery explosion erupts around the Infernal Incinerator!");
                    m.Damage(20, this);
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Infernal Incinerator unleashes a blast of fire! *");
            m_NextBlast = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Update the cooldown
        }

        private void Immolation()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("You are set ablaze by the Infernal Incinerator's magic!");
                    Timer.DelayCall(TimeSpan.FromSeconds(1), () => m.Damage(5, this)); // Additional damage over time
                    Timer.DelayCall(TimeSpan.FromSeconds(2), () => m.Damage(5, this)); // Additional damage over time
                    Timer.DelayCall(TimeSpan.FromSeconds(3), () => m.Damage(5, this)); // Additional damage over time
                }
            }

            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Infernal Incinerator ignites its enemies in flames! *");
            m_NextImmolation = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Update the cooldown
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
