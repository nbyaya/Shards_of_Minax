using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss trapdoor spider corpse")]
    public class BossTrapdoorSpider : GiantTrapdoorSpider
    {
        [Constructable]
        public BossTrapdoorSpider() : base()
        {
            Name = "Trapdoor Spider";
            Title = "the Terrifying";
            Hue = 0x497; // Boss-specific hue (for visual distinction)
            Body = 28; // Same body as the original

            // Enhanced stats for boss
            SetStr(1500, 2000);
            SetDex(300, 400);
            SetInt(300, 500);

            SetHits(15000, 18000); // High health for boss

            SetDamage(40, 60); // Increased damage range

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 75, 90);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 85, 100);
            SetResistance(ResistanceType.Energy, 65, 80);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);
            SetSkill(SkillName.Poisoning, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);

            Fame = 30000; // Higher fame for a boss-level creature
            Karma = -30000; // Negative karma for a villainous boss

            VirtualArmor = 120; // Increased virtual armor

            Tamable = false; // Boss-level creatures are not tamable
            ControlSlots = 0; // No control slots for tamed versions

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Add loot drop customization: 5 MaxxiaScrolls along with the normal loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Retain the base loot generation
            this.Say("You will never escape my web of doom!");
            PackGold(2000, 3000); // Enhanced gold drops for boss
            PackItem(new IronIngot(Utility.RandomMinMax(100, 200))); // More valuable resources
            PackItem(new TrapdoorWeb()); // Add a special item drop related to the boss's abilities
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain the base thinking logic
        }

        public BossTrapdoorSpider(Serial serial) : base(serial)
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
