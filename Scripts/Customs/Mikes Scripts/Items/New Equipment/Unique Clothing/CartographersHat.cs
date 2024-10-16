using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CartographersHat : WideBrimHat
{
    [Constructable]
    public CartographersHat()
    {
        Name = "Cartographer's Hat";
        Hue = Utility.Random(600, 2400);
        Attributes.CastRecovery = 2;
        SkillBonuses.SetValues(0, SkillName.Cartography, 25.0);
        Resistances.Fire = 5;
        Resistances.Physical = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CartographersHat(Serial serial) : base(serial)
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
