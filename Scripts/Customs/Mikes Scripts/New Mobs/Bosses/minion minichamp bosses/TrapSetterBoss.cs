using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the trap overlord")]
    public class TrapSetterBoss : TrapSetter
    {
        [Constructable]
        public TrapSetterBoss() : base()
        {
            Name = "Trap Overlord";
            Title = "the Supreme Trap Setter";

            // Update stats to match or exceed Barracoon (as example, based on other boss stats)
            SetStr(900, 1200); // Enhanced strength
            SetDex(250, 300); // Enhanced dexterity
            SetInt(200, 250); // Enhanced intelligence

            SetHits(12000); // High health similar to other bosses

            SetDamage(15, 25); // Slightly higher damage for a stronger boss

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 70.0, 90.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);
            SetSkill(SkillName.Stealth, 100.0, 120.0);
            SetSkill(SkillName.Poisoning, 100.0, 120.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 60;

            // Attach a random ability
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

        public TrapSetterBoss(Serial serial) : base(serial)
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
