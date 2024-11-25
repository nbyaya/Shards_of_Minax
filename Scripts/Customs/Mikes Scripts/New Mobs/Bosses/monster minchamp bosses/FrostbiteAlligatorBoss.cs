using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a frostbite alligator boss corpse")]
    public class FrostbiteAlligatorBoss : FrostbiteAlligator
    {
        private bool m_AbilitiesInitialized;

        [Constructable]
        public FrostbiteAlligatorBoss()
            : base()
        {
            Name = "Frostbite Alligator";
            Title = "the Eternal Warden";

            // Enhance stats to match or exceed Barracoon's level of power
            SetStr(1200); // Increased strength
            SetDex(255); // Max dexterity
            SetInt(250); // Increased intelligence
            
            SetHits(12000); // Increased health
            SetDamage(35, 45); // Increased damage

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 80, 95);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 150.0);
            SetSkill(SkillName.Wrestling, 120.0, 150.0);

            Fame = 30000; // Increased fame for a boss
            Karma = -30000; // Increased karma for a boss

            VirtualArmor = 100; // Enhanced armor

            // Mark as a boss and add the random ability
            Tamable = false;
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_AbilitiesInitialized = false; // Initialize abilities flag
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

        public FrostbiteAlligatorBoss(Serial serial)
            : base(serial)
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

            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }
}
