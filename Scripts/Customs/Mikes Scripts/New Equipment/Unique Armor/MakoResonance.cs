using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class MakoResonance : ChainCoif
{
    [Constructable]
    public MakoResonance()
    {
        Name = "Mako Resonance";
        Hue = Utility.Random(200, 600);
        BaseArmorRating = Utility.RandomMinMax(35, 70);
        AbsorptionAttributes.EaterEnergy = 25;
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusMana = 40;
        Attributes.EnhancePotions = 20;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        EnergyBonus = 20;
        FireBonus = 10;
        PhysicalBonus = 10;
        ColdBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public MakoResonance(Serial serial) : base(serial)
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
