using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EmberPlateArms : PlateArms
{
    [Constructable]
    public EmberPlateArms()
    {
        Name = "Ember PlateArms";
        Hue = Utility.Random(500, 600);
        BaseArmorRating = Utility.RandomMinMax(40, 75);
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusInt = 20;
        FireBonus = 20;
        EnergyBonus = 5;
        PoisonBonus = -5;
        PhysicalBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EmberPlateArms(Serial serial) : base(serial)
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
