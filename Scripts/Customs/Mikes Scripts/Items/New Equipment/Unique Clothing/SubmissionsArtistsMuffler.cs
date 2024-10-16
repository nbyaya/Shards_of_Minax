using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SubmissionsArtistsMuffler : Cap
{
    [Constructable]
    public SubmissionsArtistsMuffler()
    {
        Name = "Submission Artist's Muffler";
        Hue = Utility.Random(300, 2300);
        Attributes.AttackChance = 10;
        Attributes.RegenStam = 2;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
        Resistances.Physical = 10;
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SubmissionsArtistsMuffler(Serial serial) : base(serial)
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
