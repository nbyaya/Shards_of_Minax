using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GauntletsOfPrecision : LeatherGloves
{
    [Constructable]
    public GauntletsOfPrecision()
    {
        Name = "Gauntlets of Precision";
        Hue = Utility.Random(100, 600);
        BaseArmorRating = Utility.RandomMinMax(25, 55);
        ArmorAttributes.DurabilityBonus = 10;
        Attributes.BonusDex = 15;
        Attributes.EnhancePotions = 10;
        SkillBonuses.SetValues(0, SkillName.Cooking, 10.0);
        ColdBonus = 5;
        EnergyBonus = 10;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GauntletsOfPrecision(Serial serial) : base(serial)
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
