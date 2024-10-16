using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FalconersCoif : LeatherCap
{
    [Constructable]
    public FalconersCoif()
    {
        Name = "Falconer's Coif";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(18, 53);
        AbsorptionAttributes.EaterCold = 25;
        ArmorAttributes.SelfRepair = 5;
        Attributes.BonusDex = 25;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0);
        ColdBonus = 15;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FalconersCoif(Serial serial) : base(serial)
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
