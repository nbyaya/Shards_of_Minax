using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a blighted toad corpse")]
    public class BlightedToad : BaseCreature
    {
        private DateTime m_NextDiseaseCloud;
        private DateTime m_NextDecayTouch;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BlightedToad()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a blighted toad";
            Body = 80; // Giant toad body
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

            Hue = 2466; // Unique hue

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public BlightedToad(Serial serial)
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
                    m_NextDiseaseCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDecayTouch = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextDiseaseCloud)
                {
                    DiseaseCloud();
                }

                if (DateTime.UtcNow >= m_NextDecayTouch)
                {
                    DecayTouch();
                }
            }
        }

        private void DiseaseCloud()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Releases a cloud of disease! *");
            PlaySound(0x20D); // Disease sound effect

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m.Player)
                {
                    // Apply disease effect (custom implementation needed)
                    m.SendMessage("You are enveloped by a cloud of disease!");
                    // Add disease logic here (e.g., damage over time, debuffs)
                }
            }

            m_NextDiseaseCloud = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown
        }

        private void DecayTouch()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Touches with decay! *");
            PlaySound(0x20D); // Decay sound effect

            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                // Apply decay touch effect (custom implementation needed)
                target.SendMessage("You feel a wave of decay from the toad's touch!");
                // Add decay logic here (e.g., damage over time, reduced resistance)
            }

            m_NextDecayTouch = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
