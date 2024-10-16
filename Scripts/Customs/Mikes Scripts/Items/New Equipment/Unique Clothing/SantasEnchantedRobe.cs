using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SantasEnchantedRobe : Robe
{
    [Constructable]
    public SantasEnchantedRobe()
    {
        Name = "Santa's Enchanted Robe";
        Hue = Utility.Random(30, 2935);
        ClothingAttributes.SelfRepair = 5;
        Attributes.BonusInt = 15;
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.Peacemaking, 25.0);
        Resistances.Cold = 20;
        Resistances.Fire = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SantasEnchantedRobe(Serial serial) : base(serial)
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
