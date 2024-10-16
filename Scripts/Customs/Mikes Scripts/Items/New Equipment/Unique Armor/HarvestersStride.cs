using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HarvestersStride : LeatherLegs
{
    [Constructable]
    public HarvestersStride()
    {
        Name = "Harvester's Stride";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.EaterKinetic = 25;
        Attributes.BonusDex = 30;
        Attributes.RegenStam = 8;
        SkillBonuses.SetValues(0, SkillName.Tracking, 25.0);
        ColdBonus = 20;
        EnergyBonus = 20;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public HarvestersStride(Serial serial) : base(serial)
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
