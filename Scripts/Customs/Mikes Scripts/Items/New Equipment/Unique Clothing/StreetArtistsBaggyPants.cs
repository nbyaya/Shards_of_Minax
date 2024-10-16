using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StreetArtistsBaggyPants : LongPants
{
    [Constructable]
    public StreetArtistsBaggyPants()
    {
        Name = "Street Artist's Baggy Pants";
        Hue = Utility.Random(300, 2100);
        Attributes.Luck = 20;
        Attributes.BonusMana = 5;
        SkillBonuses.SetValues(0, SkillName.Begging, 15.0);
        SkillBonuses.SetValues(1, SkillName.Provocation, 10.0);
        Resistances.Physical = 10;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StreetArtistsBaggyPants(Serial serial) : base(serial)
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
