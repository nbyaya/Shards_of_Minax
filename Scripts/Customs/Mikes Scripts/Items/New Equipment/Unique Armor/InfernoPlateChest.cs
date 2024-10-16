using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class InfernoPlateChest : PlateChest
{
    [Constructable]
    public InfernoPlateChest()
    {
        Name = "Inferno PlateChest";
        Hue = Utility.Random(500, 600);
        BaseArmorRating = Utility.RandomMinMax(50, 85);
        AbsorptionAttributes.EaterFire = 25;
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusStr = 30;
        FireBonus = 30;
        EnergyBonus = 10;
        PoisonBonus = -5;
        PhysicalBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public InfernoPlateChest(Serial serial) : base(serial)
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
