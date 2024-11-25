using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss blood serpent corpse")]
    public class BossBloodSerpent : BloodSerpent
    {
        private bool m_AbilitiesInitialized;

        [Constructable]
        public BossBloodSerpent()
            : base()
        {
            Name = "Boss Blood Serpent";
            Title = "the Devourer of Souls";
            Hue = 1150; // Boss-specific hue for a more menacing look
            Body = 0x15; // Same body as the original

            SetStr(1200, 1500); // Boosted stats for a boss-level creature
            SetDex(250, 350);
            SetInt(250, 350);

            SetHits(15000, 20000); // Increased health for a boss
            SetDamage(40, 50); // Increased damage for a boss

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.Anatomy, 75.0, 100.0);
            SetSkill(SkillName.EvalInt, 95.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 110.0, 130.0);
            SetSkill(SkillName.Wrestling, 110.0, 130.0);

            Fame = 30000; // Boss-level fame
            Karma = -30000; // Negative karma for this boss

            VirtualArmor = 100; // Increased virtual armor for the boss

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Set abilities initialization flag to false, and initialize them
            m_AbilitiesInitialized = false;
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("The Blood Serpent will not be defeated easily!");
            PackGold(2000, 3000); // Increased gold for a boss
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 10);
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Add 5 MaxxiaScrolls as requested
            }
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for using abilities like constriction and projection
        }

        public BossBloodSerpent(Serial serial) : base(serial)
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
            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }
}
