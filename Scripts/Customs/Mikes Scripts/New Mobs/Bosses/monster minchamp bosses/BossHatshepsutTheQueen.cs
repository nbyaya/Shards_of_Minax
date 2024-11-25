using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss HatshepsutTheQueen corpse")]
    public class BossHatshepsutTheQueen : HatshepsutTheQueen
    {
        [Constructable]
        public BossHatshepsutTheQueen() : base()
        {
            Name = "Boss HatshepsutTheQueen";
            Title = "the Enforcer";

            SetStr(1200); // Boss-level strength
            SetDex(255); // Maxed out dexterity
            SetInt(250); // High intelligence for the boss

            SetHits(15000); // Boss-level health
            SetDamage(39, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Higher resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 100.0); // Improved skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 35000; // High fame for a boss
            Karma = -35000;

            VirtualArmor = 120; // Higher armor

            // Attach the XmlRandomAbility for additional random effects
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Inherit base loot
            this.Say("Feel the wrath!");
            PackGold(1000, 1500); // Increased gold drops for the boss
            PackItem(new IronIngot(Utility.RandomMinMax(50, 100))); // More ingots for the boss
            AddLoot(LootPack.FilthyRich, 2); // Additional high-end loot
            AddLoot(LootPack.Rich); // Standard rich loot
            AddLoot(LootPack.Gems, 8); // High number of gems
        }

        public override void OnThink()
        {
            base.OnThink(); // Inherit the OnThink behavior
        }

        public BossHatshepsutTheQueen(Serial serial) : base(serial)
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
