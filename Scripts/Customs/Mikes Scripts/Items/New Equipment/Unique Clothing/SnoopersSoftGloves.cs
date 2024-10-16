using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SnoopersSoftGloves : LeatherGloves
{
    [Constructable]
    public SnoopersSoftGloves()
    {
        Name = "Snooper's Soft Gloves";
        Hue = Utility.Random(600, 2600);
        Attributes.BonusDex = 10;
        Attributes.LowerManaCost = 5;
        SkillBonuses.SetValues(0, SkillName.Snooping, 25.0);
        SkillBonuses.SetValues(1, SkillName.Stealing, 20.0);
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SnoopersSoftGloves(Serial serial) : base(serial)
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
