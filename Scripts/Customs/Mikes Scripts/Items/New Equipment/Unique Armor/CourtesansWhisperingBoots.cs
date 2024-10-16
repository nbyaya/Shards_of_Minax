using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CourtesansWhisperingBoots : LeatherLegs
{
    [Constructable]
    public CourtesansWhisperingBoots()
    {
        Name = "Courtesan's Whispering Boots";
        Hue = Utility.Random(100, 500);
        BaseArmorRating = Utility.RandomMinMax(25, 55);
        AbsorptionAttributes.EaterEnergy = 10;
        Attributes.RegenStam = 5;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 10.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CourtesansWhisperingBoots(Serial serial) : base(serial)
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
