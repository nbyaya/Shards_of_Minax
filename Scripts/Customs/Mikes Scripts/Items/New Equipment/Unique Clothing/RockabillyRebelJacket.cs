using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RockabillyRebelJacket : FancyShirt
{
    [Constructable]
    public RockabillyRebelJacket()
    {
        Name = "Rockabilly Rebel Jacket";
        Hue = Utility.Random(1, 2000);
        ClothingAttributes.LowerStatReq = 4;
        Attributes.BonusStr = 15;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
        Resistances.Physical = 25;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RockabillyRebelJacket(Serial serial) : base(serial)
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
