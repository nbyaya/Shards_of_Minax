using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DarkKnightsVoidLeggings : PlateLegs
{
    [Constructable]
    public DarkKnightsVoidLeggings()
    {
        Name = "Dark Knight's Void Leggings";
        Hue = Utility.Random(10, 20);
        BaseArmorRating = Utility.RandomMinMax(65, 85);
        AbsorptionAttributes.EaterEnergy = 25;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusMana = 50;
        Attributes.CastRecovery = 2;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        ColdBonus = 20;
        EnergyBonus = 30;
        FireBonus = 25;
        PhysicalBonus = 25;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DarkKnightsVoidLeggings(Serial serial) : base(serial)
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
