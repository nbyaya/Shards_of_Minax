using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FortunesPlateChest : PlateChest
{
    [Constructable]
    public FortunesPlateChest()
    {
        Name = "Fortune's PlateChest";
        Hue = Utility.Random(700, 950);
        BaseArmorRating = Utility.RandomMinMax(50, 80);
        AbsorptionAttributes.EaterCold = 10;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.Luck = 200;
        ColdBonus = 15;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FortunesPlateChest(Serial serial) : base(serial)
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
