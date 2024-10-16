using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BladedancersCloseHelm : CloseHelm
{
    [Constructable]
    public BladedancersCloseHelm()
    {
        Name = "Blade Dancer's CloseHelm";
        Hue = Utility.Random(600, 900);
        BaseArmorRating = Utility.RandomMinMax(40, 75);
        AbsorptionAttributes.EaterEnergy = 10;
        ArmorAttributes.LowerStatReq = 10;
        Attributes.BonusInt = 5;
        Attributes.ReflectPhysical = 10;
        SkillBonuses.SetValues(0, SkillName.Parry, 15.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BladedancersCloseHelm(Serial serial) : base(serial)
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
