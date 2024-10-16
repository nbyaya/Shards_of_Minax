using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class CourtierDashingBoots : LeatherLegs
{
    [Constructable]
    public CourtierDashingBoots()
    {
        Name = "Courtier's Dashing Boots";
        Hue = Utility.Random(700, 950);
        BaseArmorRating = Utility.RandomMinMax(30, 58);
        AbsorptionAttributes.EaterEnergy = 10;
        ArmorAttributes.MageArmor = 1;
        Attributes.RegenStam = 5;
        Attributes.CastSpeed = 1;
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        ColdBonus = 5;
        EnergyBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 5;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public CourtierDashingBoots(Serial serial) : base(serial)
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
