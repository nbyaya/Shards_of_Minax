using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HammerlordsCap : Cap
{
    [Constructable]
    public HammerlordsCap()
    {
        Name = "Hammerlord's Cap";
        Hue = Utility.Random(400, 1400);
        Attributes.BonusStr = 15;
        Attributes.CastSpeed = 1;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 25.0);
        Resistances.Energy = 10;
        Resistances.Physical = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HammerlordsCap(Serial serial) : base(serial)
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
