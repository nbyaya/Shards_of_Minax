using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ExplorersBoots : Boots
{
    [Constructable]
    public ExplorersBoots()
    {
        Name = "Explorer's Boots";
        Hue = Utility.Random(750, 2550);
        ClothingAttributes.LowerStatReq = 4;
        Attributes.BonusInt = 5;
        SkillBonuses.SetValues(0, SkillName.Cartography, 25.0);
        Resistances.Cold = 10;
        Resistances.Physical = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ExplorersBoots(Serial serial) : base(serial)
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
