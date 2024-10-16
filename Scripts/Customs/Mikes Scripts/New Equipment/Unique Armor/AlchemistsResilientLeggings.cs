using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AlchemistsResilientLeggings : LeatherLegs
{
    [Constructable]
    public AlchemistsResilientLeggings()
    {
        Name = "Alchemist's Resilient Leggings";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(25, 60);
        AbsorptionAttributes.ResonanceCold = 15;
        ArmorAttributes.MageArmor = 1;
        Attributes.RegenMana = 5;
        Attributes.LowerRegCost = 10;
        SkillBonuses.SetValues(0, SkillName.Herding, 15.0);
        ColdBonus = 15;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AlchemistsResilientLeggings(Serial serial) : base(serial)
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
