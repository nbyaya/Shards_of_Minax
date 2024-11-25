using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a solar overlord corpse")]
    public class SolarElementalBoss : SolarElemental
    {
        private DateTime m_NextSolarFlare;
        private DateTime m_NextRadiantBurst;
        private DateTime m_NextSunsEmbrace;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SolarElementalBoss() : base()
        {
            Name = "Solar Overlord";
            Title = "the Supreme Solar Elemental";

            // Update stats to match or exceed Barracoon's stats or better
            SetStr(1200); // Enhanced strength
            SetDex(255); // Maximum dexterity
            SetInt(250); // Enhanced intelligence

            SetHits(12000); // Higher hit points than the regular Solar Elemental
            SetDamage(35, 45); // Increased damage range

            SetResistance(ResistanceType.Physical, 75); // Better resistance than the base Solar Elemental
            SetResistance(ResistanceType.Fire, 80); // Enhanced fire resistance
            SetResistance(ResistanceType.Cold, 60); // Slightly better cold resistance
            SetResistance(ResistanceType.Poison, 80); // Increased poison resistance
            SetResistance(ResistanceType.Energy, 60); // Better energy resistance

            SetSkill(SkillName.Anatomy, 50.0, 75.0); // Enhanced anatomy skill
            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Higher eval int skill
            SetSkill(SkillName.Magery, 110.0, 120.0); // Increased magery skill
            SetSkill(SkillName.Meditation, 50.0, 75.0); // Better meditation skill
            SetSkill(SkillName.MagicResist, 150.0, 175.0); // Enhanced magic resist skill
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Better tactics
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Increased wrestling skill

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
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

        public SolarElementalBoss(Serial serial)
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
