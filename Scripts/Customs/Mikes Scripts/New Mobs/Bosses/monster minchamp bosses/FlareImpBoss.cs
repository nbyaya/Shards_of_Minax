using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a flare imp overlord corpse")]
    public class FlareImpBoss : FlareImp
    {
        private DateTime m_NextFireball;
        private DateTime m_NextBlazingSpeed;
        private DateTime m_NextFieryTrickster;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FlareImpBoss() : base()
        {
            Name = "Flare Imp Overlord";
            Title = "the Infernal Ruler";
            Body = 15; // Fire elemental body
            Hue = 1659; // Unique hue
            BaseSoundID = 838;

            // Enhanced stats
            SetStr(1200, 1500);
            SetDex(255, 350);
            SetInt(250, 350);

            SetHits(15000, 20000);

            SetDamage(40, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 40);
            SetDamageType(ResistanceType.Energy, 10);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 120.0, 130.0);
            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.Meditation, 50.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 120;

            // Attach the XmlRandomAbility to the boss
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_AbilitiesInitialized = false;
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
            // Additional boss-specific behavior can be added here if necessary
        }

        public FlareImpBoss(Serial serial) : base(serial)
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
