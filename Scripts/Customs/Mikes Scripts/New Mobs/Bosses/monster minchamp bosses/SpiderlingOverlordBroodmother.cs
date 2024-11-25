using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Spiderling Overlord Broodmother corpse")]
    public class SpiderlingOverlordBroodmother : SpiderlingMinionBroodmother
    {
        [Constructable]
        public SpiderlingOverlordBroodmother()
            : base()
        {
            Name = "Spiderling Overlord Broodmother";
            Title = "the Supreme Broodmother";
            Hue = 1152; // Different unique hue for the boss version
            BaseSoundID = 0x388;

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Higher strength for the boss
            SetDex(255); // Maximum dexterity
            SetInt(250); // High intelligence

            SetHits(12000); // Much higher health than original
            SetDamage(45, 55); // Increased damage

            // Adjust resistances to make the boss tougher
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 120.0); // Increased Magic Resist
            SetSkill(SkillName.Tactics, 120.0); // Increased Tactics
            SetSkill(SkillName.Wrestling, 120.0); // Increased Wrestling

            Fame = 25000; // High fame for a boss
            Karma = -25000; // Negative karma, as expected for a boss

            VirtualArmor = 100; // Higher armor to make it tougher

            // Attach the random ability via XmlSpawner
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
            // Additional boss logic could be added here (e.g., special actions, scripted events)
        }

        public SpiderlingOverlordBroodmother(Serial serial)
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
        }
    }
}
