using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("the corpse of an inferno dragon overlord")]
    public class InfernoDrakonBoss : InfernoDrakon
    {
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public InfernoDrakonBoss()
            : base()
        {
            Name = "Inferno Dragon Overlord";
            Title = "the Inferno King";
            Hue = 1480; // Fiery hue for the boss

            // Enhanced stats for the boss version (higher than original InfernoDrakon)
            SetStr(1200, 1500); // Increased strength
            SetDex(255, 300);   // Increased dexterity
            SetInt(250, 350);   // Increased intelligence

            SetHits(14000);     // Increased health
            SetDamage(40, 50);  // Increased damage

            SetDamageType(ResistanceType.Physical, 60); // Adjusted damage type percentages
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 10);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);  // Higher magic resist
            SetSkill(SkillName.Tactics, 120.0);      // Increased tactics skill
            SetSkill(SkillName.Wrestling, 120.0);    // Increased wrestling skill

            Fame = 30000;  // Increased fame for the boss
            Karma = -30000; // Increased karma penalty for the boss

            VirtualArmor = 100; // Increased virtual armor

            Tamable = false;    // Boss is not tamable
            ControlSlots = 0;   // Not controllable

            m_AbilitiesInitialized = false; // Initialize flag for abilities

            // Attach a random ability to the boss
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here if needed
        }

        public InfernoDrakonBoss(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
