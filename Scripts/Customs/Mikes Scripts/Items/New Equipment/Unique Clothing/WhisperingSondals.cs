using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WhisperingSondals : Sandals
{
    [Constructable]
    public WhisperingSondals()
    {
        Name = "Whispering Sandals";
        Hue = Utility.Random(300, 2400);
        Attributes.BonusDex = 15;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        Resistances.Cold = 5;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WhisperingSondals(Serial serial) : base(serial)
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
