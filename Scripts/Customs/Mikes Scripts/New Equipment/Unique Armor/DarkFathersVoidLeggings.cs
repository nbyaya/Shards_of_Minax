using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DarkFathersVoidLeggings : PlateLegs
{
    [Constructable]
    public DarkFathersVoidLeggings()
    {
        Name = "Dark Father's Void Leggings";
        Hue = Utility.Random(500, 750);
        BaseArmorRating = Utility.RandomMinMax(60, 90);
        AbsorptionAttributes.EaterCold = 30;
        ArmorAttributes.LowerStatReq = 40;
        Attributes.RegenStam = 10;
        Attributes.BonusStr = 25;
        SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 25.0);
        ColdBonus = 25;
        EnergyBonus = 20;
        FireBonus = 20;
        PhysicalBonus = 20;
        PoisonBonus = 25;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DarkFathersVoidLeggings(Serial serial) : base(serial)
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
