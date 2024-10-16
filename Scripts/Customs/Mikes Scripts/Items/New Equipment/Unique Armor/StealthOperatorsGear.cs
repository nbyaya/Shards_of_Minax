using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StealthOperatorsGear : StuddedLegs
{
    [Constructable]
    public StealthOperatorsGear()
    {
        Name = "Stealth Operator's Gear";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(25, 70);
        AbsorptionAttributes.EaterCold = 25;
        ArmorAttributes.LowerStatReq = 20;
        Attributes.NightSight = 1;
        Attributes.RegenStam = 10;
        SkillBonuses.SetValues(0, SkillName.Stealth, 25.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 20.0);
        ColdBonus = 20;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StealthOperatorsGear(Serial serial) : base(serial)
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
