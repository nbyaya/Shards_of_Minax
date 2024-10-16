using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BanditsHiddenCloak : Cloak
{
    [Constructable]
    public BanditsHiddenCloak()
    {
        Name = "Bandit's Hidden Cloak";
        Hue = Utility.Random(1200, 2900);
        Attributes.LowerRegCost = 10;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Hiding, 25.0);
        SkillBonuses.SetValues(1, SkillName.Stealing, 20.0);
        Resistances.Cold = 15;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BanditsHiddenCloak(Serial serial) : base(serial)
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
