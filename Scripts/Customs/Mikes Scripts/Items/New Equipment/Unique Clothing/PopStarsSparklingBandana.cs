using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PopStarsSparklingBandana : Bandana
{
    [Constructable]
    public PopStarsSparklingBandana()
    {
        Name = "Pop Star's Sparkling Bandana";
        Hue = Utility.Random(250, 2250);
        Attributes.NightSight = 1;
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        Resistances.Energy = 10;
        Resistances.Physical = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PopStarsSparklingBandana(Serial serial) : base(serial)
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
