using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TruckersIconicCap : Cap
{
    [Constructable]
    public TruckersIconicCap()
    {
        Name = "Trucker's Iconic Cap";
        Hue = Utility.Random(300, 2300);
        Attributes.BonusInt = 10;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 15.0);
		SkillBonuses.SetValues(1, SkillName.Focus, 46.0);
        Resistances.Energy = 5;
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TruckersIconicCap(Serial serial) : base(serial)
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
