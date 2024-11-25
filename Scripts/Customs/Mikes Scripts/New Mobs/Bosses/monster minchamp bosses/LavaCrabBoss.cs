using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a lava crab overlord corpse")]
    public class LavaCrabBoss : LavaCrab
    {
        private DateTime m_NextMoltenPull;
        private DateTime m_NextEruptionSlam;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public LavaCrabBoss()
            : base()
        {
            Name = "Lava Crab Overlord";
            Title = "the Fiery Terror";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Matching Barracoon's upper strength
            SetDex(255); // Matching Barracoon's upper dexterity
            SetInt(250); // Match original stats for intelligence

            SetHits(12000); // Boss health, same as Barracoon
            SetDamage(40, 50); // Higher damage for a boss

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0); // Boss level resist
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 30000; // Increased fame for a boss
            Karma = -30000; // Increased karma for a villain

            VirtualArmor = 100; // High virtual armor for a boss

            Tamable = false;
            ControlSlots = 0;

            m_AbilitiesInitialized = false; // Initialize flag

            // Attach the random ability
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
            // Additional boss logic could be added here, e.g., more frequent use of abilities or special attacks
        }

        public LavaCrabBoss(Serial serial)
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
