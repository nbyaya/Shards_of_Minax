using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a dreaded creeper boss corpse")]
    public class DreadedCreeperBoss : DreadedCreeper
    {
        private DateTime m_NextDreadWave;
        private DateTime m_NextNightmareRoots;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public DreadedCreeperBoss() : base()
        {
            Name = "The Dreaded Overlord";
            Title = "the Nightmarish Boss";
            Body = 8; // Corpser body
            Hue = 1389; // Unique dark green hue
            BaseSoundID = 684;

            // Update stats to match or exceed Barracoon's level
            SetStr(1200, 1500);  // Enhanced strength
            SetDex(255, 300);     // Enhanced dexterity
            SetInt(250, 350);     // Enhanced intelligence

            SetHits(12000, 14000); // Enhanced health

            SetDamage(35, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);  // Improved skill levels
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 120.0, 130.0);

            Fame = 30000;  // Increased fame
            Karma = -30000; // Increased negative karma

            VirtualArmor = 120; // Enhanced virtual armor

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_AbilitiesInitialized = false; // Set the flag to false
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
            // Optionally, you could add more advanced logic here for the boss's AI
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

        public DreadedCreeperBoss(Serial serial) : base(serial)
        {
        }
    }
}
