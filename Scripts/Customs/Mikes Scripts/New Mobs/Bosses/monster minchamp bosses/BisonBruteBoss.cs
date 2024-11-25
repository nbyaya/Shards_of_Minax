using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a bison overlord corpse")]
    public class BisonBruteBoss : BisonBrute
    {
        [Constructable]
        public BisonBruteBoss() : base()
        {
            Name = "Bison Overlord";
            Title = "the Mighty Brute";

            // Update stats to match or exceed Barracoon and previous NPCs
            SetStr(1200); // Higher strength for boss tier
            SetDex(255); // Max dexterity for high mobility
            SetInt(250); // High intelligence to boost abilities

            SetHits(12000); // Boss-level health
            SetDamage(50, 60); // Increased damage

            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000;  // Increased fame
            Karma = -30000; // Negative karma for a boss-tier NPC

            VirtualArmor = 100; // Increased armor for higher defense

            Tamable = false; // Bosses are not tamable
            ControlSlots = 0;

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
            // Additional boss-specific logic can be added here if necessary
        }

        public BisonBruteBoss(Serial serial) : base(serial)
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
