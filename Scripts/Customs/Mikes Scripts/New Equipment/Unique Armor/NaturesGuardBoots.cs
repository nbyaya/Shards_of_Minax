using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class NaturesGuardBoots : LeatherLegs
{
    [Constructable]
    public NaturesGuardBoots()
    {
        Name = "Nature's Guard Boots";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 55);
        AbsorptionAttributes.EaterPoison = 30;
        AbsorptionAttributes.EaterEnergy = 20;
        Attributes.BonusStam = 30;
        Attributes.NightSight = 1;
        SkillBonuses.SetValues(0, SkillName.Veterinary, 20.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public NaturesGuardBoots(Serial serial) : base(serial)
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
