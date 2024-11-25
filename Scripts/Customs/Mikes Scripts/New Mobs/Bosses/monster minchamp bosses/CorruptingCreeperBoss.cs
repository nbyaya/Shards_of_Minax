using System;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a corrupted vine corpse")]
    public class CorruptingCreeperBoss : CorruptingCreeper
    {
        private DateTime m_NextDecayField;
        private DateTime m_NextCorruptionTouch;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CorruptingCreeperBoss() : base()
        {
            Name = "Corrupting Creeper";
            Title = "the Decayed Overlord";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Increased strength for boss-tier
            SetDex(255); // Max dexterity
            SetInt(250); // High intelligence for better spellcasting

            SetHits(12000); // Boss-tier health
            SetDamage(35, 45); // Increased damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.Anatomy, 50.0, 70.0);
            SetSkill(SkillName.EvalInt, 120.0, 130.0);
            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;

            Tamable = false;
            ControlSlots = 3;
            MinTameSkill = 100.0;

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_AbilitiesInitialized = false; // Initialize flag
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

        public CorruptingCreeperBoss(Serial serial) : base(serial)
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
