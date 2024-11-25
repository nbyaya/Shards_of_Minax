using System;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the acidic overlord")]
    public class AcidicAlligatorBoss : AcidicAlligator
    {
        private DateTime m_NextAcidicBite;
        private DateTime m_NextAcidSpray;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public AcidicAlligatorBoss() : base()
        {
            Name = "Acidic Overlord";
            Title = "the Acidic Tyrant";
            Hue = 1174; // Keep the unique hue for the boss variant
            BaseSoundID = 660;

            // Update stats to match or exceed the boss-tier specifications
            SetStr(1200, 1500); // Enhanced strength
            SetDex(255, 300); // Enhanced dexterity
            SetInt(250, 350); // Enhanced intelligence

            SetHits(12000); // Enhanced health
            SetDamage(40, 50); // Enhanced damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20); // Adjusted for a more dangerous spread

            SetResistance(ResistanceType.Physical, 80, 90); // Enhanced physical resistance
            SetResistance(ResistanceType.Fire, 75, 90); // Enhanced fire resistance
            SetResistance(ResistanceType.Cold, 60, 70); // Enhanced cold resistance
            SetResistance(ResistanceType.Poison, 100); // Full poison resistance
            SetResistance(ResistanceType.Energy, 60, 70); // Enhanced energy resistance

            SetSkill(SkillName.Anatomy, 50.1, 75.0); // Improved anatomy skill
            SetSkill(SkillName.EvalInt, 100.1, 125.0); // Enhanced evalint skill
            SetSkill(SkillName.Magery, 125.5, 150.0); // Enhanced magery skill
            SetSkill(SkillName.Meditation, 50.1, 75.0); // Improved meditation skill
            SetSkill(SkillName.MagicResist, 150.5, 200.0); // Enhanced magic resist skill
            SetSkill(SkillName.Tactics, 125.1, 150.0); // Enhanced tactics skill
            SetSkill(SkillName.Wrestling, 125.1, 150.0); // Enhanced wrestling skill

            Fame = 30000; // Increased fame for a boss-tier mob
            Karma = -30000; // Increased negative karma for the boss

            VirtualArmor = 100; // Increased virtual armor for more defense

            Tamable = false; // Make sure it's untamable for boss fights
            ControlSlots = 0; // Cannot be controlled

            m_AbilitiesInitialized = false; // Initialize the flag
            XmlAttach.AttachTo(this, new XmlRandomAbility()); // Attach the random ability
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }

            this.AddLoot(LootPack.FilthyRich, 2);
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, 10);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here
        }

        public AcidicAlligatorBoss(Serial serial) : base(serial)
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize to reinitialize abilities
            m_NextAcidicBite = DateTime.UtcNow; // Reset to current time
            m_NextAcidSpray = DateTime.UtcNow; // Reset to current time
        }
    }
}
