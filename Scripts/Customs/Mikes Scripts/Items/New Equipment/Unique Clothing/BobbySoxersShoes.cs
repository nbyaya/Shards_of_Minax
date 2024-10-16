using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BobbySoxersShoes : Shoes
{
    [Constructable]
    public BobbySoxersShoes()
    {
        Name = "Bobby Soxer's Shoes";
        Hue = Utility.Random(1, 2500);
        Attributes.BonusDex = 10;
        Attributes.RegenStam = 3;
        Resistances.Energy = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BobbySoxersShoes(Serial serial) : base(serial)
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
