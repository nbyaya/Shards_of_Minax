using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AstartesWarBoots : PlateLegs
{
    [Constructable]
    public AstartesWarBoots()
    {
        Name = "Astartes War Boots";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(24, 84);
        AbsorptionAttributes.EaterPoison = 15;
        ArmorAttributes.DurabilityBonus = 35;
        Attributes.BonusHits = 10;
        Attributes.RegenStam = 5;
        ColdBonus = 15;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AstartesWarBoots(Serial serial) : base(serial)
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
