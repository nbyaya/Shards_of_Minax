using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SilentNightCloak : Cloak
{
    [Constructable]
    public SilentNightCloak()
    {
        Name = "Silent Night Cloak";
        Hue = Utility.Random(600, 1600);
        ClothingAttributes.SelfRepair = 4;
        Attributes.BonusDex = 10;
        Attributes.LowerManaCost = 5;
        SkillBonuses.SetValues(0, SkillName.Hiding, 20.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 15.0);
        Resistances.Cold = 10;
        Resistances.Physical = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SilentNightCloak(Serial serial) : base(serial)
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
