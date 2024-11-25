using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss muck golem corpse")]
    public class BossMuckGolem : MuckGolem
    {
        [Constructable]
        public BossMuckGolem()
            : base()
        {
            Name = "Boss Muck Golem";
            Title = "the Swamp Colossus";
            Hue = 0x497; // Unique hue for a boss
            Body = 752; // Retaining the original body type for the boss

            // Enhanced stats compared to original MuckGolem
            SetStr(1600, 1800);
            SetDex(255);
            SetInt(300, 400);

            SetHits(15000); // Boss-level health
            SetDamage(45, 60); // Boss-level damage

            SetResistance(ResistanceType.Physical, 85, 95);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 85, 95);
            SetResistance(ResistanceType.Energy, 70, 85);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 120.0, 150.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Increased fame for the boss
            Karma = -30000; // Negative karma for a boss

            VirtualArmor = 100; // Boss-level armor

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Drop additional 5 MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("You shall not escape the swamp!");
            PackGold(1500, 2000); // Enhanced gold drop
            PackItem(new IronIngot(Utility.RandomMinMax(100, 200))); // Increased ingot drops for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for Mud Sling, Quicksand Trap, and Swamp Meld
        }

        public BossMuckGolem(Serial serial)
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
