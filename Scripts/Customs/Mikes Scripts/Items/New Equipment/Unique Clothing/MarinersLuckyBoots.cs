using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MarinersLuckyBoots : Boots
{
    [Constructable]
    public MarinersLuckyBoots()
    {
        Name = "Mariner's Lucky Boots";
        Hue = Utility.Random(600, 2600);
        Attributes.Luck = 20;
        Attributes.BonusStam = 5;
        SkillBonuses.SetValues(0, SkillName.Fishing, 15.0);
        Resistances.Physical = 10;
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MarinersLuckyBoots(Serial serial) : base(serial)
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
