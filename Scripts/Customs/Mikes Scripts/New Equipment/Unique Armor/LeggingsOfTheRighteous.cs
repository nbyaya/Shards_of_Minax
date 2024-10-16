using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LeggingsOfTheRighteous : PlateLegs
{
    [Constructable]
    public LeggingsOfTheRighteous()
    {
        Name = "Leggings of the Righteous";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.EaterFire = 30;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.BonusStam = 40;
        Attributes.RegenStam = 6;
        SkillBonuses.SetValues(0, SkillName.Bushido, 20.0);
        ColdBonus = 20;
        EnergyBonus = 20;
        FireBonus = 25;
        PhysicalBonus = 25;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LeggingsOfTheRighteous(Serial serial) : base(serial)
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
