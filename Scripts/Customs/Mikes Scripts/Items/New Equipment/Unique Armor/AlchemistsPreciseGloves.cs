using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AlchemistsPreciseGloves : LeatherGloves
{
    [Constructable]
    public AlchemistsPreciseGloves()
    {
        Name = "Alchemist's Precise Gloves";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(20, 55);
        AbsorptionAttributes.ResonanceEnergy = 10;
        ArmorAttributes.DurabilityBonus = 25;
        Attributes.BonusDex = 15;
        Attributes.LowerManaCost = 10;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 20.0);
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 5;
        PhysicalBonus = 5;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AlchemistsPreciseGloves(Serial serial) : base(serial)
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
