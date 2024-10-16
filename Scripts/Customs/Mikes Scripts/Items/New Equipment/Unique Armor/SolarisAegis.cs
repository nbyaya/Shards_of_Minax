using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SolarisAegis : HeaterShield
{
    [Constructable]
    public SolarisAegis()
    {
        Name = "Solaris Aegis";
        Hue = Utility.Random(1, 250);
        BaseArmorRating = Utility.RandomMinMax(40, 90);
        AbsorptionAttributes.EaterFire = 40;
        Attributes.RegenHits = 5;
        ColdBonus = -20;
        FireBonus = 20;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SolarisAegis(Serial serial) : base(serial)
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
