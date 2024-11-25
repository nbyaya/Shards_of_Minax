using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Magnetic Crab corpse")]
    public class MagneticCrabBoss : MagneticCrab
    {
        [Constructable]
        public MagneticCrabBoss()
            : base()
        {
            Name = "Magnetic Overlord Crab";
            Title = "the Supreme Magnet";

            // Enhanced stats to match or exceed Barracoon
            SetStr(1200); // Higher strength
            SetDex(255); // Higher dexterity
            SetInt(250); // Higher intelligence

            SetHits(12000); // Much higher health for a boss
            SetDamage(35, 50); // Increased damage for a tougher boss

            SetResistance(ResistanceType.Physical, 80, 90); // Increased resistance
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Increased magic resist
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90; // Keeping it as is for boss-tier durability

            Tamable = false; // Boss is not tamable
            ControlSlots = 0; // Cannot be controlled

            Hue = 1455; // Unique blue-ish hue

            // Attach the XmlRandomAbility to add a random ability
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
            // Additional boss logic could be added here, e.g., more frequent use of abilities or special attacks
        }

        public MagneticCrabBoss(Serial serial)
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
