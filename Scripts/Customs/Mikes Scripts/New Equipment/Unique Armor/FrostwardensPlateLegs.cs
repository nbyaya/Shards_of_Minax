using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostwardensPlateLegs : PlateLegs
{
    [Constructable]
    public FrostwardensPlateLegs()
    {
        Name = "Frostwarden's PlateLegs";
        Hue = Utility.Random(600, 650);
        BaseArmorRating = Utility.RandomMinMax(48, 77);
        AbsorptionAttributes.EaterCold = 20;
        ArmorAttributes.LowerStatReq = 15;
        Attributes.BonusDex = 10;
        Attributes.Luck = 20;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 15.0);
        ColdBonus = 25;
        EnergyBonus = 10;
        FireBonus = 0;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostwardensPlateLegs(Serial serial) : base(serial)
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
