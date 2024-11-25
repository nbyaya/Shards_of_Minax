using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;
using Server.Network;
using Server.Items;


namespace Server.Mobiles
{
    [CorpseName("a shadow crab overlord corpse")]
    public class ShadowCrabBoss : ShadowCrab
    {
        private DateTime m_NextShadowPull;
        private DateTime m_NextDarkSlash;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ShadowCrabBoss()
            : base()
        {
            Name = "Shadow Crab Overlord";
            Title = "the Supreme Shadow Crab";

            // Update stats to match or exceed Barracoon-style boss NPC
            SetStr(1200); // Stronger strength
            SetDex(255); // High dexterity
            SetInt(250); // High intelligence

            SetHits(12000); // Increased health
            SetDamage(35, 45); // Increased damage

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced Magic Resist
            SetSkill(SkillName.Tactics, 120.0); // Higher tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling skill
            SetSkill(SkillName.EvalInt, 120.0); // Boosted EvalInt for magic resistance and damage

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90; // High virtual armor for tankiness

            Tamable = false; // Boss cannot be tamed
            ControlSlots = 0; // No control slots since it is a boss

            // Attach the XmlRandomAbility to provide additional random effects
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

            // Additional boss loot
            AddLoot(LootPack.FilthyRich, 2); // Rich loot
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 8); // Extra gems
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here if needed
        }

        public ShadowCrabBoss(Serial serial)
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
