using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CourtesansGracefulKimono : PlainDress
{
    [Constructable]
    public CourtesansGracefulKimono()
    {
        Name = "Courtesan's Graceful Kimono";
        Hue = Utility.Random(100, 2500);
        Attributes.BonusDex = 10;
        Attributes.BonusInt = 10;
        SkillBonuses.SetValues(0, SkillName.Provocation, 20.0);
        SkillBonuses.SetValues(1, SkillName.Musicianship, 20.0);
        Resistances.Energy = 10;
        Resistances.Physical = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CourtesansGracefulKimono(Serial serial) : base(serial)
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
