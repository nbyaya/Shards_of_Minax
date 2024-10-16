using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BlazePlateLegs : PlateLegs
{
    [Constructable]
    public BlazePlateLegs()
    {
        Name = "Blaze PlateLegs";
        Hue = Utility.Random(500, 600);
        BaseArmorRating = Utility.RandomMinMax(45, 80);
        AbsorptionAttributes.ResonanceFire = 20;
        Attributes.BonusDex = 25;
        FireBonus = 25;
        EnergyBonus = 10;
        PoisonBonus = -5;
        PhysicalBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BlazePlateLegs(Serial serial) : base(serial)
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
