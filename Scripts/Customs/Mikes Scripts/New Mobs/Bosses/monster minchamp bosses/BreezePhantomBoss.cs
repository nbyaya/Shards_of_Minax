using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a breeze phantom overlord's corpse")]
    public class BreezePhantomBoss : BreezePhantom
    {
        private bool m_AbilitiesActivated; // Ensure it activates abilities after initialization

        [Constructable]
        public BreezePhantomBoss() : base()
        {
            Name = "Breeze Phantom Overlord";
            Title = "the Supreme Phantom";

            // Update stats to match or exceed the boss tier
            SetStr(1200, 1500); // Higher Strength
            SetDex(255, 300); // Higher Dexterity
            SetInt(250, 350); // Higher Intelligence

            SetHits(15000); // Higher Health

            SetDamage(35, 45); // Higher Damage Range

            // Enhanced Damage Types (similar to the original)
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85); // Higher Physical Resistance
            SetResistance(ResistanceType.Fire, 75, 85); // Higher Fire Resistance
            SetResistance(ResistanceType.Cold, 60, 70); // Higher Cold Resistance
            SetResistance(ResistanceType.Poison, 100); // Max Poison Resistance
            SetResistance(ResistanceType.Energy, 50, 60); // Higher Energy Resistance

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0); // Enhanced Magic Resist
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 100; // Increased Virtual Armor

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

        public BreezePhantomBoss(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesActivated = false;
        }
    }
}
