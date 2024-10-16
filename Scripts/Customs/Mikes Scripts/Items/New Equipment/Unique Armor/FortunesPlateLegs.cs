using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FortunesPlateLegs : PlateLegs
{
    [Constructable]
    public FortunesPlateLegs()
    {
        Name = "Fortune's PlateLegs";
        Hue = Utility.Random(700, 950);
        BaseArmorRating = Utility.RandomMinMax(45, 75);
        AbsorptionAttributes.EaterEnergy = 10;
        ArmorAttributes.MageArmor = 1;
        Attributes.Luck = 175;
        SkillBonuses.SetValues(0, SkillName.DetectHidden, 10.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FortunesPlateLegs(Serial serial) : base(serial)
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
