using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HammerlordsLegplates : PlateLegs
{
    [Constructable]
    public HammerlordsLegplates()
    {
        Name = "Hammerlord's Legplates";
        Hue = Utility.Random(350, 650);
        BaseArmorRating = Utility.RandomMinMax(45, 80);
        AbsorptionAttributes.EaterCold = 10;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.BonusStam = 20;
        PhysicalBonus = 20;
        ColdBonus = 15;
        EnergyBonus = 5;
        FireBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HammerlordsLegplates(Serial serial) : base(serial)
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
