using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BeggarsLuckyBandana : Bandana
{
    [Constructable]
    public BeggarsLuckyBandana()
    {
        Name = "Beggar's Lucky Bandana";
        Hue = Utility.Random(700, 2700);
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.Begging, 25.0);
        SkillBonuses.SetValues(1, SkillName.DetectHidden, 10.0);
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BeggarsLuckyBandana(Serial serial) : base(serial)
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
