using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a goliath birdeater corpse")]
    public class BossGoliathBirdeater : GoliathBirdeater
    {
        [Constructable]
        public BossGoliathBirdeater()
        {
            Name = "Goliath Birdeater";
            Title = "the Colossus";
            Hue = 0x497; // Unique hue for the boss
            Body = 28; // Same as GiantSpider, you can keep it consistent
            BaseSoundID = 0x388;

            SetStr(1200, 1500); // Enhanced strength, compared to the original Goliath Birdeater
            SetDex(255, 300);
            SetInt(250, 350);

            SetHits(15000, 18000); // Boss-level health
            SetDamage(40, 55); // Enhanced damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Boss-level fame
            Karma = -30000; // Boss-level karma

            VirtualArmor = 100; // Boss-level armor

            Tamable = false; // Boss shouldn't be tamable
            ControlSlots = 0; // No control slots needed

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
            this.Say("You shall be crushed by my bite!");
            PackGold(1000, 1500); // Enhanced loot
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 8);
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // More ingots for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for unique abilities like CrushingBite, TerrifyingDisplay, etc.
        }

        public BossGoliathBirdeater(Serial serial) : base(serial)
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
