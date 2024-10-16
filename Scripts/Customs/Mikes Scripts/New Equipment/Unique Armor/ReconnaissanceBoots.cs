using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ReconnaissanceBoots : LeatherLegs
{
    [Constructable]
    public ReconnaissanceBoots()
    {
        Name = "Reconnaissance Boots";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 65);
        AbsorptionAttributes.EaterEnergy = 20;
        ArmorAttributes.LowerStatReq = 15;
        Attributes.BonusDex = 15;
        Attributes.LowerManaCost = 10;
        SkillBonuses.SetValues(0, SkillName.Tracking, 25.0);
        ColdBonus = 10;
        EnergyBonus = 25;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ReconnaissanceBoots(Serial serial) : base(serial)
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
