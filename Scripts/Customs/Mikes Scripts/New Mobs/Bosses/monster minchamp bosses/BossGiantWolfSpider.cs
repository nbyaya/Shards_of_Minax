using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a giant wolf spider corpse")]
    public class BossGiantWolfSpider : GiantWolfSpider
    {
        [Constructable]
        public BossGiantWolfSpider()
            : base()
        {
            Name = "Giant Wolf Spider";
            Title = "the Titan";
            Hue = 0x497; // Unique hue for the boss
            Body = 28; // Default Giant Spider body

            SetStr(1500, 2000); // Enhanced strength
            SetDex(255, 300); // Enhanced dexterity
            SetInt(300, 400); // Enhanced intelligence

            SetHits(15000); // Boss-level health
            SetDamage(45, 60); // Increased damage

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 50.0);

            Fame = 30000; // Boss-level fame
            Karma = -30000; // Boss-level karma

            VirtualArmor = 100; // Enhanced armor

            // Attach the random ability for extra unpredictability
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot drop
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Drop 5 MaxxiaScrolls
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            PackGold(1000, 2000); // Increased gold drop
            AddLoot(LootPack.FilthyRich, 2); // Add richer loot
            AddLoot(LootPack.Gems, 10); // More gems for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original behavior for abilities
        }

        public BossGiantWolfSpider(Serial serial)
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
