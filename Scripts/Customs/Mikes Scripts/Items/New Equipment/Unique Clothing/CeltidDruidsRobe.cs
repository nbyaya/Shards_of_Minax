using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CeltidDruidsRobe : Robe
{
    [Constructable]
    public CeltidDruidsRobe()
    {
        Name = "Celtic Druid's Robe";
        Hue = Utility.Random(500, 2550);
        ClothingAttributes.MageArmor = 1;
        Attributes.BonusInt = 20;
        Attributes.RegenMana = 3;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
        SkillBonuses.SetValues(1, SkillName.Herding, 15.0);
        Resistances.Energy = 15;
        Resistances.Poison = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CeltidDruidsRobe(Serial serial) : base(serial)
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
