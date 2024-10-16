using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GlamRockersJacket : FancyShirt
{
    [Constructable]
    public GlamRockersJacket()
    {
        Name = "Glam Rocker's Jacket";
        Hue = Utility.Random(150, 2150);
        ClothingAttributes.DurabilityBonus = 4;
        Attributes.BonusDex = 10;
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        Resistances.Energy = 15;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GlamRockersJacket(Serial serial) : base(serial)
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
