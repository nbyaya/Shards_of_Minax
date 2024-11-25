using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a stonebreath corpse")]
    public class MordrakeBoss : Mordrake
    {
        [Constructable]
        public MordrakeBoss() : base()
        {
            Name = "Mordrake the Stonebreath";
            Title = "the Supreme Stonebreath";

            // Enhance stats to match or exceed Barracoon as a baseline
            SetStr(1200); // Increased strength
            SetDex(255); // Increased dexterity
            SetInt(250); // Increased intelligence

            SetHits(12000); // Increased health

            SetDamage(35, 45); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Maximum magic resist
            SetSkill(SkillName.Tactics, 120.0); // Increased tactics
            SetSkill(SkillName.Wrestling, 120.0); // Increased wrestling
            SetSkill(SkillName.Magery, 120.0); // Increased magery
            SetSkill(SkillName.EvalInt, 120.0); // Increased EvalInt

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma (still evil)

            VirtualArmor = 100; // Increased virtual armor

            Tamable = false; // Bosses are not tamable
            ControlSlots = 0; // No control slots for taming

            // Attach the XmlRandomAbility
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
            // Additional boss logic could be added here if needed
        }

        public MordrakeBoss(Serial serial) : base(serial)
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
