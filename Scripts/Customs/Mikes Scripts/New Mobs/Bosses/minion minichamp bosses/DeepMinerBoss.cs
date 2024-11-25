using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the deep mining overlord")]
    public class DeepMinerBoss : DeepMiner
    {
        [Constructable]
        public DeepMinerBoss() : base()
        {
            Name = "Deep Mining Overlord";
            Title = "the Master of the Depths";

            // Update stats to match or exceed Barracoon's level (or better)
            SetStr(900, 1200); // Higher strength for boss tier
            SetDex(200, 250); // Higher dexterity for better combat agility
            SetInt(150, 200); // Boosted intelligence for stronger abilities

            SetHits(12000); // Higher health to match boss-tier difficulty
            SetStam(300); // Increased stamina to match boss behavior
            SetMana(750); // Max mana for casting stronger abilities

            SetDamage(29, 38); // Stronger damage to pose more of a threat

            SetResistance(ResistanceType.Physical, 75, 85); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // Stronger fire resistance
            SetResistance(ResistanceType.Cold, 80, 90); // Stronger cold resistance
            SetResistance(ResistanceType.Poison, 85, 90); // Stronger poison resistance
            SetResistance(ResistanceType.Energy, 80, 90); // Energy resistance

            SetSkill(SkillName.MagicResist, 100.0); // Improved magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Higher tactics skill
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling skill
            SetSkill(SkillName.Mining, 100.0); // Excellent mining skill

            Fame = 22500; // Increased fame to match a boss NPC
            Karma = -22500; // Negative karma for a villain

            VirtualArmor = 80; // Increased virtual armor for higher defense

            // Attach random ability to the boss
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new SpikeLineMateria());
			PackItem(new CraneSummoningMateria());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Additional boss-specific logic can be added here if desired
        }

        public DeepMinerBoss(Serial serial) : base(serial)
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
