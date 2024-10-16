using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GroovyBellBottomPants : LongPants
{
    [Constructable]
    public GroovyBellBottomPants()
    {
        Name = "Groovy Bell-Bottom Pants";
        Hue = Utility.Random(250, 2100);
        Attributes.BonusStam = 10;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        Resistances.Physical = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GroovyBellBottomPants(Serial serial) : base(serial)
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
