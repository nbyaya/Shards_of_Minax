using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ElementalistsProtectiveCloak : Cloak
{
    [Constructable]
    public ElementalistsProtectiveCloak()
    {
        Name = "Elementalist's Protective Cloak";
        Hue = Utility.Random(500, 2700);
        Attributes.CastRecovery = 2;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        Resistances.Cold = 15;
        Resistances.Fire = 15;
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ElementalistsProtectiveCloak(Serial serial) : base(serial)
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
