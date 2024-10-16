using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AerobicsInstructorsLegwarmers : ThighBoots
{
    [Constructable]
    public AerobicsInstructorsLegwarmers()
    {
        Name = "Aerobics Instructor's Legwarmers";
        Hue = Utility.Random(300, 2300);
        Attributes.BonusStam = 15;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Parry, 15.0);
        Resistances.Physical = 20;
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AerobicsInstructorsLegwarmers(Serial serial) : base(serial)
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
