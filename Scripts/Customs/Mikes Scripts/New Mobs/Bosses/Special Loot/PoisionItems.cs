using System;
using Server;
using Server.Items;

namespace Server.Items
{
    // PoisonedDagger
    public class PoisonedDagger : Dagger
    {
        [Constructable]
        public PoisonedDagger()
        {
            Name = "Poisoned Dagger";
            Hue = 0x48E;
            Attributes.WeaponSpeed = 20;
            Attributes.WeaponDamage = 35;
            SkillBonuses.SetValues(0, SkillName.Poisoning, 10.0);
        }

        public PoisonedDagger(Serial serial) : base(serial)
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

    // VialOfToxins
    public class VialOfToxins : Item
    {
        [Constructable]
        public VialOfToxins()
        {
            Name = "Vial of Toxins";
            ItemID = 0xF0B;
            Hue = 0x46;
        }

        public VialOfToxins(Serial serial) : base(serial)
        {
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            base.AddNameProperty(list);
            list.Add("Boosts poison damage");
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

    // LethalPoisonPotion
    public class LethalPoisonPotion : Item
    {
        [Constructable]
        public LethalPoisonPotion() : base(0xF0A)
        {
            Name = "Lethal Poison Potion";
            Hue = 0x46;
        }

        public LethalPoisonPotion(Serial serial) : base(serial)
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

    // PoisonousPlant
    public class PoisonousPlant : Item
    {
        [Constructable]
        public PoisonousPlant() : base(0x18E7)
        {
            Name = "Poisonous Plant";
            Hue = 0x48E;
            Movable = true;
        }

        public PoisonousPlant(Serial serial) : base(serial)
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

    // ToxicCauldron
    public class ToxicCauldron : Item
    {
        [Constructable]
        public ToxicCauldron() : base(0x9ED)
        {
            Name = "Toxic Cauldron";
            Hue = 0x48E;
            Movable = true;
        }

        public ToxicCauldron(Serial serial) : base(serial)
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
