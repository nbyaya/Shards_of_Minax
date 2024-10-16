using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BootsOfTheNetherTraveller : LeatherLegs
{
    [Constructable]
    public BootsOfTheNetherTraveller()
    {
        Name = "Boots of the Nether Traveller";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterFire = 30;
        Attributes.RegenStam = 5;
        Attributes.BonusHits = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.ItemID, 10.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 25;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BootsOfTheNetherTraveller(Serial serial) : base(serial)
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
