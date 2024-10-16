using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a rath'zor the shattered corpse")]
    public class RathZorTheShattered : ElderGazer
    {
        private DateTime m_NextRealityShatter;
        private DateTime m_NextDimensionalRift;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public RathZorTheShattered()
            : base()
        {
            Name = "Rath'Zor the Shattered";
            Hue = 1767; // Unique hue for Rath'Zor
            Body = 22; // ElderGazer body
            BaseSoundID = 377;

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

        public RathZorTheShattered(Serial serial)
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
                    m_NextRealityShatter = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDimensionalRift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRealityShatter)
                {
                    RealityShatter();
                }

                if (DateTime.UtcNow >= m_NextDimensionalRift)
                {
                    DimensionalRift();
                }
            }
        }

        private void RealityShatter()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Rath'Zor's presence causes reality to shatter! *");
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.Damage(10, this); // Example damage
                    m.SendMessage("The ground cracks beneath you, causing great pain!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextRealityShatter = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Update cooldown
        }

        private void DimensionalRift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Rath'Zor tears open a rift in the fabric of reality! *");
            FixedEffect(0x373A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.Damage(10, this); // Example damage
                    m.SendMessage("You are pulled and disoriented by a dimensional rift!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextDimensionalRift = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Update cooldown
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
