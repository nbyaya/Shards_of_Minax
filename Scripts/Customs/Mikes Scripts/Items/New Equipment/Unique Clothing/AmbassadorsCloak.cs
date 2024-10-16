using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AmbassadorsCloak : Cloak
{
    [Constructable]
    public AmbassadorsCloak()
    {
        Name = "Ambassador's Cloak";
        Hue = Utility.Random(50, 2950);
        Attributes.BonusStr = 10;
        Attributes.CastSpeed = 1;
        SkillBonuses.SetValues(0, SkillName.Discordance, 20.0);
        SkillBonuses.SetValues(1, SkillName.Provocation, 15.0);
        Resistances.Fire = 10;
        Resistances.Energy = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AmbassadorsCloak(Serial serial) : base(serial)
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
