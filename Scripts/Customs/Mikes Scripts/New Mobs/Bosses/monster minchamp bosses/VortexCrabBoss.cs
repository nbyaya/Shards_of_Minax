using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a vortex crab boss corpse")]
    public class VortexCrabBoss : VortexCrab
    {
        private bool m_AbilitiesInitialized;

        [Constructable]
        public VortexCrabBoss() : base("Vortex Crab Boss")
        {
            // Boss-tier enhancements
            SetStr(1200); // Strength set higher
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Increased health to boss levels

            SetDamage(45, 60); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Higher resistance to physical
            SetResistance(ResistanceType.Fire, 75, 85); // Higher fire resistance
            SetResistance(ResistanceType.Cold, 65, 75); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 100); // Poison resistance stays high
            SetResistance(ResistanceType.Energy, 60, 70); // Increased energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // High magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics skill
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling skill
            SetSkill(SkillName.Magery, 100.0); // Enhanced magery skill

            Fame = 35000; // High fame
            Karma = -35000; // High negative karma (boss-tier)

            VirtualArmor = 120; // Enhanced armor

            Tamable = false; // Boss is not tamable
            ControlSlots = 0; // Not controllable by players
            MinTameSkill = 0;

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
            // Additional boss logic can be added here if needed
        }

        public VortexCrabBoss(Serial serial) : base(serial)
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
        }
    }
}
