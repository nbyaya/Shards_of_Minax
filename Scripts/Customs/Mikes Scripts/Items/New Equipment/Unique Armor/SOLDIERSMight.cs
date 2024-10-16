using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SOLDIERSMight : PlateArms
{
    [Constructable]
    public SOLDIERSMight()
    {
        Name = "SOLDIER's Might";
        Hue = Utility.Random(100, 500);
        BaseArmorRating = Utility.RandomMinMax(45, 85);
        AbsorptionAttributes.EaterKinetic = 15;
        ArmorAttributes.DurabilityBonus = 30;
        Attributes.BonusStam = 30;
        Attributes.AttackChance = 20;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        PhysicalBonus = 20;
        FireBonus = 15;
        EnergyBonus = 10;
        ColdBonus = 5;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SOLDIERSMight(Serial serial) : base(serial)
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
