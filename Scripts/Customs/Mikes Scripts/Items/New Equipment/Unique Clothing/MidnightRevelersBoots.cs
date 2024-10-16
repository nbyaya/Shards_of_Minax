using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MidnightRevelersBoots : Boots
{
    [Constructable]
    public MidnightRevelersBoots()
    {
        Name = "Midnight Reveler's Boots";
        Hue = Utility.Random(100, 900);
        Attributes.BonusDex = 15;
        Attributes.NightSight = 1;
        Resistances.Physical = 10;
        Resistances.Energy = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MidnightRevelersBoots(Serial serial) : base(serial)
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
