using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss goral corpse")]
    public class BossGoral : Goral
    {
        [Constructable]
        public BossGoral()
            : base()
        {
            // Override name, title, and appearance for a boss-level creature
            Name = "Goral";
            Title = "the Colossus";
            Hue = 0x497; // Unique hue for the boss
            Body = 0xD1; // Goat body (same as base)

            // Enhanced stats
            SetStr(1600); // Enhanced strength for boss-level creature
            SetDex(300);  // Enhanced dexterity
            SetInt(400);  // Increased intelligence for more abilities

            SetHits(15000); // Increased health
            SetDamage(40, 50); // Increased damage output

            SetDamageType(ResistanceType.Physical, 60);  // Higher physical damage resistance
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 30);

            // Enhanced resistances for the boss
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 65, 75);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Higher skills for more challenging combat
            SetSkill(SkillName.Anatomy, 75.0, 100.0);
            SetSkill(SkillName.EvalInt, 120.0, 150.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 75.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 150.0);
            SetSkill(SkillName.Wrestling, 120.0, 150.0);

            Fame = 35000;  // Higher fame to match the boss
            Karma = -35000;

            VirtualArmor = 100; // Enhanced virtual armor

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Dropping 5 MaxxiaScroll items
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Retain base loot
            this.Say("You will regret facing me!");
            PackGold(2000, 2500); // More gold drop for a boss-level creature
            PackItem(new IronIngot(Utility.RandomMinMax(100, 200))); // Extra ingots as bonus loot
        }

        public override void OnThink()
        {
            base.OnThink(); // Keep the original ability logic intact

            // Additional checks can be added here if needed to further enhance boss behavior
        }

        public BossGoral(Serial serial)
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
