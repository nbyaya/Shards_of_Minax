using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StreetPerformersCap : Cap
{
    [Constructable]
    public StreetPerformersCap()
    {
        Name = "Street Performer's Cap";
        Hue = Utility.Random(100, 2500);
        Attributes.AttackChance = 5;
        Attributes.Luck = 15;
        SkillBonuses.SetValues(0, SkillName.Begging, 20.0);
        SkillBonuses.SetValues(1, SkillName.Musicianship, 10.0);
        Resistances.Physical = 10;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StreetPerformersCap(Serial serial) : base(serial)
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
