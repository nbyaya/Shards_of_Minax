using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a raging alligator boss corpse")]
    public class RagingAlligatorBoss : RagingAlligator
    {
        private DateTime m_NextRage;
        private DateTime m_NextGroundSlam;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public RagingAlligatorBoss()
            : base()
        {
            Name = "Raging Alligator Boss";
            Title = "the Overlord";

            // Update stats to make it boss-tier
            SetStr(1500, 2000); // Increased Strength for a boss-level challenge
            SetDex(255, 300);   // Increased Dexterity for improved speed and attack rate
            SetInt(300, 400);   // Increased Intelligence for stronger magical abilities

            SetHits(16000, 18000); // Significantly higher health

            SetDamage(50, 60); // Increased damage range for higher difficulty

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 120);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 90.0, 120.0);
            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 90.0, 120.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 30000; // Higher Fame for a tougher boss
            Karma = -30000; // Negative karma for an evil boss

            VirtualArmor = 100; // Increased armor to make it tougher

            Tamable = false; // Boss is untamable

            m_AbilitiesInitialized = false; // Initialize the flag for special abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility()); // Attach a random ability
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
            // Additional boss logic could be added here
        }

        public RagingAlligatorBoss(Serial serial)
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
