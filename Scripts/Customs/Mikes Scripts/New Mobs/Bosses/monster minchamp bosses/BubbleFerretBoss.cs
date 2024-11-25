using System;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a bubble ferret boss corpse")]
    public class BubbleFerretBoss : BubbleFerret
    {
        private DateTime m_NextBubbleBurst;
        private DateTime m_NextBubbleShield;
        private DateTime m_BubbleShieldEnd;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BubbleFerretBoss() : base()
        {
            Name = "Bubble Ferret Overlord";
            Title = "the Supreme Bubbler";
            Hue = 1578; // Unique blue hue
            BaseSoundID = 0xCF;

            // Update stats to match or exceed Barracoon (or better)
            SetStr(1200); // Increased strength
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Increased health to match boss-tier
            SetDamage(35, 45); // Increased damage range

            // Update resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            // Enhanced skills
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 80.0);
            SetSkill(SkillName.EvalInt, 120.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma (negative for boss-tier)

            VirtualArmor = 100; // Increased virtual armor

            Tamable = false; // No taming
            ControlSlots = 0; // Not tamable

            m_AbilitiesInitialized = false; // Initialize flag

            PackItem(new BossTreasureBox());
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
            // Additional boss logic could be added here
        }

        public BubbleFerretBoss(Serial serial) : base(serial)
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

            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
