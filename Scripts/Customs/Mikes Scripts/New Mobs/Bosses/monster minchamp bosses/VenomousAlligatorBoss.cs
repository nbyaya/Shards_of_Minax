using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a venomous alligator corpse")]
    public class VenomousAlligatorBoss : VenomousAlligator
    {
        private DateTime m_NextVenomousBite;
        private DateTime m_NextPoisonCloud;
        private bool m_AbilitiesInitialized; // New flag to track initial setup

        [Constructable]
        public VenomousAlligatorBoss() : base()
        {
            Name = "Venomous Alligator King";
            Title = "the Toxic Tyrant";
            Hue = 1160; // Unique hue for the boss alligator

            // Update stats to match or exceed Barracoon's
            SetStr(1200); // Higher strength
            SetDex(255);  // Maxed dexterity for better agility
            SetInt(750);  // Higher intelligence for magic-related abilities

            SetHits(12000); // Boss-tier health
            SetDamage(35, 45); // Higher damage range

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100); // Max poison resistance
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Boosted magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Boosted tactics
            SetSkill(SkillName.Wrestling, 120.0); // Boosted wrestling skill

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 100; // Higher virtual armor

            Tamable = false; // Boss is untamable
            ControlSlots = 3;

            m_AbilitiesInitialized = false;

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

        public VenomousAlligatorBoss(Serial serial) : base(serial)
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

            m_AbilitiesInitialized = false; // Reset flag
        }
    }
}
