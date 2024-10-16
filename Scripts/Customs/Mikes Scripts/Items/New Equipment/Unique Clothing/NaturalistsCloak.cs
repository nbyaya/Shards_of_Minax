using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NaturalistsCloak : Cloak
{
    [Constructable]
    public NaturalistsCloak()
    {
        Name = "Naturalist's Cloak";
        Hue = Utility.Random(450, 2400);
        ClothingAttributes.SelfRepair = 2;
        Attributes.BonusMana = 10;
        SkillBonuses.SetValues(0, SkillName.AnimalLore, 20.0);
        SkillBonuses.SetValues(1, SkillName.AnimalTaming, 10.0);
        Resistances.Cold = 10;
        Resistances.Physical = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NaturalistsCloak(Serial serial) : base(serial)
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
