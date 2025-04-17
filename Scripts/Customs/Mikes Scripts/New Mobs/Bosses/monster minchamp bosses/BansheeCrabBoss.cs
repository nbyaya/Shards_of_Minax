using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a banshee crab overlord corpse")]
    public class BansheeCrabBoss : BansheeCrab
    {
        private DateTime m_NextWailingPull;
        private DateTime m_NextScreechAttack;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BansheeCrabBoss() : base()
        {
            // Update stats to match or exceed Barracoon
            Name = "Banshee Crab Overlord";
            Hue = 1461; // Ghostly hue, same as the original
            BaseSoundID = 0x4F2;

            // Enhanced stats
            SetStr(1200, 1500); // Enhanced strength
            SetDex(255, 300); // Enhanced dexterity
            SetInt(250, 350); // Enhanced intelligence

            SetHits(12000); // Boss health
            SetDamage(40, 50); // Increased damage

            // Enhanced resistances and damage types
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 120); // Maxed out poison resistance
            SetResistance(ResistanceType.Energy, 60, 70);

            // Enhanced skills
            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 100.0, 150.0);
            SetSkill(SkillName.Magery, 100.0, 150.0);
            SetSkill(SkillName.Meditation, 50.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 100.0, 150.0);
            SetSkill(SkillName.Wrestling, 100.0, 150.0);

            Fame = 30000; // Increased fame for a boss
            Karma = -30000; // Increased karma for a boss

            VirtualArmor = 100; // Higher virtual armor

            Tamable = false; // This is now a boss, not tamable
            ControlSlots = 0; // Boss cannot be controlled by a player
            MinTameSkill = 0.0; // Not tamable anymore

            m_AbilitiesInitialized = false; // Initialize abilities flag
        }

        // ✅ Required deserialization constructor
        public BansheeCrabBoss(Serial serial) : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here, e.g., more frequent use of abilities or special attacks
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
