using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AstartesShoulderGuard : PlateArms
{
    [Constructable]
    public AstartesShoulderGuard()
    {
        Name = "Astartes Shoulder Guard";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(26, 86);
        AbsorptionAttributes.EaterCold = 20;
        ArmorAttributes.DurabilityBonus = 40;
        Attributes.ReflectPhysical = 10;
        Attributes.AttackChance = 10;
        ColdBonus = 20;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AstartesShoulderGuard(Serial serial) : base(serial)
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
