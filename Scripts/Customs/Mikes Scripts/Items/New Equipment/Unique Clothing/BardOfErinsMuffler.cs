using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BardOfErinsMuffler : Cap
{
    [Constructable]
    public BardOfErinsMuffler()
    {
        Name = "Bard of Erin's Muffler";
        Hue = Utility.Random(700, 2750);
        Attributes.BonusDex = 10;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        SkillBonuses.SetValues(1, SkillName.Peacemaking, 20.0);
        Resistances.Cold = 15;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BardOfErinsMuffler(Serial serial) : base(serial)
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
