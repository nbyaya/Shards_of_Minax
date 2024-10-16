using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BladedancersPlateArms : PlateArms
{
    [Constructable]
    public BladedancersPlateArms()
    {
        Name = "Blade Dancer's PlateArms";
        Hue = Utility.Random(600, 900);
        BaseArmorRating = Utility.RandomMinMax(40, 70);
        AbsorptionAttributes.EaterKinetic = 10;
        ArmorAttributes.LowerStatReq = 10;
        Attributes.BonusHits = 15;
        Attributes.DefendChance = 7;
        SkillBonuses.SetValues(0, SkillName.Swords, 10.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 15;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BladedancersPlateArms(Serial serial) : base(serial)
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
