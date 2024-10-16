using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an infernal toad corpse")]
    public class InfernalToad : GiantToad
    {
        private DateTime m_NextLavaSpit;
        private DateTime m_NextEmberAura;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public InfernalToad()
            : base()
        {
            Name = "an infernal toad";
            Body = 80; // Use the same body as GiantToad
            Hue = 2464; // Unique hue for the infernal look
            BaseSoundID = 0x26B;
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

        public InfernalToad(Serial serial)
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
                    m_NextLavaSpit = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20)); // Random interval between 1 and 20 seconds
                    m_NextEmberAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30)); // Random interval between 1 and 30 seconds
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextLavaSpit)
                {
                    LavaSpit();
                }

                if (DateTime.UtcNow >= m_NextEmberAura)
                {
                    EmberAura();
                }
            }
        }

        private void LavaSpit()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Infernal Toad spits molten lava! *");

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    m.SendMessage("You are scorched by the infernal toad's lava!");
                    m.PlaySound(0x208); // Fire sound effect
                }
            }

            m_NextLavaSpit = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown
        }

        private void EmberAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Infernal Toad emanates a fiery aura! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    m.SendMessage("You are burned by the infernal toad's ember aura!");
                }
            }

            // Increase the Infernal Toad's fire resistance
            SetResistance(ResistanceType.Fire, 80, 90);

            m_NextEmberAura = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Reset cooldown
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
