using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MelodiousMuffler : Cap
{
    [Constructable]
    public MelodiousMuffler()
    {
        Name = "Melodious Muffler";
        Hue = Utility.Random(400, 2600);
        Attributes.BonusInt = 10;
        Attributes.CastSpeed = 1;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 25.0);
        SkillBonuses.SetValues(1, SkillName.Discordance, 15.0);
        Resistances.Fire = 5;
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MelodiousMuffler(Serial serial) : base(serial)
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
