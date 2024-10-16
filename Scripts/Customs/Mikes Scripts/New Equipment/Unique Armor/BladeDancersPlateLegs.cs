using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BladeDancersPlateLegs : PlateLegs
{
    [Constructable]
    public BladeDancersPlateLegs()
    {
        Name = "Blade Dancer's PlateLegs";
        Hue = Utility.Random(600, 900);
        BaseArmorRating = Utility.RandomMinMax(45, 80);
        AbsorptionAttributes.EaterFire = 10;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.BonusDex = 10;
        Attributes.AttackChance = 8;
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 15;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BladeDancersPlateLegs(Serial serial) : base(serial)
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
