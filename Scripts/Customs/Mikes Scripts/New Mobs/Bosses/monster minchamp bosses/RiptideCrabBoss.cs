using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Riptide Crab corpse")]
    public class RiptideCrabBoss : RiptideCrab
    {
        private DateTime m_NextTidalPull;
        private DateTime m_NextAbyssalSlam;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public RiptideCrabBoss()
            : base()
        {
            Name = "Riptide Crab Overlord";
            Title = "the Abyssal Terror";

            SetStr(1200, 1500);
            SetDex(255, 300);
            SetInt(250, 350);

            SetHits(15000);
            SetDamage(40, 50);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 120);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 80.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 80.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 100;

            Tamable = false;
            ControlSlots = 0;

            m_AbilitiesInitialized = false;

            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // Required for deserialization
        public RiptideCrabBoss(Serial serial) : base(serial)
        {
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional AI or boss mechanics go here
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
