using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AnglersSeabreezeCloak : Cloak
{
    [Constructable]
    public AnglersSeabreezeCloak()
    {
        Name = "Angler's Seabreeze Cloak";
        Hue = Utility.Random(400, 2900);
        ClothingAttributes.SelfRepair = 3;
        Attributes.BonusDex = 10;
        Attributes.LowerManaCost = 5;
        SkillBonuses.SetValues(0, SkillName.Fishing, 25.0);
        Resistances.Cold = 20;
        Resistances.Fire = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AnglersSeabreezeCloak(Serial serial) : base(serial)
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
