using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HelmetOfTheOreWhisperer : Bandana
{
    [Constructable]
    public HelmetOfTheOreWhisperer()
    {
        Name = "Bandana of the Ore Whisperer";
        Hue = Utility.Random(400, 1400);
        ClothingAttributes.SelfRepair = 5;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
        SkillBonuses.SetValues(1, SkillName.ItemID, 15.0); // Assuming ItemID is a placeholder for a specific skill
        Resistances.Fire = 10;
        Resistances.Physical = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HelmetOfTheOreWhisperer(Serial serial) : base(serial)
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
