using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SilentStepTabi : LeatherLegs
{
    [Constructable]
    public SilentStepTabi()
    {
        Name = "Silent Step Tabi";
        Hue = Utility.Random(500, 600);
        BaseArmorRating = Utility.RandomMinMax(25, 55);
        AbsorptionAttributes.ResonanceKinetic = 10;
        Attributes.BonusStam = 10;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SilentStepTabi(Serial serial) : base(serial)
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
