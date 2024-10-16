using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FletchersPrecisionGloves : TribalMask
{
    [Constructable]
    public FletchersPrecisionGloves()
    {
        Name = "Fletcher's Precision Mask";
        Hue = Utility.Random(300, 2300);
        ClothingAttributes.SelfRepair = 4;
        Attributes.BonusDex = 20;
        Attributes.Luck = 10;
        SkillBonuses.SetValues(0, SkillName.Fletching, 25.0);
        Resistances.Physical = 10;
        Resistances.Cold = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FletchersPrecisionGloves(Serial serial) : base(serial)
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
