using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TerrasMysticRobe : FemaleStuddedChest
{
    [Constructable]
    public TerrasMysticRobe()
    {
        Name = "Terra's Mystic Robe";
        Hue = Utility.Random(600, 900);
        BaseArmorRating = Utility.RandomMinMax(25, 60);
        AbsorptionAttributes.EaterEnergy = 20;
        Attributes.BonusMana = 30;
        Attributes.SpellChanneling = 1;
        Attributes.EnhancePotions = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 15;
        PhysicalBonus = 5;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TerrasMysticRobe(Serial serial) : base(serial)
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
