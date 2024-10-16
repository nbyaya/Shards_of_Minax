using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class UrbanitesSneakers : Shoes
{
    [Constructable]
    public UrbanitesSneakers()
    {
        Name = "Urbanite's Sneakers";
        Hue = Utility.Random(200, 2700);
        Attributes.BonusDex = 10;
        Attributes.BonusStam = 5;
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        Resistances.Physical = 10;
        Resistances.Energy = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public UrbanitesSneakers(Serial serial) : base(serial)
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
