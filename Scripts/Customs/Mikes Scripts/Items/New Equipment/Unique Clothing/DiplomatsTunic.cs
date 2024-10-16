using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DiplomatsTunic : Tunic
{
    [Constructable]
    public DiplomatsTunic()
    {
        Name = "Diplomat's Tunic";
        Hue = Utility.Random(250, 2150);
        Attributes.BonusInt = 20;
        Attributes.LowerManaCost = 10;
        SkillBonuses.SetValues(0, SkillName.Peacemaking, 70.0); // Assuming Negotiation or similar skill
        Resistances.Physical = 10;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DiplomatsTunic(Serial serial) : base(serial)
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
