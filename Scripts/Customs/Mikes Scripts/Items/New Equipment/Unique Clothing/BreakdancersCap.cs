using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BreakdancersCap : Cap
{
    [Constructable]
    public BreakdancersCap()
    {
        Name = "Breakdancer's Cap";
        Hue = Utility.Random(50, 2000);
        Attributes.BonusDex = 15;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        Resistances.Physical = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BreakdancersCap(Serial serial) : base(serial)
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
