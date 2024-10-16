using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PhilosophersGreaves : PlateLegs
{
    [Constructable]
    public PhilosophersGreaves()
    {
        Name = "Philosopher's Greaves";
        Hue = Utility.Random(1, 500);
        BaseArmorRating = Utility.RandomMinMax(60, 85);
        AbsorptionAttributes.EaterEnergy = 25;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusMana = 30;
        Attributes.RegenStam = 10;
        SkillBonuses.SetValues(0, SkillName.ItemID, 25.0); // Assuming ItemID is a skill here
        ColdBonus = 20;
        EnergyBonus = 25;
        FireBonus = 15;
        PhysicalBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PhilosophersGreaves(Serial serial) : base(serial)
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
