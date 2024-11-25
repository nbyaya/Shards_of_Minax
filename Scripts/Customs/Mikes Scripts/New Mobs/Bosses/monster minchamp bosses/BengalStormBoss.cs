using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Bengal Storm overlord corpse")]
    public class BengalStormBoss : BengalStorm
    {
        private DateTime m_NextLightningPaw;
        private DateTime m_NextThunderclap;
        private DateTime m_NextStormCloak;
        private bool m_IsStormCloakActive;
        private bool m_AbilitiesInitialized; // Flag to track if random intervals have been initialized

        [Constructable]
        public BengalStormBoss() : base()
        {
            Name = "Bengal Storm Overlord";
            Title = "the Tempest Bringer";
            Hue = 1301; // Lightning-themed hue
            BaseSoundID = 0x69; // Standard cat sound ID

            // Update stats to match or exceed the original BengalStorm
            SetStr(1200, 1500); // Enhanced strength
            SetDex(255, 300); // Enhanced dexterity
            SetInt(250, 350); // Enhanced intelligence

            SetHits(15000); // Increased health for a boss-tier NPC

            SetDamage(40, 50); // Enhanced damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0); // Enhanced skill levels
            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 30000; // Boss-level fame
            Karma = -30000; // Boss-level karma

            VirtualArmor = 100; // Increased armor for added defense

            // Initialize the abilities
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

            // Optional: Additional boss-specific AI or abilities could go here
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
