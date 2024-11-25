using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss spider monkey corpse")]
    public class BossSpiderMonkey : SpiderMonkey
    {
        [Constructable]
        public BossSpiderMonkey()
        {
            Name = "Boss Spider Monkey";
            Title = "the Alpha";
            Hue = 1960; // Unique hue for a boss
            Body = 0x1D; // Same body as SpiderMonkey

            SetStr(1200); // Increased strength for boss-level fight
            SetDex(255);
            SetInt(250);

            SetHits(15000); // Increased health for boss-level challenge
            SetDamage(45, 55); // Enhanced damage

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 120.0, 150.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 50.0, 80.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 30000; // Increased fame for the boss
            Karma = -30000; // Negative karma for the boss

            VirtualArmor = 120; // High armor for the boss

            Tamable = false; // The boss is untamable
            ControlSlots = 0; // Bosses don't need control slots


            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Retain original loot
            this.Say("Feel my wrath!");
            PackGold(2000, 3000); // Increased gold drop for boss
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // More ingots for a tougher boss

            // Additional loot: 5 MaxxiaScrolls for the boss fight
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities
        }

        public BossSpiderMonkey(Serial serial) : base(serial)
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
