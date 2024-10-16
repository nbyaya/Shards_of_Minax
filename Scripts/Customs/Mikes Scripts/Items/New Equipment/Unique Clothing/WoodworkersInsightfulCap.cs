using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WoodworkersInsightfulCap : Cap
{
    [Constructable]
    public WoodworkersInsightfulCap()
    {
        Name = "Woodworker's Insightful Cap";
        Hue = Utility.Random(500, 1400);
        Attributes.BonusInt = 10;
        Attributes.CastSpeed = 1;
        SkillBonuses.SetValues(0, SkillName.Carpentry, 20.0);
        Resistances.Cold = 10;
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WoodworkersInsightfulCap(Serial serial) : base(serial)
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
