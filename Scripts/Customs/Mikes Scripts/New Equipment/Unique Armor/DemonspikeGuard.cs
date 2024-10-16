using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DemonspikeGuard : PlateChest
{
    [Constructable]
    public DemonspikeGuard()
    {
        Name = "Demonspike Guard";
        Hue = Utility.Random(100, 500);
        BaseArmorRating = Utility.RandomMinMax(60, 100);
        ArmorAttributes.DurabilityBonus = 25;
        Attributes.DefendChance = 20;
        Attributes.ReflectPhysical = 15;
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 25;
        PhysicalBonus = 25;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DemonspikeGuard(Serial serial) : base(serial)
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
