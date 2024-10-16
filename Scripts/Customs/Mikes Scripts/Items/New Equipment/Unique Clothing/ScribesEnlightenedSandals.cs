using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ScribesEnlightenedSandals : Sandals
{
    [Constructable]
    public ScribesEnlightenedSandals()
    {
        Name = "Scribe's Enlightened Sandals";
        Hue = Utility.Random(250, 2150);
        Attributes.BonusInt = 10;
        Attributes.RegenMana = 3;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 25.0);
        Resistances.Fire = 10;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ScribesEnlightenedSandals(Serial serial) : base(serial)
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
