using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a banshee corpse")]
    public class BansheeBoss : Banshee
    {
        private DateTime m_NextScreech;
        private DateTime m_NextHaunt;
        private DateTime m_NextCurse;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public BansheeBoss()
            : base()
        {
            Name = "The Banshee Queen";
            Title = "the Wailing Terror";

            // Update stats to match or exceed Barracoon's
            SetStr(1200); // Increased strength for a boss-tier NPC
            SetDex(255); // Maximum dexterity
            SetInt(250); // Increased intelligence

            SetHits(12000); // Boss-tier health
            SetDamage(35, 45); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 100;

            Tamable = false;
            ControlSlots = 3; // Boss-tier control slots
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialization flag

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
            // Additional boss logic could be added here if needed
        }

        public BansheeBoss(Serial serial)
            : base(serial)
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
