using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GlovesOfTransmutation : PlateGloves
{
    [Constructable]
    public GlovesOfTransmutation()
    {
        Name = "Gloves of Transmutation";
        Hue = Utility.Random(1, 500);
        BaseArmorRating = Utility.RandomMinMax(60, 85);
        AbsorptionAttributes.EaterFire = 25;
        Attributes.BonusDex = 20;
        Attributes.EnhancePotions = 30;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 25.0);
        ColdBonus = 15;
        EnergyBonus = 20;
        FireBonus = 25;
        PhysicalBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GlovesOfTransmutation(Serial serial) : base(serial)
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
