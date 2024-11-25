using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a sky seraph overlord corpse")]
    public class SkySeraphBoss : SkySeraph
    {
        private DateTime m_NextCelestialWinds;
        private DateTime m_NextHeavenlyStrike;
        private DateTime m_NextAerialGrace;
        private bool m_AbilitiesActivated;

        [Constructable]
        public SkySeraphBoss() : base()
        {
            Name = "Sky Seraph Overlord";
            Title = "the Divine Fury";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Higher strength for boss-tier
            SetDex(255); // Max dexterity for extra agility
            SetInt(250); // Max intelligence for powerful spells

            SetHits(12000); // Higher health for a boss fight
            SetDamage(35, 45); // Enhanced damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50);

            SetSkill(SkillName.Anatomy, 50.0);
            SetSkill(SkillName.EvalInt, 110.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 110.0);
            SetSkill(SkillName.Wrestling, 110.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 120;

            Tamable = false; // Prevent taming for the boss
            ControlSlots = 0; // Not tamable
            MinTameSkill = 0; // Not tamable

            m_AbilitiesActivated = false;

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
            // Additional boss logic could be added here if desired
        }

        public SkySeraphBoss(Serial serial) : base(serial)
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

            m_AbilitiesActivated = false; // Reset flag to reinitialize on next OnThink call
        }
    }
}
