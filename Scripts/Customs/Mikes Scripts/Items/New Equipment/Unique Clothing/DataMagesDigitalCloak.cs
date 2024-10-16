using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DataMagesDigitalCloak : Cloak
{
    [Constructable]
    public DataMagesDigitalCloak()
    {
        Name = "Data Mage's Digital Cloak";
        Hue = Utility.Random(950, 2800);
        Attributes.SpellChanneling = 1;
        Attributes.CastRecovery = 5;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Lockpicking, 20.0);
        Resistances.Energy = 25;
		XmlAttach.AttachTo(this, new XmlLevelItem());
        // Note: Chaos resistance does not exist in ServUO; will ignore it
    }

    public DataMagesDigitalCloak(Serial serial) : base(serial)
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
