using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MinersSturdyBoots : Boots
{
    [Constructable]
    public MinersSturdyBoots()
    {
        Name = "Miner's Sturdy Boots";
        Hue = Utility.Random(600, 1600);
        ClothingAttributes.DurabilityBonus = 3;
        Attributes.BonusStr = 10;
        SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
        SkillBonuses.SetValues(1, SkillName.Lumberjacking, 15.0);
        Resistances.Physical = 20;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MinersSturdyBoots(Serial serial) : base(serial)
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
