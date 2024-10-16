using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MapmakersInsightfulMuffler : Cap
{
    [Constructable]
    public MapmakersInsightfulMuffler()
    {
        Name = "Mapmaker's Insightful Muffler";
        Hue = Utility.Random(320, 2320);
        Attributes.BonusInt = 10;
        Attributes.Luck = 10;
        SkillBonuses.SetValues(0, SkillName.Cartography, 20.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);
        Resistances.Cold = 15;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MapmakersInsightfulMuffler(Serial serial) : base(serial)
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
