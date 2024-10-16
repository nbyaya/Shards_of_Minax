using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SherlocksSleuthingCap : DeerMask
{
    [Constructable]
    public SherlocksSleuthingCap()
    {
        Name = "Sherlock's Sleuthing Cap";
        Hue = Utility.Random(550, 2600);
        Attributes.BonusInt = 15;
        SkillBonuses.SetValues(0, SkillName.DetectHidden, 25.0);
        SkillBonuses.SetValues(1, SkillName.ItemID, 15.0);
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SherlocksSleuthingCap(Serial serial) : base(serial)
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
