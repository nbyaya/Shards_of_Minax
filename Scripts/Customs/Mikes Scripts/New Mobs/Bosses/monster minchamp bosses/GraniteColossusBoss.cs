using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a granite colossus corpse")]
    public class GraniteColossusBoss : GraniteColossus
    {
        private DateTime m_NextGraniteSlam;
        private DateTime m_NextGraniteArmor;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public GraniteColossusBoss() : base()
        {
            Name = "Granite Colossus, the Titan";
            Title = "the Earthbreaker";
            Hue = 1439; // Grayish granite hue
            Body = 14; // Large, stone-like body

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200, 1500); // Enhanced strength
            SetDex(255); // Maximum dexterity for a more mobile boss
            SetInt(750); // High intelligence to match Barracoon

            SetHits(15000); // Higher health for the boss version

            SetDamage(40, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 90, 105); // Enhanced physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // Strong fire resistance
            SetResistance(ResistanceType.Cold, 60, 70); // Enhanced cold resistance
            SetResistance(ResistanceType.Poison, 80, 90); // High poison resistance
            SetResistance(ResistanceType.Energy, 50, 60); // Good energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Boosted magic resist
            SetSkill(SkillName.Tactics, 120.0); // Advanced tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling skill
            SetSkill(SkillName.Magery, 100.0); // Elevated magery skill

            Fame = 30000; // Increased fame for a boss-tier creature
            Karma = -30000; // Boss-tier negative karma

            VirtualArmor = 100; // Enhanced virtual armor

            Tamable = false; // Boss creatures are untameable
            ControlSlots = 3;

            m_AbilitiesInitialized = false;

            // Attach the XmlRandomAbility to add special random abilities
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

            // Additional boss logic could be added here (like a special attack phase)
        }

        public GraniteColossusBoss(Serial serial) : base(serial)
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
