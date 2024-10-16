using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BeatniksBeret : Cap
{
    [Constructable]
    public BeatniksBeret()
    {
        Name = "Beatnik's Beret";
        Hue = Utility.Random(1, 2950);
        Attributes.BonusInt = 15;
        Attributes.LowerManaCost = 5;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 15.0);
        SkillBonuses.SetValues(1, SkillName.Focus, 20.0);
        Resistances.Fire = 10;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BeatniksBeret(Serial serial) : base(serial)
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
