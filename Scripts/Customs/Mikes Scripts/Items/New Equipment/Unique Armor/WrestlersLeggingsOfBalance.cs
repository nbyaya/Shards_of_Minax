using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WrestlersLeggingsOfBalance : PlateLegs
{
    [Constructable]
    public WrestlersLeggingsOfBalance()
    {
        Name = "Wrestler's Leggings of Balance";
        Hue = Utility.Random(10, 250);
        BaseArmorRating = Utility.RandomMinMax(45, 65);
        ArmorAttributes.LowerStatReq = 10;
        Attributes.BonusStam = 15;
        Attributes.RegenMana = 5;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 10.0);
        ColdBonus = 10;
        EnergyBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WrestlersLeggingsOfBalance(Serial serial) : base(serial)
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
