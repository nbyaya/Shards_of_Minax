using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PinUpHalterDress : PlainDress
{
    [Constructable]
    public PinUpHalterDress()
    {
        Name = "Pin-Up Halter Dress";
        Hue = Utility.Random(400, 2300);
        Attributes.Luck = 15;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Peacemaking, 15.0);
        Resistances.Cold = 15;
        Resistances.Poison = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PinUpHalterDress(Serial serial) : base(serial)
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
