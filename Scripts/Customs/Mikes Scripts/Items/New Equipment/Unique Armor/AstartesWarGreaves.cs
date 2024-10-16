using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AstartesWarGreaves : PlateLegs
{
    [Constructable]
    public AstartesWarGreaves()
    {
        Name = "Astartes War Greaves";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(28, 88);
        AbsorptionAttributes.EaterKinetic = 25;
        ArmorAttributes.DurabilityBonus = 45;
        Attributes.BonusStam = 20;
        Attributes.DefendChance = 10;
        ColdBonus = 20;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AstartesWarGreaves(Serial serial) : base(serial)
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
