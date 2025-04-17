using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a gorgon viper boss corpse")]
    public class GorgonViperBoss : GorgonViper
    {
        private DateTime m_NextToxicCloud;
        private DateTime m_NextFatalStrike;
        private DateTime m_NextVipersVigil;
        private DateTime m_NextSummonAllies;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public GorgonViperBoss() : base()
        {
            Name = "Gorgon Viper, the Overlord";
            Title = "the Supreme Serpent";
            Hue = 1777; // Unique hue for the boss version

            // Update stats to match or exceed the original Gorgon Viper
            SetStr(1200, 1600); // Enhanced strength
            SetDex(255, 300);   // Enhanced dexterity
            SetInt(250, 350);   // Enhanced intelligence

            SetHits(14000, 16000); // Boss-tier health
            SetDamage(40, 55); // Increased damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);  // Increased fire damage
            SetDamageType(ResistanceType.Energy, 30); // Increased energy damage

            SetResistance(ResistanceType.Physical, 80, 95); // Enhanced physical resistance
            SetResistance(ResistanceType.Fire, 75, 90);     // Enhanced fire resistance
            SetResistance(ResistanceType.Cold, 60, 75);     // Enhanced cold resistance
            SetResistance(ResistanceType.Poison, 80, 95);   // Enhanced poison resistance
            SetResistance(ResistanceType.Energy, 50, 70);   // Enhanced energy resistance

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 120;

            Tamable = false;

            // Attach a random ability for additional boss flair
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // Serialization constructor - this is what was missing
        public GorgonViperBoss(Serial serial) : base(serial)
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
            // Additional logic for the boss could be added here
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
