using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WarHeronsCap : Cap
{
    [Constructable]
    public WarHeronsCap()
    {
        Name = "War Hero's Cap";
        Hue = Utility.Random(600, 2600);
        Attributes.BonusStr = 10;
        Attributes.DefendChance = 15;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        Resistances.Physical = 20;
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WarHeronsCap(Serial serial) : base(serial)
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
