using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss chimpanzee berserker corpse")]
    public class BossChimpanzeeBerserker : ChimpanzeeBerserker
    {
        [Constructable]
        public BossChimpanzeeBerserker() : base()
        {
            Name = "Boss Chimpanzee Berserker";
            Title = "the Colossus of the Jungle";
            Hue = 1968; // Unique hue for the boss
            Body = 0x1D; // Gorilla body (default)

            SetStr(1200, 1500); // Higher strength
            SetDex(255, 300);   // Higher dexterity
            SetInt(250, 350);   // Higher intelligence

            SetHits(15000);     // Higher health for boss
            SetDamage(50, 60);  // Boss-level damage

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 130.0);
            SetSkill(SkillName.Magery, 100.0, 120.0); // Enhance magical abilities
            SetSkill(SkillName.Meditation, 50.0, 70.0);

            Fame = 30000; // High fame for boss
            Karma = -30000; // High negative karma for boss

            VirtualArmor = 100; // Stronger armor for the boss

            Tamable = false; // Boss is not tamable
            ControlSlots = 3;
            MinTameSkill = 0;

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
            this.Say("Prepare to face the wrath of the jungle!");
            PackGold(2000, 3000); // Enhanced gold drops
            AddLoot(LootPack.FilthyRich, 4); // Richer loot
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities

            // You may want to further tweak how frequently certain abilities happen, depending on how difficult you want the boss.
        }

        public BossChimpanzeeBerserker(Serial serial) : base(serial)
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
