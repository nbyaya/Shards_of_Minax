using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SmithsProtectiveTunic : Tunic
{
    [Constructable]
    public SmithsProtectiveTunic()
    {
        Name = "Smith's Protective Tunic";
        Hue = Utility.Random(600, 1600);
        ClothingAttributes.DurabilityBonus = 4;
        Attributes.BonusStr = 20;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 25.0);
        Resistances.Fire = 20;
        Resistances.Physical = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SmithsProtectiveTunic(Serial serial) : base(serial)
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
