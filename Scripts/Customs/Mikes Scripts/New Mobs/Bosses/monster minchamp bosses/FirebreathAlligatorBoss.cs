using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a firebreath alligator boss corpse")]
    public class FirebreathAlligatorBoss : FirebreathAlligator
    {
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public FirebreathAlligatorBoss() : base()
        {
            Name = "Firebreath Alligator Boss";
            Title = "the Inferno King";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Matching or exceeding Barracoon's strength
            SetDex(255);  // Matching or exceeding Barracoon's dexterity
            SetInt(250);  // Matching or exceeding Barracoon's intelligence

            SetHits(12000); // Boss-tier health
            SetDamage(35, 45); // Enhanced damage range

            // Increase resistances
            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 85);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 35000;  // Enhanced fame for a boss
            Karma = -35000; // Negative karma

            VirtualArmor = 100;  // Enhanced armor

            Tamable = false;  // The boss is untamable
            ControlSlots = 3;
            MinTameSkill = 0;

            m_AbilitiesInitialized = false; // Initialize abilities flag

            PackItem(new BossTreasureBox());
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
            // Additional boss logic could be added here
        }

        public FirebreathAlligatorBoss(Serial serial) : base(serial)
        {
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
