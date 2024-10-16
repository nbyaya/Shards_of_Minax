using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ReindeerFurCap : SkullCap
{
    [Constructable]
    public ReindeerFurCap()
    {
        Name = "Reindeer Fur Cap";
        Hue = Utility.Random(45, 2955);
        ClothingAttributes.SelfRepair = 3;
        Attributes.BonusStam = 10;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
        Resistances.Cold = 20;
        Resistances.Physical = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ReindeerFurCap(Serial serial) : base(serial)
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
