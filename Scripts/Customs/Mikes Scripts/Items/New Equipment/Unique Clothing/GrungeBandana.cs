using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GrungeBandana : Bandana
{
    [Constructable]
    public GrungeBandana()
    {
        Name = "Grunge Bandana";
        Hue = Utility.Random(250, 2750);
        Attributes.BonusDex = 10;
        Attributes.AttackChance = 5;
        SkillBonuses.SetValues(0, SkillName.Focus, 20.0);
        SkillBonuses.SetValues(1, SkillName.Provocation, 15.0);
        Resistances.Physical = 5;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GrungeBandana(Serial serial) : base(serial)
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
