using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a vile blossom corpse")]
    public class VileBlossomBoss : VileBlossom
    {
        private DateTime m_NextBlossomBurst;
        private DateTime m_NextPollinateFear;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public VileBlossomBoss() : base("Vile Blossom")
        {
            Name = "Vile Blossom Overlord";
            Title = "the Blooming Terror";

            // Enhanced stats to make it a boss
            SetStr(1200, 1500); // Increased strength
            SetDex(255, 300); // Increased dexterity
            SetInt(250, 350); // Increased intelligence

            SetHits(12000, 15000); // Increased health

            SetDamage(40, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Increased physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 60, 70); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 100); // Maximum poison resistance
            SetResistance(ResistanceType.Energy, 60, 70); // Increased energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Increased magic resist
            SetSkill(SkillName.Tactics, 120.0); // Increased tactics
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling skill

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma

            VirtualArmor = 100; // Increased virtual armor

            // Attach a random ability
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
        }

        public override void OnThink()
        {
            base.OnThink();

            // Optional: Additional boss-specific AI or abilities could go here
        }

        public VileBlossomBoss(Serial serial) : base(serial)
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }
}
