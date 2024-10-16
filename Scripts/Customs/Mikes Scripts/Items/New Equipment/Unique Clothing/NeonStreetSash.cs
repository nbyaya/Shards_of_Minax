using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NeonStreetSash : BodySash
{
    [Constructable]
    public NeonStreetSash()
    {
        Name = "Neon Street Sash";
        Hue = Utility.Random(850, 2600);
        Attributes.BonusStam = 10;
        Attributes.RegenStam = 3;
        SkillBonuses.SetValues(0, SkillName.Parry, 20.0);
        Resistances.Energy = 20;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NeonStreetSash(Serial serial) : base(serial)
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
