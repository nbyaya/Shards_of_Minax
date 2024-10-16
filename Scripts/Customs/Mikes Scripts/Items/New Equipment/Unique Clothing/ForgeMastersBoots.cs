using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ForgeMastersBoots : Boots
{
    [Constructable]
    public ForgeMastersBoots()
    {
        Name = "Forge Master's Boots";
        Hue = Utility.Random(600, 1650);
        ClothingAttributes.LowerStatReq = 3;
        Attributes.BonusDex = 10;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 20.0);
        Resistances.Physical = 20;
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ForgeMastersBoots(Serial serial) : base(serial)
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
