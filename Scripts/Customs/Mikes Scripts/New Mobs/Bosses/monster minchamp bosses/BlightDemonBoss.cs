using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a blight demon corpse")]
    public class BlightDemonBoss : BlightDemon
    {
        private bool m_AbilitiesInitialized;

        [Constructable]
        public BlightDemonBoss() : base()
        {
            Name = "Blight Demon Overlord";
            Title = "the Supreme Blight";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Matching the upper strength of Barracoon
            SetDex(255); // Matching the upper dexterity
            SetInt(250); // Matching the upper intelligence

            SetHits(12000); // Matching Barracoon's health

            SetDamage(35, 50); // Increased damage range to make it more threatening

            SetResistance(ResistanceType.Physical, 80); // Increased resistances
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 70);

            SetSkill(SkillName.Anatomy, 50.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 110.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 100;

            Tamable = false;
            ControlSlots = 5; // Boss-tier creature requires more control slots
            MinTameSkill = 0.0;

            m_AbilitiesInitialized = false;

            // Attach the random ability
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

        public BlightDemonBoss(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
