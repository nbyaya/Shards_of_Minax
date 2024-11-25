using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss emperor cobra corpse")]
    public class BossEmperorCobra : EmperorCobra
    {
        [Constructable]
        public BossEmperorCobra()
        {
            Name = "an Emperor Cobra";
            Title = "the Serpent King";
            Hue = 1777; // Retain unique hue for the boss
            Body = 0x15; // Giant Serpent body

            SetStr(1200); // Boss-level strength
            SetDex(255); // Boss-level dexterity
            SetInt(750); // Enhanced intelligence

            SetHits(12000); // Boss-level health

            SetDamage(29, 38); // Boss-level damage

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 80, 95);
            SetResistance(ResistanceType.Energy, 50, 65);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced resistance
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0); // More powerful magic skills

            Fame = 30000; // Enhanced fame
            Karma = -30000;

            VirtualArmor = 100; // Increased armor for a boss

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Add 5 MaxxiaScrolls to loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Retain original loot generation
            this.Say("The serpent king's treasure is yours... if you survive.");
            PackGold(2000, 3000); // Boss-level gold drop
            AddLoot(LootPack.FilthyRich, 3); // Enhanced loot pack
            AddLoot(LootPack.Gems, 15); // Additional gems
        }

        public BossEmperorCobra(Serial serial) : base(serial)
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
