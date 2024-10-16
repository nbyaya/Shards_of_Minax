using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PyrePlateHelm : PlateHelm
{
    [Constructable]
    public PyrePlateHelm()
    {
        Name = "Pyre PlateHelm";
        Hue = Utility.Random(500, 600);
        BaseArmorRating = Utility.RandomMinMax(50, 80);
        AbsorptionAttributes.CastingFocus = 10;
        ArmorAttributes.DurabilityBonus = 20;
        FireBonus = 20;
        EnergyBonus = 5;
        PoisonBonus = -5;
        PhysicalBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PyrePlateHelm(Serial serial) : base(serial)
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
