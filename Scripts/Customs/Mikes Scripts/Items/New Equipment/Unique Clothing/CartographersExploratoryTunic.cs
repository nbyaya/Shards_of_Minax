using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CartographersExploratoryTunic : Tunic
{
    [Constructable]
    public CartographersExploratoryTunic()
    {
        Name = "Cartographer's Exploratory Tunic";
        Hue = Utility.Random(300, 2300);
        ClothingAttributes.SelfRepair = 3;
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 5;
        SkillBonuses.SetValues(0, SkillName.Cartography, 25.0);
        SkillBonuses.SetValues(1, SkillName.Tracking, 15.0);
        Resistances.Energy = 10;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CartographersExploratoryTunic(Serial serial) : base(serial)
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
