using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GreyWanderersStride : PlateLegs
{
    [Constructable]
    public GreyWanderersStride()
    {
        Name = "Grey Wanderer's Stride";
        Hue = Utility.Random(400, 600);
        BaseArmorRating = Utility.RandomMinMax(55, 85);
        AbsorptionAttributes.EaterEnergy = 20;
        ArmorAttributes.SelfRepair = 10;
        Attributes.RegenStam = 10;
        Attributes.BonusDex = 20;
        SkillBonuses.SetValues(0, SkillName.Tracking, 20.0);
        ColdBonus = 20;
        EnergyBonus = 20;
        FireBonus = 15;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GreyWanderersStride(Serial serial) : base(serial)
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
