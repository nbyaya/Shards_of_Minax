using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the trap overlord")]
    public class TrapMakerBoss : TrapMaker
    {
        [Constructable]
        public TrapMakerBoss() : base()
        {
            Name = "Trap Overlord";
            Title = "the Supreme Setter";

            // Enhance stats to match or exceed a boss's power
            SetStr(900); // Upper end of the original strength
            SetDex(250); // Upper end of the original dexterity
            SetInt(200); // Upper end of the original intelligence

            SetHits(12000); // Matching Barracoon's health for a boss fight
            SetDamage(25, 45); // Increased damage for a more challenging fight

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Stealth, 100.0);
            SetSkill(SkillName.Poisoning, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70; // Increased armor for better defense

            // Attach random ability for enhanced gameplay experience
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

        public TrapMakerBoss(Serial serial) : base(serial)
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
