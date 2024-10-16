using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class SkinOfTheVipermagi : ChainChest
{
    [Constructable]
    public SkinOfTheVipermagi()
    {
        Name = "Skin of the Vipermagi";
        Hue = Utility.Random(200, 450);
        BaseArmorRating = Utility.RandomMinMax(35, 70);
        AbsorptionAttributes.ResonancePoison = 20;
        Attributes.LowerManaCost = 10;
        Attributes.SpellDamage = 20;
        ColdBonus = 15;
        EnergyBonus = 30;
        FireBonus = 20;
        PhysicalBonus = 15;
        PoisonBonus = 35;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public SkinOfTheVipermagi(Serial serial) : base(serial)
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
