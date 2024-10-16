using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GreavesOfTheFallenStars : PlateLegs
{
    [Constructable]
    public GreavesOfTheFallenStars()
    {
        Name = "Greaves of the Fallen Stars";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(65, 75);
        AbsorptionAttributes.EaterEnergy = 40;
        ArmorAttributes.LowerStatReq = 30;
        Attributes.BonusStam = 35;
        Attributes.BonusMana = 35;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Chivalry, 40.0);
        SkillBonuses.SetValues(1, SkillName.Bushido, 50.0);
        SkillBonuses.SetValues(2, SkillName.Ninjitsu, 30.0);
        PhysicalBonus = 18;
        FireBonus = 12;
        ColdBonus = 12;
        EnergyBonus = 18;
        PoisonBonus = 12;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GreavesOfTheFallenStars(Serial serial) : base(serial)
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
