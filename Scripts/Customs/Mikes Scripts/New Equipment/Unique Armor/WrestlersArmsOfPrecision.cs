using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WrestlersArmsOfPrecision : PlateArms
{
    [Constructable]
    public WrestlersArmsOfPrecision()
    {
        Name = "Wrestler's Arms of Precision";
        Hue = Utility.Random(10, 250);
        BaseArmorRating = Utility.RandomMinMax(45, 65);
        ArmorAttributes.SelfRepair = 5;
        Attributes.BonusDex = 15;
        Attributes.DefendChance = 10;
        SkillBonuses.SetValues(2, SkillName.Wrestling, 10.0);
        ColdBonus = 10;
        EnergyBonus = 5;
        FireBonus = 10;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WrestlersArmsOfPrecision(Serial serial) : base(serial)
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
