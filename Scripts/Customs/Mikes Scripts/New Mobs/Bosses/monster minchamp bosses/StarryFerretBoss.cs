using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a starry ferret corpse")]
    public class StarryFerretBoss : StarryFerret
    {
        private DateTime m_NextStarShower;
        private DateTime m_NextCosmicCloak;
        private DateTime m_NextGravityWarp;
        private DateTime m_NextStarlightBurst;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public StarryFerretBoss()
            : base()
        {
            Name = "Starry Ferret Overlord";
            Title = "the Cosmic Terror";
            Hue = 1569; // Starry hue (unchanged)
            BaseSoundID = 0xCF; // Base sound ID (unchanged)

            // Update stats to match boss-level stats
            SetStr(1200, 1500);  // Enhanced strength
            SetDex(255, 350);    // Enhanced dexterity
            SetInt(250, 350);    // Enhanced intelligence

            SetHits(15000, 18000);  // High health
            SetDamage(40, 60); // Higher damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 120.0, 150.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 150.0);
            SetSkill(SkillName.Wrestling, 120.0, 150.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 120; // Increased virtual armor

            // Attach a random ability (same as original)
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            Tamable = false;
            ControlSlots = 3; // Bosses are not tamable
            MinTameSkill = 100.0;

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
            // Boss-specific behaviors or timing tweaks can be added here if necessary
        }

        public StarryFerretBoss(Serial serial)
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
