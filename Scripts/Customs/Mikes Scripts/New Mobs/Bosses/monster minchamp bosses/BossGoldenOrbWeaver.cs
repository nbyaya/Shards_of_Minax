using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a golden orb weaver corpse")]
    public class BossGoldenOrbWeaver : GoldenOrbWeaver
    {
        [Constructable]
        public BossGoldenOrbWeaver()
        {
            Name = "Golden Orb Weaver";
            Title = "the Radiant Terror";
            Hue = 0x497; // Unique hue for a boss
            Body = 28; // GiantSpider body

            SetStr(1200); // Enhanced strength for boss
            SetDex(255);
            SetInt(250);

            SetHits(15000); // Increased hit points for a boss

            SetDamage(40, 50); // Increased damage range for boss

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 170.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Higher fame for boss
            Karma = -30000;

            VirtualArmor = 120; // Increased virtual armor for a boss

            Tamable = false;
            ControlSlots = 3;

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
            this.Say("I will weave your doom!");
            PackGold(1500, 2000); // Enhanced gold drops
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // More ingots for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for web abilities
        }

        public BossGoldenOrbWeaver(Serial serial) : base(serial)
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
