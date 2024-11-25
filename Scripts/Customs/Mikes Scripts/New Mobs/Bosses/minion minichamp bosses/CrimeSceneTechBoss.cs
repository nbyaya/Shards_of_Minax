using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the tech overlord")]
    public class CrimeSceneTechBoss : CrimeSceneTech
    {
        [Constructable]
        public CrimeSceneTechBoss() : base()
        {
            Name = "Tech Overlord";
            Title = "the Supreme Gadgeteer";

            // Update stats to match or exceed Barracoon or the highest possible boss tier stats
            SetStr(800); // Enhancing strength
            SetDex(200); // Enhancing dexterity
            SetInt(500); // Enhancing intelligence

            SetHits(10000); // Setting high health for a boss-tier creature
            SetStam(300); // Boosting stamina
            SetMana(600); // Boosting mana

            SetDamage(20, 40); // Improving damage range for a more formidable threat

            // Enhancing resistances for a boss
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 100.0);

            Fame = 25000; // Enhanced fame
            Karma = -25000; // Enhanced karma (negative for a boss enemy)

            VirtualArmor = 60; // Increasing virtual armor for additional defense

            // Attach the random ability XML attachment
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new BarracoonSummoningMateria());
			PackItem(new HutFlower());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            // Drop additional valuable items
            PackGold(500, 600); // Enhanced gold drop for a boss
            AddLoot(LootPack.FilthyRich); // Rich loot pack for a boss-tier NPC

            // Say something intimidating when defeated
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "My gadgets... failed me..."); break;
                case 1: this.Say(true, "This isn't over..."); break;
            }

            // Additional item drop
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 20)));
        }

        public override void OnThink()
        {
            base.OnThink();

            // Additional boss logic could go here if needed
        }

        public CrimeSceneTechBoss(Serial serial) : base(serial)
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
