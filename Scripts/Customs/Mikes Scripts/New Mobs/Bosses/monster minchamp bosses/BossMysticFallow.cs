using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss mystic fallow corpse")]
    public class BossMysticFallow : MysticFallow
    {
        [Constructable]
        public BossMysticFallow()
        {
            Name = "Mystic Fallow";
            Title = "the Eternal Sage";
            Hue = 1976; // Ethereal hue
            Body = 0xEA; // GreatHart body (can randomize if needed)

            SetStr(1200, 1500); // Higher strength for boss
            SetDex(255, 300);   // Higher dexterity for boss
            SetInt(300, 400);   // Higher intelligence for boss

            SetHits(15000); // Boss-level health
            SetDamage(40, 50); // Increased damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85); // Increased resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0); // Increased skill range
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Boss-level magic resist
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Boss-level fame
            Karma = -30000;

            VirtualArmor = 100; // Higher armor for the boss

            Tamable = false; // Non-tamable boss
            ControlSlots = 3;
            MinTameSkill = 0; // Disallow taming

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Dropping 5 MaxxiaScrolls
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("You shall not defeat me!");
            PackGold(1000, 2000); // Enhanced gold drop for the boss
            AddLoot(LootPack.FilthyRich, 2); // Richer loot pack
            AddLoot(LootPack.Rich); // Rich loot pack
            AddLoot(LootPack.Gems, 10); // Increased gems
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original behavior
        }

        public BossMysticFallow(Serial serial) : base(serial)
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
