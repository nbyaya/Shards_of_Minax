using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CourtisansRefinedGown : FancyDress
{
    [Constructable]
    public CourtisansRefinedGown()
    {
        Name = "Courtisan's Refined Gown";
        Hue = Utility.Random(200, 2700);
        Attributes.BonusInt = 10;
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.Peacemaking, 20.0);
        Resistances.Cold = 10;
        Resistances.Energy = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CourtisansRefinedGown(Serial serial) : base(serial)
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
