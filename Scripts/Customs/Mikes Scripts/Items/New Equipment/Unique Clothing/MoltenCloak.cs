using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MoltenCloak : Cloak
{
    [Constructable]
    public MoltenCloak()
    {
        Name = "Molten Cloak";
        Hue = Utility.Random(200, 800);
        ClothingAttributes.DurabilityBonus = 5;
        Attributes.RegenHits = 2;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 20.0);
        Resistances.Fire = 25;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MoltenCloak(Serial serial) : base(serial)
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
