using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SilksOfTheVictor : StuddedChest
{
    [Constructable]
    public SilksOfTheVictor()
    {
        Name = "Silks of the Victor";
        Hue = Utility.Random(100, 400);
        BaseArmorRating = Utility.RandomMinMax(30, 65);
        ArmorAttributes.LowerStatReq = 10;
        Attributes.BonusStr = 20;
        Attributes.BonusMana = 25;
        Attributes.Luck = 15;
        PhysicalBonus = 15;
        ColdBonus = 10;
        EnergyBonus = 15;
        FireBonus = 10;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SilksOfTheVictor(Serial serial) : base(serial)
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
