using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class IntriguersFeatheredHat : FeatheredHat
{
    [Constructable]
    public IntriguersFeatheredHat()
    {
        Name = "Intriguer's Feathered Hat";
        Hue = Utility.Random(300, 2300);
        Attributes.BonusDex = 15;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.DetectHidden, 20.0);
        Resistances.Physical = 5;
        Resistances.Poison = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public IntriguersFeatheredHat(Serial serial) : base(serial)
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
