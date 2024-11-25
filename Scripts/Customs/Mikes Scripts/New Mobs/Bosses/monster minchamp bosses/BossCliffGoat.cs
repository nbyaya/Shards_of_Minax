using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss cliff goat corpse")]
    public class BossCliffGoat : CliffGoat
    {
        [Constructable]
        public BossCliffGoat()
        {
            Name = "Cliff Goat";
            Title = "the Colossus";
            Hue = 0x497; // Unique hue for a boss
            Body = 0xD1; // Default goat body, can be customized if needed

            SetStr(1200); // Enhanced strength
            SetDex(255);  // Max dexterity
            SetInt(250);  // Enhanced intelligence

            SetHits(12000); // Boss-level health
            SetDamage(40, 50); // Higher damage

            SetResistance(ResistanceType.Physical, 90, 100); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 80, 100);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Max magic resist
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Higher tactics
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Higher wrestling

            Fame = 24000; // High fame
            Karma = -24000; // Negative karma

            VirtualArmor = 100; // Enhanced armor

            Tamable = false; // Not tamable for boss version
            ControlSlots = 3;
            MinTameSkill = 100;

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("You dare challenge the mountain goat!");
            PackGold(1500, 2000); // Enhanced gold drops
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // More ingots for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities

            // You can also tweak ability behavior here, e.g. make them more frequent or stronger
        }

        public BossCliffGoat(Serial serial) : base(serial)
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
