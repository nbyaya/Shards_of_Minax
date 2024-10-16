using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FortunesPlateArms : PlateArms
{
    [Constructable]
    public FortunesPlateArms()
    {
        Name = "Fortune's PlateArms";
        Hue = Utility.Random(700, 950);
        BaseArmorRating = Utility.RandomMinMax(35, 70);
        AbsorptionAttributes.EaterKinetic = 10;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.Luck = 125;
        SkillBonuses.SetValues(0, SkillName.Carpentry, 10.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 15;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FortunesPlateArms(Serial serial) : base(serial)
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
