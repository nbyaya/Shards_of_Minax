using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a sphinx cat corpse")]
    public class SphinxCat : BaseCreature
    {
        private DateTime m_NextMysticGaze;
        private DateTime m_NextAncientAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SphinxCat()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a sphinx cat";
            Body = 0xC9; // Cat body
            Hue = 1293; // Unique hue
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

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SphinxCat(Serial serial)
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
                    m_NextMysticGaze = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20)); // Random start between 5 and 20 seconds
                    m_NextAncientAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30)); // Random start between 10 and 30 seconds
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextMysticGaze)
                {
                    MysticGaze();
                }

                if (DateTime.UtcNow >= m_NextAncientAura)
                {
                    AncientAura();
                }

                if (Utility.RandomDouble() < 0.05) // 5% chance to use Arcane Prowess
                {
                    ArcaneProwess();
                }
            }
        }

        private void MysticGaze()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sphinx Cat gazes mystically *");
                target.Freeze(TimeSpan.FromSeconds(5));
                m_NextMysticGaze = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown
            }
        }

        private void AncientAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sphinx Cat's ancient aura envelops the area *");
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m != Combatant && m.Alive)
                {
                    // Reducing target's attack power by reducing their damage
                    m.SendMessage("You feel your strength draining!");
                    // Apply a penalty or other effect here if needed
                }
            }

            m_NextAncientAura = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
        }

        private void ArcaneProwess()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Sphinx Cat channels arcane power *");

            // Play some visual effect or sound if desired
            FixedEffect(0x373A, 10, 15);
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
