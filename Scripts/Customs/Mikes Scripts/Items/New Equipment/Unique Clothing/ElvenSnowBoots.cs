using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ElvenSnowBoots : Boots
{
    [Constructable]
    public ElvenSnowBoots()
    {
        Name = "Elven Snow Boots";
        Hue = Utility.Random(1150, 2200);
        ClothingAttributes.LowerStatReq = 4;
        Attributes.BonusDex = 12;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        Resistances.Cold = 25;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ElvenSnowBoots(Serial serial) : base(serial)
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
