using System;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a leaf bear overlord corpse")]
    public class LeafBearBoss : LeafBear
    {
        [Constructable]
        public LeafBearBoss()
            : base()
        {
            Name = "Leaf Bear Overlord";
            Title = "the Forest's Wrath";
            Hue = 1191; // Forest green hue for consistency

            // Update stats to make it a boss-level creature
            SetStr(1200); // Enhanced strength
            SetDex(255); // Enhanced dexterity
            SetInt(250); // Enhanced intelligence

            SetHits(12000); // Boss-level health
            SetDamage(50, 70); // Enhanced damage

            // Set enhanced resistances
            SetResistance(ResistanceType.Physical, 85);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0); // High magic resistance for a boss
            SetSkill(SkillName.Tactics, 120.0); // Boss-level tactics
            SetSkill(SkillName.Wrestling, 120.0); // Boss-level wrestling

            Fame = 24000; // Fame level set to be higher for a boss
            Karma = -24000; // Karma remains negative for consistency

            VirtualArmor = 90; // Boss-level armor

            // Attach the XmlRandomAbility
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

        public LeafBearBoss(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Ensure the abilities are initialized properly upon deserialization
        }
    }
}
