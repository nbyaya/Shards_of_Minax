using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SolarisLorica : PlateChest
{
    [Constructable]
    public SolarisLorica()
    {
        Name = "Solaris Lorica";
        Hue = Utility.Random(300, 700);
        BaseArmorRating = Utility.RandomMinMax(30, 80);
        AbsorptionAttributes.ResonancePoison = 40;
        ArmorAttributes.SelfRepair = 10;
        Attributes.LowerManaCost = 10;
        Attributes.SpellDamage = 15;
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SolarisLorica(Serial serial) : base(serial)
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
