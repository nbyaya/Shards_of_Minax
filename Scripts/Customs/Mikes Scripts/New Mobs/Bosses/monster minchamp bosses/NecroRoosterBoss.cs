using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a necro overlord rooster corpse")]
    public class NecroRoosterBoss : NecroRooster
    {
        private DateTime m_NextNecroticPeck;
        private DateTime m_NextDeathEgg;

        [Constructable]
        public NecroRoosterBoss() : base()
        {
            Name = "Necro Overlord Rooster";
            Title = "the Undying Peck";

            // Enhance stats to match or exceed Barracoon's values
            SetStr(1200); // Set the strength to 1200
            SetDex(255); // Max dexterity from the original
            SetInt(750); // Increased intelligence for higher spell power

            SetHits(12000); // High health similar to Barracoon
            SetDamage(35, 45); // Increased damage to reflect a boss fight

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // High fame value for a boss
            Karma = -30000; // Negative karma to indicate a villainous character

            VirtualArmor = 100;

            Tamable = false;
            ControlSlots = 0;
            MinTameSkill = 0;

            // Attach the random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_NextNecroticPeck = DateTime.UtcNow;
            m_NextDeathEgg = DateTime.UtcNow;
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Drop 5 MaxxiaScrolls in addition to regular loot
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

        public NecroRoosterBoss(Serial serial)
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
        }
    }
}
