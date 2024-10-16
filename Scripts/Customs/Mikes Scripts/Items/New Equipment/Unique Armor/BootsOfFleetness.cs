using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BootsOfFleetness : LeatherLegs
{
    [Constructable]
    public BootsOfFleetness()
    {
        Name = "Boots of Fleetness";
        Hue = Utility.Random(200, 650);
        BaseArmorRating = Utility.RandomMinMax(28, 58);
        AbsorptionAttributes.EaterEnergy = 10;
        ArmorAttributes.SelfRepair = 3;
        Attributes.BonusStam = 10;
        Attributes.RegenStam = 3;
        SkillBonuses.SetValues(0, SkillName.Cooking, 5.0);
        ColdBonus = 5;
        EnergyBonus = 15;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BootsOfFleetness(Serial serial) : base(serial)
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
