using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SirensResonance : PlateChest
{
    [Constructable]
    public SirensResonance()
    {
        Name = "Siren's Resonance";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(60, 100);
        AbsorptionAttributes.ResonanceKinetic = 25;
        ArmorAttributes.DurabilityBonus = 50;
        Attributes.BonusMana = 50;
        Attributes.EnhancePotions = 15;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 25.0);
        SkillBonuses.SetValues(1, SkillName.Provocation, 20.0);
        ColdBonus = 20;
        EnergyBonus = 15;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SirensResonance(Serial serial) : base(serial)
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
