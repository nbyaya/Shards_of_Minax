using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FishmongersKilt : Kilt
{
    [Constructable]
    public FishmongersKilt()
    {
        Name = "Fishmonger's Kilt";
        Hue = Utility.Random(500, 2500);
        Attributes.BonusDex = 5;
        Attributes.RegenHits = 2;
        SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);
        Resistances.Cold = 20;
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FishmongersKilt(Serial serial) : base(serial)
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
