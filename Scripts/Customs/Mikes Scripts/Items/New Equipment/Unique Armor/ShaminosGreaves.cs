using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShaminosGreaves : ChainLegs
{
    [Constructable]
    public ShaminosGreaves()
    {
        Name = "Shamino's Greaves";
        Hue = Utility.Random(250, 450);
        BaseArmorRating = Utility.RandomMinMax(40, 75);
        AbsorptionAttributes.EaterPoison = 10;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.BonusDex = 25;
        Attributes.AttackChance = 15;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 15;
        PhysicalBonus = 15;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShaminosGreaves(Serial serial) : base(serial)
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
