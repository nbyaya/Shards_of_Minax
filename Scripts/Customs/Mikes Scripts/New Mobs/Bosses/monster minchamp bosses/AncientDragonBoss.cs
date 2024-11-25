using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an ancient dragon corpse")]
    public class AncientDragonBoss : AncientDragon
    {
        private DateTime m_NextAncientBreath;
        private DateTime m_NextDragonsRoar;
        private DateTime m_NextTemporalShield;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AncientDragonBoss()
            : base()
        {
            // Update name and appearance for the boss
            Name = "Ancient Dragon Overlord";
            Hue = 1490; // Unique hue for ancient dragon
            Title = "the Overlord";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Exceeds original stats (stronger)
            SetDex(255); // Maximum dexterity
            SetInt(250); // Maximum intelligence

            SetHits(12000); // Exceeds original hit points
            SetDamage(45, 60); // Increased damage range

            SetResistance(ResistanceType.Physical, 85, 90); // Higher resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 100.0);
            SetSkill(SkillName.MagicResist, 120.0, 150.0); // Enhanced magic resist
            SetSkill(SkillName.Tactics, 120.0, 150.0); // Improved tactics
            SetSkill(SkillName.Wrestling, 120.0, 150.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma

            VirtualArmor = 120; // Increased virtual armor

            // Attach the XmlRandomAbility for random abilities
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

            // Boss-specific logic can go here, e.g., abilities or behavior enhancements
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
            // Reinitialize abilities
        }
    }
}
