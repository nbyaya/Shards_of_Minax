using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SkatersBaggyPants : LongPants
{
    [Constructable]
    public SkatersBaggyPants()
    {
        Name = "Skater's Baggy Pants";
        Hue = Utility.Random(400, 2400);
        Attributes.BonusDex = 15;
        ClothingAttributes.LowerStatReq = 3;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        Resistances.Physical = 10;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SkatersBaggyPants(Serial serial) : base(serial)
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
