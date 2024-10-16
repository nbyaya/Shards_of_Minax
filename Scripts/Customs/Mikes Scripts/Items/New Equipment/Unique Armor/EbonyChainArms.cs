using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EbonyChainArms : LeatherArms
{
    [Constructable]
    public EbonyChainArms()
    {
        Name = "Ebony Chain Arms";
        Hue = Utility.Random(750, 900);
        BaseArmorRating = Utility.RandomMinMax(40, 75);
        AbsorptionAttributes.EaterKinetic = 20;
        ArmorAttributes.DurabilityBonus = 25;
        Attributes.AttackChance = 10;
        Attributes.DefendChance = 10;
        FireBonus = 15;
        EnergyBonus = 10;
        ColdBonus = 10;
        PhysicalBonus = 20;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EbonyChainArms(Serial serial) : base(serial)
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
