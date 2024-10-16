using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TunicOfTheWild : LeatherChest
{
    [Constructable]
    public TunicOfTheWild()
    {
        Name = "Tunic of the Wild";
        Hue = Utility.Random(1, 1000);
        BaseArmorRating = Utility.RandomMinMax(20, 70);
        AbsorptionAttributes.EaterEnergy = 25;
        ArmorAttributes.MageArmor = 1;
        Attributes.BonusStam = 30;
        Attributes.RegenStam = 5;
        SkillBonuses.SetValues(0, SkillName.Archery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tracking, 15.0);
        ColdBonus = 10;
        EnergyBonus = 20;
        FireBonus = 10;
        PhysicalBonus = 10;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TunicOfTheWild(Serial serial) : base(serial)
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