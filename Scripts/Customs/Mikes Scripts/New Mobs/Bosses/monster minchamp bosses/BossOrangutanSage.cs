using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a powerful orangutan sage corpse")]
    public class BossOrangutanSage : OrangutanSage
    {
        [Constructable]
        public BossOrangutanSage()
            : base()
        {
            // Enhancing stats for the boss version
            Name = "Boss Orangutan Sage";
            Title = "the Ancient Guardian";
            Hue = 0x4E3; // Unique hue for the boss version
            Body = 0x1D; // Keeping the Gorilla body

            SetStr(1500, 1800); // Enhanced strength
            SetDex(255, 300); // Enhanced dexterity
            SetInt(300, 400); // Enhanced intelligence

            SetHits(15000, 18000); // Boss-level health

            SetDamage(45, 60); // Boss-level damage

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.MagicResist, 150.0, 170.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 30000; // Higher fame for boss
            Karma = -30000; // Negative karma for a boss enemy

            VirtualArmor = 100; // Increased armor

            Tamable = false; // Boss should not be tamable
            ControlSlots = 0; // No control slots for this boss

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Enhanced loot drops
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Boss drops 5 MaxxiaScrolls
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Keep original loot
            this.Say("You cannot hope to defeat me, mortal!");
            PackGold(2000, 3000); // Increased gold drop for the boss
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // Boss drops more resources
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities like leap, camouflage, etc.
        }

        public BossOrangutanSage(Serial serial)
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
