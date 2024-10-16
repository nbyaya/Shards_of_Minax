using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChampagneToastTunic : Tunic
{
    [Constructable]
    public ChampagneToastTunic()
    {
        Name = "Champagne Toast Tunic";
        Hue = Utility.Random(300, 1100);
        Attributes.Luck = 10;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        Resistances.Cold = 5;
        Resistances.Fire = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChampagneToastTunic(Serial serial) : base(serial)
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
