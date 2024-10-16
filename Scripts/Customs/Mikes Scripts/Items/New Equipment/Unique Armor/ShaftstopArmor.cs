using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ShaftstopArmor : BoneChest
{
    [Constructable]
    public ShaftstopArmor()
    {
        Name = "Shaftstop Armor";
        Hue = Utility.Random(500, 850);
        BaseArmorRating = Utility.RandomMinMax(40, 80);
        AbsorptionAttributes.EaterKinetic = 20;
        Attributes.BonusHits = 50;
        Attributes.ReflectPhysical = 20;
        PhysicalBonus = 40;
        EnergyBonus = 5;
        FireBonus = 10;
        ColdBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ShaftstopArmor(Serial serial) : base(serial)
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
