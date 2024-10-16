using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FilmNoirDetectivesTrenchCoat : Surcoat
{
    [Constructable]
    public FilmNoirDetectivesTrenchCoat()
    {
        Name = "Film Noir Detective's Trench Coat";
        Hue = Utility.Random(1, 2900);
        Attributes.NightSight = 1;
        Attributes.BonusInt = 10;
        SkillBonuses.SetValues(0, SkillName.DetectHidden, 25.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 20.0);
        Resistances.Physical = 15;
        Resistances.Energy = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FilmNoirDetectivesTrenchCoat(Serial serial) : base(serial)
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
