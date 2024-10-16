using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BakersSoftShoes : Shoes
{
    [Constructable]
    public BakersSoftShoes()
    {
        Name = "Baker's Soft Shoes";
        Hue = Utility.Random(200, 2700);
        Attributes.BonusDex = 8;
        Attributes.LowerManaCost = 5;
        SkillBonuses.SetValues(0, SkillName.Cooking, 20.0);
        Resistances.Cold = 10;
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BakersSoftShoes(Serial serial) : base(serial)
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
