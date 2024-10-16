using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RoyalGuardsBoots : ThighBoots
{
    [Constructable]
    public RoyalGuardsBoots()
    {
        Name = "Royal Guard's Boots";
        Hue = Utility.Random(600, 2600);
        ClothingAttributes.DurabilityBonus = 5;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        Resistances.Physical = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public RoyalGuardsBoots(Serial serial) : base(serial)
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
