using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WildwalkersGreaves : LeatherLegs
{
    [Constructable]
    public WildwalkersGreaves()
    {
        Name = "Wildwalker's Greaves";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(55, 70);
        AbsorptionAttributes.EaterFire = 25;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusStam = 30;
        SkillBonuses.SetValues(0, SkillName.Herding, 20.0);
        ColdBonus = 20;
        EnergyBonus = 10;
        FireBonus = 25;
        PhysicalBonus = 15;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WildwalkersGreaves(Serial serial) : base(serial)
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
