using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class LioneyesRemorse : HeaterShield
{
    [Constructable]
    public LioneyesRemorse()
    {
        Name = "Lioneye's Remorse";
        Hue = Utility.Random(200, 500);
        BaseArmorRating = Utility.RandomMinMax(40, 85);
        Attributes.BonusHits = 50;
        ArmorAttributes.DurabilityBonus = 20;
        Attributes.ReflectPhysical = 10;
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public LioneyesRemorse(Serial serial) : base(serial)
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
