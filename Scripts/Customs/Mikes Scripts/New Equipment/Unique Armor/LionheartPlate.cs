using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LionheartPlate : PlateChest
{
    [Constructable]
    public LionheartPlate()
    {
        Name = "Lionheart Plate";
        Hue = Utility.Random(400, 750);
        BaseArmorRating = Utility.RandomMinMax(45, 85);
        AbsorptionAttributes.EaterCold = 15;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.BonusStr = 25;
        Attributes.BonusDex = 20;
        Attributes.BonusInt = 15;
        ColdBonus = 25;
        EnergyBonus = 15;
        FireBonus = 25;
        PhysicalBonus = 20;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LionheartPlate(Serial serial) : base(serial)
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
