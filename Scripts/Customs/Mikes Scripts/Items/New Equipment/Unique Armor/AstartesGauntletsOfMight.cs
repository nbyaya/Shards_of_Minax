using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AstartesGauntletsOfMight : PlateGloves
{
    [Constructable]
    public AstartesGauntletsOfMight()
    {
        Name = "Astartes Gauntlets of Might";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(25, 85);
        AbsorptionAttributes.EaterFire = 20;
        ArmorAttributes.DurabilityBonus = 40;
        Attributes.BonusDex = 15;
        Attributes.WeaponDamage = 10;
        ColdBonus = 15;
        EnergyBonus = 10;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AstartesGauntletsOfMight(Serial serial) : base(serial)
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
