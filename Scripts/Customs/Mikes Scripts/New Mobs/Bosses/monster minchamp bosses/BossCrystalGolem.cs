using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss crystal golem corpse")]
    public class BossCrystalGolem : CrystalGolem
    {
        [Constructable]
        public BossCrystalGolem()
        {
            Name = "Crystal Golem";
            Title = "the Colossus";
            Hue = 0x497; // Unique hue for a boss
            Body = 752; // Default Golem body

            SetStr(1200); // Enhanced strength for the boss
            SetDex(255);
            SetInt(250);

            SetHits(12000); // Boss-level health
            SetDamage(35, 50); // Increased damage

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.Anatomy, 50.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000; // Boss-level fame
            Karma = -24000;

            VirtualArmor = 90; // Retain the original armor value

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot: Add 5 MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is an existing item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Retain the original loot generation
            this.Say("My crystalline form will shatter your hope!");
            PackGold(1000, 1500); // Enhanced gold drops
            PackItem(new CrystalShards()); // Maybe a unique item as a drop for the boss
            PackItem(new Diamond(Utility.RandomMinMax(5, 10))); // Add gems as a boss reward
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original behavior for abilities
        }

        public BossCrystalGolem(Serial serial) : base(serial)
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
