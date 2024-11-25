using System;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a cursed white-tail corpse")]
    public class BossCursedWhiteTail : CursedWhiteTail
    {
        [Constructable]
        public BossCursedWhiteTail()
            : base()
        {
            Name = "Cursed White-tail";
            Title = "the Spectral Nightmare";
            Hue = 1152; // Unique boss hue for identification
            Body = 0xEA; // Keeping the GreatHart body

            // Enhanced stats, adjusted for the boss version
            SetStr(1500, 1800);
            SetDex(250, 350);
            SetInt(350, 500);

            SetHits(15000); // Boss-level health
            SetDamage(45, 60); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 120.0, 130.0);
            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);

            Fame = 30000; // Increased fame for the boss
            Karma = -30000;

            VirtualArmor = 120; // Increased armor

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
            this.Say("You will feel the wrath of my curse!");

            // Enhanced loot
            PackGold(2000, 3000);
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 8);
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for special attacks (Curse, Charge, Summon)
        }

        public BossCursedWhiteTail(Serial serial)
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
