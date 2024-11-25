using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss sand golem corpse")]
    public class BossSandGolem : SandGolem
    {
        [Constructable]
        public BossSandGolem()
            : base()
        {
            Name = "The Sand Golem";
            Title = "the Colossus";
            Hue = 1927; // Unique hue for the boss
            Body = 752; // Default body of Sand Golem

            SetStr(1200, 1600); // Enhanced strength
            SetDex(255, 300);  // Enhanced dexterity
            SetInt(300, 500);  // Enhanced intelligence

            SetHits(12000);    // Boss-level health

            SetDamage(40, 50); // Enhanced damage

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 110.0, 130.0);
            SetSkill(SkillName.Wrestling, 110.0, 130.0);

            Fame = 30000;    // Boss-level fame
            Karma = -30000;  // Boss-level karma

            VirtualArmor = 100; // Boss-level armor

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
            this.Say("You face the wrath of the Sand Golem!");
            PackGold(1500, 2000); // Enhanced gold drops
            AddLoot(LootPack.FilthyRich, 3); // Increased loot quality
            AddLoot(LootPack.Gems, 12); // Increased gem drop


        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities like Sandstorm, Mirage, etc.
        }

        public BossSandGolem(Serial serial)
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
