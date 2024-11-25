using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the dual-wielding overlord")]
    public class DualWielderBoss : DualWielder
    {
        [Constructable]
        public DualWielderBoss() : base()
        {
            Name = "Dual-Wielding Overlord";
            Title = "the Supreme Swordsman";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Matching Barracoon's upper strength
            SetDex(255);  // Matching the max dexterity of the original
            SetInt(250);  // Using the original upper intelligence

            SetHits(10000); // Increase hits for the boss-tier difficulty
            SetDamage(25, 35); // Boosted damage range for a boss-tier NPC

            SetResistance(ResistanceType.Physical, 80, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Swords, 120.0, 140.0);

            Fame = 10000;  // Increased Fame for a boss-tier NPC
            Karma = -10000;  // Negative Karma for a villain

            VirtualArmor = 70;

            // Attach a random ability for extra effects
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new CityBanner());
			PackItem(new CaptainCooksTreasure());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            PackGold(500, 750);
            AddLoot(LootPack.Rich);
        }

        public override void OnThink()
        {
            base.OnThink();

            // Optional: Add more boss-specific behavior here
        }

        public DualWielderBoss(Serial serial) : base(serial)
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
