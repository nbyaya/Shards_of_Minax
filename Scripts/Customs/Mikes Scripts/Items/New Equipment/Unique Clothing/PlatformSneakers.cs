using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PlatformSneakers : Shoes
{
    [Constructable]
    public PlatformSneakers()
    {
        Name = "Platform Sneakers";
        Hue = Utility.Random(400, 2300);
        Attributes.BonusDex = 12;
        Attributes.WeaponSpeed = 10;
        Resistances.Energy = 10;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PlatformSneakers(Serial serial) : base(serial)
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
