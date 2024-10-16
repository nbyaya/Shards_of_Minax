using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BeastmastersCrown : PlateHelm
{
    [Constructable]
    public BeastmastersCrown()
    {
        Name = "Beastmaster's Crown";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(55, 70);
        AbsorptionAttributes.EaterKinetic = 25;
        ArmorAttributes.DurabilityBonus = 50;
        Attributes.BonusMana = 30;
        SkillBonuses.SetValues(0, SkillName.AnimalTaming, 25.0);
        ColdBonus = 15;
        EnergyBonus = 20;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BeastmastersCrown(Serial serial) : base(serial)
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
