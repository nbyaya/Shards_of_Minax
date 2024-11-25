using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Riptide Crab corpse")]
    public class RiptideCrabBoss : RiptideCrab
    {
        private DateTime m_NextTidalPull;
        private DateTime m_NextAbyssalSlam;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public RiptideCrabBoss()
            : base()
        {
            Name = "Riptide Crab Overlord";
            Title = "the Abyssal Terror";

            // Enhance stats to match or exceed Barracoon (or better)
            SetStr(1200, 1500); // Stronger strength
            SetDex(255, 300); // Higher dexterity
            SetInt(250, 350); // Higher intelligence

            SetHits(15000); // Higher health than the base version
            SetDamage(40, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 120); // Higher poison resistance
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 80.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 80.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0); // Higher magic resist
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 30000; // Higher fame for boss-tier creature
            Karma = -30000; // Negative karma for a hostile boss

            VirtualArmor = 100; // Increased virtual armor

            Tamable = false; // The boss cannot be tamed
            ControlSlots = 0; // Boss is not controllable by players

            // Initialize the ability cooldowns and flag
            m_AbilitiesInitialized = false;

            // Attach a random ability to the boss
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Add 5 MaxxiaScrolls in addition to the regular loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here if needed
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
