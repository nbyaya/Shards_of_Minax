using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GlovesOfStonemasonry : Boots
{
    [Constructable]
    public GlovesOfStonemasonry()
    {
        Name = "Boots of Stonemasonry";
        Hue = Utility.Random(600, 1600);
        ClothingAttributes.DurabilityBonus = 4;
        Attributes.LowerRegCost = 5;
        SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
        Resistances.Physical = 25;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GlovesOfStonemasonry(Serial serial) : base(serial)
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
