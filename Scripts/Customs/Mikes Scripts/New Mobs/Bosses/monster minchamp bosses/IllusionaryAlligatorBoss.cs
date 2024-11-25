using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the illusionary alligator boss")]
    public class IllusionaryAlligatorBoss : IllusionaryAlligator
    {
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public IllusionaryAlligatorBoss()
            : base()
        {
            Name = "Illusionary Alligator Boss";
            Title = "the Supreme Predator";

            // Update stats to match or exceed Barracoon-like boss standards
            SetStr(1200); // Increase Strength
            SetDex(255); // Maximum Dexterity
            SetInt(250); // Max Intelligence

            SetHits(12000); // Increase health significantly
            SetDamage(40, 50); // Higher damage range to make it more threatening

            SetDamageType(ResistanceType.Physical, 60); // More physical damage
            SetDamageType(ResistanceType.Fire, 30); // Slight fire damage
            SetDamageType(ResistanceType.Energy, 10); // Small energy damage

            SetResistance(ResistanceType.Physical, 80, 90); // Increase physical resistance
            SetResistance(ResistanceType.Fire, 70, 90); // Increase fire resistance
            SetResistance(ResistanceType.Cold, 60, 80); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 100); // Max poison resistance
            SetResistance(ResistanceType.Energy, 60, 80); // Increased energy resistance

            SetSkill(SkillName.Anatomy, 50.0); // Enhance skill in Anatomy
            SetSkill(SkillName.EvalInt, 120.0); // Increase EvalInt for magic effectiveness
            SetSkill(SkillName.Magery, 120.0); // Increase Magery skill for stronger magical abilities
            SetSkill(SkillName.Meditation, 50.0); // Increase Meditation skill
            SetSkill(SkillName.MagicResist, 150.0); // High Magic Resist
            SetSkill(SkillName.Tactics, 120.0); // Increased Tactics for better combat behavior
            SetSkill(SkillName.Wrestling, 120.0); // Higher Wrestling skill for physical combat

            Fame = 30000; // Higher fame
            Karma = -30000; // Negative karma for a boss-tier creature

            VirtualArmor = 90; // Increase virtual armor for extra defense

            Tamable = false; // The boss should not be tamable
            ControlSlots = 0; // Not tamable
            MinTameSkill = 0;

            m_AbilitiesInitialized = false; // Initialize ability flag

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
            // Additional boss logic could be added here
        }

        public IllusionaryAlligatorBoss(Serial serial)
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
