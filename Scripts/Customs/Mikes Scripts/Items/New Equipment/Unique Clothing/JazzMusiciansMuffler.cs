using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class JazzMusiciansMuffler : Cap
{
    [Constructable]
    public JazzMusiciansMuffler()
    {
        Name = "Jazz Musician's Muffler";
        Hue = Utility.Random(300, 2400);
        Attributes.BonusDex = 5;
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 25.0);
        SkillBonuses.SetValues(1, SkillName.Discordance, 15.0);
        Resistances.Cold = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public JazzMusiciansMuffler(Serial serial) : base(serial)
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
