using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss sable antelope corpse")]
    public class BossSableAntelope : SableAntelope
    {
        [Constructable]
        public BossSableAntelope()
        {
            Name = "Supreme Sable Antelope";
            Title = "the Majestic";
            Hue = 0x4B; // A unique hue for a boss-level antelope
            Body = 0xD1; // Using goat body, same as original
            BaseSoundID = 0x99; // Same sound as original

            SetStr(1200, 1500); // Enhanced strength
            SetDex(255, 300); // Enhanced dexterity
            SetInt(250, 350); // Enhanced intelligence

            SetHits(12000, 15000); // Boss-level health

            SetDamage(40, 50); // Boss-level damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 90); // Enhanced resistance
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0); // Higher skill levels
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0); // Boss-level resist
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 30000; // Higher fame for a boss
            Karma = -30000;

            VirtualArmor = 100; // Increased armor

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Drops 5 MaxxiaScrolls
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("You dare challenge me?");
            PackGold(1500, 2000); // Enhanced loot
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // More ingots
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities
        }

        public BossSableAntelope(Serial serial) : base(serial)
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
