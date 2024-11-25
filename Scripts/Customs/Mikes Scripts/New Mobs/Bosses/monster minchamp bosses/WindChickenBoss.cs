using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a wind chicken boss corpse")]
    public class WindChickenBoss : WindChicken
    {
        private DateTime m_NextStormEgg;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public WindChickenBoss()
            : base()
        {
            Name = "Wind Chicken Boss";
            Title = "the Stormbringer";

            // Enhance stats to match or exceed Barracoon
            SetStr(1200, 1500); // Increase strength
            SetDex(250, 300);   // Increase dexterity
            SetInt(300, 500);   // Increase intelligence

            SetHits(15000);     // Increase health
            SetDamage(40, 50);  // Increase damage range

            // Increase resistances
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Increase skills to match a boss-tier NPC
            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 130.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;

            Tamable = false;  // The boss version is untamable
            ControlSlots = 0;

            m_AbilitiesInitialized = false;

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Add 5 MaxxiaScroll items to the loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Add additional boss behavior here if necessary
        }

        public WindChickenBoss(Serial serial) : base(serial)
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
