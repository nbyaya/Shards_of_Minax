using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a lava overlord corpse")]
    public class LavaFiendBoss : LavaFiend
    {
        private DateTime m_NextLavaFlow;
        private DateTime m_NextVolcanicEruption;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public LavaFiendBoss()
            : base()
        {
            Name = "Lava Overlord";
            Title = "the Supreme Fiend";
            Body = 14; // Earth Elemental body
            BaseSoundID = 268;
            Hue = 1500; // Fiery red-orange hue

            // Update stats to match or exceed Barracoon
            SetStr(1200);
            SetDex(255);
            SetInt(250);

            SetHits(12000);
            SetDamage(45, 55);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 80.0);
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Magery, 115.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 80.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 100;

            Tamable = false;
            ControlSlots = 0;

            m_AbilitiesInitialized = false;

            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // 🔧 Serialization constructor added here
        public LavaFiendBoss(Serial serial) : base(serial)
        {
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

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_AbilitiesInitialized = false;
        }
    }
}
