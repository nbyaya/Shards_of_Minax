using System;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a Zebu Zealot Boss corpse")]
    public class ZebuZealotBoss : ZebuZealot
    {
        [Constructable]
        public ZebuZealotBoss() : base()
        {
            Name = "Zebu Zealot Boss";
            Title = "the Divine Overlord";

            // Update stats to match or exceed Barracoon's (or appropriate for a boss-tier NPC)
            SetStr(1200, 1500); // Strength range increased for boss status
            SetDex(255, 300); // Higher dexterity for better speed and combat
            SetInt(250, 350); // Higher intelligence to improve spellcasting

            SetHits(15000); // Increased health for boss-tier difficulty
            SetDamage(45, 60); // Higher damage output for the boss

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 100);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 120); // Increased poison resistance for challenge
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0); // Increased magery for stronger spells
            SetSkill(SkillName.Meditation, 80.0);

            Fame = 30000; // Increased fame to match boss-tier status
            Karma = -30000; // Negative karma to reflect boss-level evil

            VirtualArmor = 100; // Increased armor for higher durability

            Tamable = false; // Boss-level creatures are not tamable
            ControlSlots = 3; // Adjust control slots if needed, or leave as is for balance
            MinTameSkill = 93.9;

            // Attach the XmlRandomAbility to give it a random ability
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

            // Optional: Additional boss-specific AI or abilities could go here
        }

        public ZebuZealotBoss(Serial serial) : base(serial)
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
        }
    }
}
