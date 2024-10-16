using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BowyersInsightfulBandana : Bandana
{
    [Constructable]
    public BowyersInsightfulBandana()
    {
        Name = "Bowyer's Insightful Bandana";
        Hue = Utility.Random(400, 2400);
        Attributes.BonusInt = 15;
        Attributes.CastSpeed = 1;
        SkillBonuses.SetValues(0, SkillName.Fletching, 20.0);
        SkillBonuses.SetValues(1, SkillName.DetectHidden, 15.0);
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BowyersInsightfulBandana(Serial serial) : base(serial)
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
