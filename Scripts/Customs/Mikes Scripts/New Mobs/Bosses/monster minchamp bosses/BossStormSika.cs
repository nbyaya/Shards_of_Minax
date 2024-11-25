using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss storm sika corpse")]
    public class BossStormSika : StormSika
    {
        [Constructable]
        public BossStormSika()
            : base()
        {
            Name = "A Storm Sika";
            Title = "the Tempest";
            Hue = 1971; // Stormy grey hue, can modify for uniqueness

            SetStr(1200, 1500); // Enhanced strength for the boss
            SetDex(255, 300); // Higher dexterity
            SetInt(250, 350); // Higher intelligence

            SetHits(15000); // Boss-level health

            SetDamage(40, 60); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Improved resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resist
            SetSkill(SkillName.Tactics, 120.0, 130.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 120.0, 130.0); // Enhanced wrestling
            SetSkill(SkillName.Magery, 120.0, 130.0); // Enhanced magery for abilities

            Fame = 30000; // Increased fame for the boss
            Karma = -30000; // Negative karma for the boss

            VirtualArmor = 100; // Higher virtual armor

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // 5 MaxxiaScroll drops
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("The storm is upon you!");
            PackGold(1000, 1500); // Increased gold drop
            PackItem(new IronIngot(Utility.RandomMinMax(50, 100))); // More iron ingots for the boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain the original thinking logic, includes abilities
        }

        public BossStormSika(Serial serial) : base(serial)
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
