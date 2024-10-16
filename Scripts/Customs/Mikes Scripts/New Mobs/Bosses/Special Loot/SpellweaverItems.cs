using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
    public class EnchantedStaff : GnarledStaff
    {
        [Constructable]
        public EnchantedStaff()
        {
            Name = "Enchanted Staff";
            Hue = 0x482;
            Attributes.SpellChanneling = 1;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.SpellDamage = 15;
        }

        public EnchantedStaff(Serial serial) : base(serial)
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

    public class DarkGrimoire : Item
    {
        [Constructable]
        public DarkGrimoire() : base(0x1C12)
        {
            Name = "Dark Grimoire";
            Hue = 0x497;
            Weight = 1.0;
        }

        public DarkGrimoire(Serial serial) : base(serial)
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

namespace Server.Mobiles
{
    public class Familiar : BaseCreature
    {
        [Constructable]
        public Familiar() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Familiar";
            Body = 0x4;
            Hue = 0x497;

            SetStr(150, 200);
            SetDex(150, 200);
            SetInt(200, 250);

            SetHits(500);
            SetMana(1000);

            SetDamage(15, 20);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 80.0);
            SetSkill(SkillName.Magery, 80.0);
            SetSkill(SkillName.Meditation, 80.0);
            SetSkill(SkillName.MagicResist, 80.0);
            SetSkill(SkillName.Tactics, 70.0);
            SetSkill(SkillName.Wrestling, 70.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;
        }

        public Familiar(Serial serial) : base(serial)
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
