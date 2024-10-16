using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WitchesBindingGloves : LeatherGloves
{
    [Constructable]
    public WitchesBindingGloves()
    {
        Name = "Witch's Binding Gloves";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(20, 50);
        AbsorptionAttributes.ResonanceCold = 15;
        Attributes.EnhancePotions = 20;
        Attributes.LowerRegCost = 10;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0);
        ColdBonus = 15;
        EnergyBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WitchesBindingGloves(Serial serial) : base(serial)
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
