using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StormforgedLeggings : PlateLegs
{
    [Constructable]
    public StormforgedLeggings()
    {
        Name = "Stormforged Leggings";
        Hue = Utility.Random(550, 850);
        BaseArmorRating = Utility.RandomMinMax(45, 75);
        AbsorptionAttributes.EaterCold = 10;
        ArmorAttributes.MageArmor = 1;
        Attributes.RegenStam = 5;
        Attributes.BonusHits = 20;
        SkillBonuses.SetValues(0, SkillName.Tinkering, 10.0);
        EnergyBonus = 20;
        ColdBonus = 10;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StormforgedLeggings(Serial serial) : base(serial)
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
