using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Sahiwal Shaman boss corpse")]
    public class SahiwalShamanBoss : SahiwalShaman
    {
        private DateTime m_NextNaturesGrasp;
        private DateTime m_NextHealingRain;
        private DateTime m_NextSummonSpirit;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SahiwalShamanBoss()
            : base()
        {
            Name = "Sahiwal Shaman Boss";
            Title = "the Spiritbinder";

            // Update stats to match or exceed Barracoon's stats for boss-tier difficulty
            SetStr(1200); // Boss-tier strength
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence
            
            SetHits(12000); // Increased health for the boss-tier
            SetDamage(35, 45); // Increased damage range for more threat

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 30000; // Increased Fame to reflect boss status
            Karma = -30000; // Negative Karma for the boss

            VirtualArmor = 100; // Increased virtual armor for tanking

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_AbilitiesInitialized = false;
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Optional: Additional boss-specific AI or abilities could go here
        }

        public SahiwalShamanBoss(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
