using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WardenOfTheWestsArms : PlateArms
{
    [Constructable]
    public WardenOfTheWestsArms()
    {
        Name = "Warden of the West's Arms";
        Hue = Utility.Random(2001, 3000);
        BaseArmorRating = Utility.RandomMinMax(65, 95);
        ArmorAttributes.MageArmor = 1;
        ArmorAttributes.ReactiveParalyze = 1;
        Attributes.BonusStam = 20;
        Attributes.ReflectPhysical = 10;
        SkillBonuses.SetValues(0, SkillName.Parry, 45.0);
        SkillBonuses.SetValues(1, SkillName.Swords, 30.0);
        PhysicalBonus = 18;
        FireBonus = 12;
        ColdBonus = 18;
        EnergyBonus = 12;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WardenOfTheWestsArms(Serial serial) : base(serial)
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
