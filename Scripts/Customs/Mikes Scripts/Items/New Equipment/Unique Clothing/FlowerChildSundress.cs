using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FlowerChildSundress : PlainDress
{
    [Constructable]
    public FlowerChildSundress()
    {
        Name = "Flower Child Sundress";
        Hue = Utility.Random(350, 2400);
        ClothingAttributes.MageArmor = 1;
        Attributes.RegenHits = 2;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 20.0);
        Resistances.Poison = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FlowerChildSundress(Serial serial) : base(serial)
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
